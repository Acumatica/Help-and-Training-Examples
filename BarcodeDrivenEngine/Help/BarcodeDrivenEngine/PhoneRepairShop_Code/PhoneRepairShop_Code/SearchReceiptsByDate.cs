using System;
using PX.Data;
using PX.BarcodeProcessing;
using PX.Objects.PO.WMS;
using PX.Objects.PO;
using PX.Data.BQL.Fluent;
using PX.Objects.AP;
using PX.Data.BQL;

namespace PhoneRepairShopWMS
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class SearchReceiptsByDate : ReceivePutAway.ScanExtension
    {
        [PXOverride]
        public virtual ScanState<ReceivePutAway> DecorateScanState(
            ScanState<ReceivePutAway> original,
            Func<ScanState<ReceivePutAway>, 
                ScanState<ReceivePutAway>> base_DecorateScanState)
        {
            var state = base_DecorateScanState(original);

            if (state is ReceivePutAway.ReceiveMode.ReceiptState receiptState)
                PatchReceiptStateInReceiveMode(receiptState); 

            return state;
        }

        protected virtual void PatchReceiptStateInReceiveMode(
            ReceivePutAway.ReceiveMode.ReceiptState receiptState)
        {
            receiptState
                .Intercept.HandleAbsence.ByAppend((basis, barcode) =>
                {
                    if (DateTime.TryParse(barcode.Trim(), out var date))
                    {
                        POReceipt receiptByDate =
                            SelectFrom<POReceipt>.
                            LeftJoin<Vendor>.On<POReceipt.vendorID.
                                IsEqual<Vendor.bAccountID>>.SingleTableOnly.
                            Where<
                                POReceipt.receiptDate.IsEqual<@P.AsDateTime>.
                                And<POReceipt.receiptType.IsEqual<POReceiptType.poreceipt>>.
                                And<
                                    Vendor.bAccountID.IsNull.
                                    Or<Match<Vendor, AccessInfo.userName.FromCurrent>>>>.
                            View.ReadOnly.Select(basis, date);

                        if (receiptByDate != null)
                            return AbsenceHandling.ReplaceWith(receiptByDate);
                    }

                    return AbsenceHandling.Skipped;
            });
        }
    }
}
