using PX.BarcodeProcessing;
using PX.Objects.IN;
using PX.Objects.IN.WMS;
using System.Collections.Generic;
using PX.Data;
using PX.Common;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
    using WMSBase = WarehouseManagementSystem<INScanCount, INScanCount.Host>;

    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class INScanCount: WMSBase
    {
        public PXSetupOptional<INScanSetup,
            Where<INScanSetup.branchID.IsEqual<AccessInfo.branchID.FromCurrent>>> Setup;
        public class Host : INPICountEntry { }

        // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
        public new class QtySupport : WMSBase.QtySupport { }

        protected override bool UseQtyCorrectection =>
            Setup.Current.UseDefaultQtyInCount != true;

        protected override IEnumerable<ScanMode<INScanCount>> CreateScanModes()
        {
            yield return new CountMode();
        }

        public virtual INPIHeader Document => DocumentView.Current;
        public virtual PXSelectBase<INPIHeader> DocumentView => Graph.PIHeader;
        public virtual PXSelectBase<INPIDetail> Details => Graph.PIDetail;

        protected virtual bool IsDocumentStatusEditable(string status) =>
        status == INPIHdrStatus.Counting;

        public override bool DocumentIsEditable =>
            base.DocumentIsEditable && IsDocumentStatusEditable(Document.Status);

        public sealed class CountMode : ScanMode
        {
            public const string Value = "INCO";
            public class value : BqlString.Constant<value>
            {
                public value() : base(CountMode.Value) { }
            }

            public override string Code => Value;
            public override string Description => Msg.Description;

            [PXLocalizable]
            new public abstract class Msg
            {
                public const string Description = "Scan and Count";
            }

            protected override IEnumerable<ScanState<INScanCount>> CreateStates()
            {
                yield return new RefNbrState();
                yield return new LocationState();
                yield return new InventoryItemState()
                    .Intercept.HandleAbsence.ByAppend((basis, barcode) =>
                    {
                        if (basis.TryProcessBy<LocationState>(barcode,
                            StateSubstitutionRule.KeepPositiveReports |
                            StateSubstitutionRule.KeepApplication))
                            return AbsenceHandling.Done;
                        return AbsenceHandling.Skipped;
                    });
                yield return new LotSerialState();
                yield return new ConfirmState();
            }


            public sealed class RefNbrState : WMSBase.RefNbrState<INPIHeader>
            {
                protected override string StatePrompt => Msg.Prompt;
                protected override INPIHeader GetByBarcode(string barcode) =>
                    INPIHeader.PK.Find(Basis, barcode);
                protected override void ReportMissing(string barcode) =>
                    Basis.ReportError(Msg.Missing, barcode);

                protected override Validation Validate(INPIHeader entity) =>
                    Basis.IsDocumentStatusEditable(entity.Status)
                    ? Validation.Ok
                    : Validation.Fail(Msg.InvalidStatus,
                        Basis.SightOf<INPIHeader.status>(entity));

                protected override void Apply(INPIHeader entity)
                {
                    Basis.RefNbr = entity.PIID;
                    Basis.SiteID = entity.SiteID;
                    Basis.NoteID = entity.NoteID;
                    Basis.DocumentView.Current = entity;
                }
                protected override void ClearState()
                {
                    Basis.RefNbr = null;
                    Basis.SiteID = null;
                    Basis.NoteID = null;
                    Basis.DocumentView.Current = null;
                }

                protected override void ReportSuccess(INPIHeader entity) =>
                    Basis.ReportInfo(Msg.Ready, entity.PIID);

                [PXLocalizable]
                public abstract class Msg
                {
                    public const string Prompt = "Scan a reference number of the PI count.";
                    public const string Missing = "The {0} PI count was not found.";
                    public const string InvalidStatus =
                        "Document has the {0} status, cannot be used for count.";
                    public const string Ready =
                        "The {0} PI count has been loaded and is ready for processing.";
                    public const string NotSet = "Document number is not selected.";
                }
            }

            public new sealed class LocationState : WMSBase.LocationState
            {
                protected override bool IsStateSkippable() => Basis.LocationID != null;

                protected override Validation Validate(INLocation location)
                {
                    if (Basis.HasFault(location, base.Validate, out Validation fault))
                        return fault;

                    INPIStatusLoc statusLocation =
                        SelectFrom<INPIStatusLoc>.
                        Where<
                            INPIStatusLoc.pIID.IsEqual<WMSScanHeader.refNbr.FromCurrent>.
                            And<
                                INPIStatusLoc.locationID.IsEqual<@P.AsInt>.
                                Or<INPIStatusLoc.locationID.IsNull>>>.
                        View.ReadOnly
                        .Select(Basis, location.LocationID);

                    if (statusLocation == null)
                        return Validation.Fail(Msg.NotPresent, location.LocationCD);

                    return Validation.Ok;
                }

                [PXLocalizable]
                public new abstract class Msg : WMSBase.LocationState.Msg
                {
                    public const string NotPresent = "The {0} location is not in the list and cannot be added.";
                }
            }

            public sealed class ConfirmState : WMSBase.ConfirmationState
            {
                public override string Prompt =>
                    Basis.Localize(Msg.Prompt, Basis.SightOf<WMSScanHeader.inventoryID>(),
                        Basis.Qty, Basis.UOM);

                // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
                public class Logic : ScanExtension
                {
                    public virtual FlowStatus Confirm()
                    {
                        if (!CanConfirm(out var error))
                            return error;

                        return ConfirmRow();
                    }

                    protected virtual bool CanConfirm(out FlowStatus error)
                    {
                        if (Basis.Document == null)
                        {
                            error = FlowStatus.Fail(RefNbrState.Msg.NotSet);
                            return false;
                        }

                        if (Basis.DocumentIsEditable == false)
                        {
                            error = FlowStatus.Fail(RefNbrState.Msg.InvalidStatus, 
                                Basis.DocumentView.Cache.GetStateExt<INPIHeader.status>(Basis.Document));
                            return false;
                        }

                        if (Basis.InventoryID == null)
                        {
                            error = FlowStatus.Fail(InventoryItemState.Msg.NotSet);
                            return false;
                        }

                        if (Basis.CurrentMode.HasActive<LotSerialState>() && Basis.LotSerialNbr == null)
                        {
                            error = FlowStatus.Fail(LotSerialState.Msg.NotSet);
                            return false;
                        }

                        if (Basis.CurrentMode.HasActive<LotSerialState>() &&
                            Basis.SelectedLotSerialClass.LotSerTrack == INLotSerTrack.SerialNumbered &&
                            Basis.BaseQty != 1)
                        {
                            error = FlowStatus.Fail(InventoryItemState.Msg.SerialItemNotComplexQty);
                            return false;
                        }

                        error = FlowStatus.Ok;
                        return true;
                    }

                    protected virtual FlowStatus ConfirmRow()
                    {
                        INPIDetail row = FindDetailRow();

                        decimal? newQty = row?.PhysicalQty ?? 0;
                        if (Basis.Remove == true)
                            newQty -= Basis.BaseQty;
                        else
                            newQty += Basis.BaseQty;

                        if (Basis.CurrentMode.HasActive<LotSerialState>() &&
                            Basis.SelectedLotSerialClass.LotSerTrack == INLotSerTrack.SerialNumbered &&
                            newQty.IsNotIn(0, 1))
                        {
                            return FlowStatus.Fail(InventoryItemState.Msg.SerialItemNotComplexQty);
                        }

                        if (row == null)
                        {
                            row = (INPIDetail)Basis.Details.Cache.CreateInstance();
                            row.PIID = Basis.RefNbr;
                            row.LineNbr = (int)PXLineNbrAttribute.NewLineNbr<INPIDetail.lineNbr>(Basis.Details.Cache, Basis.Document);
                            row.InventoryID = Basis.InventoryID;
                            row.SubItemID = Basis.SubItemID;
                            row.SiteID = Basis.SiteID;
                            row.LocationID = Basis.LocationID;
                            row.LotSerialNbr = Basis.LotSerialNbr;
                            row.PhysicalQty = Basis.BaseQty;
                            row.BookQty = 0;
                            row = Basis.Details.Insert(row);

                            Basis.SaveChanges();

                            row = Basis.Details.Locate(row) ?? row;
                        }

                        Basis.Details.SetValueExt<INPIDetail.physicalQty>(row, newQty);
                        row = Basis.Details.Update(row);
                        Basis.SaveChanges();

                        Basis.DispatchNext(
                            null,
                            Basis.SightOf<WMSScanHeader.inventoryID>(), Basis.Qty, Basis.UOM);

                        return FlowStatus.Ok;
                    }

                    protected virtual INPIDetail FindDetailRow()
                    {
                        var findDetailCmd = BqlCommand.CreateInstance(Basis.Details.View.BqlSelect.GetType());

                        findDetailCmd = findDetailCmd.WhereAnd<Where<
                            INPIDetail.inventoryID.IsEqual<WMSScanHeader.inventoryID.FromCurrent>.
                            And<INPIDetail.siteID.IsEqual<WMSScanHeader.siteID.FromCurrent>>
                        >>();

                        if (Basis.CurrentMode.HasActive<LocationState>() && Basis.LocationID != null)
                            findDetailCmd = findDetailCmd.WhereAnd<Where<INPIDetail.locationID.IsEqual<
                                WMSScanHeader.locationID.FromCurrent>>>();

                        if (Basis.CurrentMode.HasActive<LotSerialState>() && Basis.LotSerialNbr != null)
                            findDetailCmd = findDetailCmd.WhereAnd<Where<INPIDetail.lotSerialNbr.IsEqual<
                                WMSScanHeader.lotSerialNbr.FromCurrent>>>();

                        var findDetailView = Basis.Graph.TypedViews.GetView(findDetailCmd, false);
                        var findResultSet = (PXResult<INPIDetail>)findDetailView.SelectSingle();

                        return findResultSet;
                    }
                }

                protected override FlowStatus PerformConfirmation() =>
                    Get<Logic>().Confirm();


                [PXLocalizable]
                new public abstract class Msg
                {
                    public const string Prompt = "Confirm counting {0} x {1} {2}.";
                }
            }

            protected override IEnumerable<ScanTransition<INScanCount>> CreateTransitions()
            {
                yield return Transition(t => t.From<RefNbrState>().To<LocationState>());
                yield return Transition(t => t.From<LocationState>().To<InventoryItemState>());
                yield return Transition(t => t.From<InventoryItemState>().To<LotSerialState>());
            }

            protected override IEnumerable<ScanCommand<INScanCount>> CreateCommands()
            {
                yield return new RemoveCommand();
                yield return new QtySupport.SetQtyCommand();
                yield return new ConfirmCommand();
            }

            public sealed class ConfirmCommand : ScanCommand
            {
                public override string Code => "CONFIRM";
                public override string ButtonName => "scanConfirmDocument";
                public override string DisplayName => Msg.DisplayName;
                protected override bool IsEnabled => Basis.DocumentIsEditable;

                protected override bool Process()
                {
                    if (Basis.Document == null)
                        Basis.ReportError(RefNbrState.Msg.NotSet);
                    if (Basis.DocumentIsEditable == false)
                        Basis.ReportError(RefNbrState.Msg.InvalidStatus,
                            Basis.DocumentView.Cache.GetStateExt<INPIHeader.status>(
                            Basis.Document));

                    BqlCommand nullPhysicalQtyCmd = BqlCommand.CreateInstance(
                        Basis.Details.View.BqlSelect.GetType());
                    nullPhysicalQtyCmd = nullPhysicalQtyCmd.WhereAnd<Where<
                        INPIDetail.physicalQty.IsNull>>();
                    PXView nullPhysicalQtyView = Basis.Graph.TypedViews.GetView(
                        nullPhysicalQtyCmd, false);

                    foreach (INPIDetail detail in nullPhysicalQtyView.SelectMulti().
                        RowCast<INPIDetail>())
                    {
                        Basis.Details.SetValueExt<INPIDetail.physicalQty>(detail, 0m);
                        Basis.Details.Update(detail);
                    }

                    Basis.SaveChanges();

                    Basis.DocumentView.Current = null;
                    Basis.CurrentMode.Reset(fullReset: true);
                    Basis.ReportInfo(Msg.CountConfirmed);

                    return true;
                }

                [PXLocalizable]
                public abstract class Msg
                {
                    public const string DisplayName = "Confirm";
                    public const string CountConfirmed = "The count has been saved.";
                }
            }

            protected override IEnumerable<ScanRedirect<INScanCount>> CreateRedirects()
            {
                return AllWMSRedirects.CreateFor<INScanCount>();
            }

            public new sealed class RedirectFrom<TForeignBasis> :
        WMSBase.RedirectFrom<TForeignBasis>
        where TForeignBasis : PXGraphExtension, IBarcodeDrivenStateMachine
            {
                public override string Code => "COUNT";
                public override string DisplayName => Msg.DisplayName;
                public override bool IsPossible =>
                    PXAccess.FeatureInstalled<PX.Objects.CS.FeaturesSet.wMSInventory>();
                [PXLocalizable]
                public abstract class Msg
                {
                    public const string DisplayName = "PI Count";
                }
            }

            protected override void ResetMode(bool fullReset)
            {
                Clear<RefNbrState>(when: fullReset && !Basis.IsWithinReset);
                Clear<LocationState>(when: fullReset);
                Clear<InventoryItemState>();
                Clear<LotSerialState>();
            }
        }

        protected override ScanState<INScanCount> DecorateScanState(
        ScanState<INScanCount> original)
        {
            var state = base.DecorateScanState(original);

            if (state is WMSBase.LotSerialState lotSerialState)
                PatchLotSerialState(lotSerialState);

            return state;
        }

        protected virtual void PatchLotSerialState(
            WMSBase.LotSerialState lotSerialState)
        {
            lotSerialState
                .Intercept.IsStateActive.ByConjoin(
                    basis => basis.IsEnterableLotSerial(isForIssue: false));
        }
    }
}
