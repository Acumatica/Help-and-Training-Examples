using System;
using PX.Data;
using PX.BarcodeProcessing;
using PX.Objects.SO.WMS;
using PX.Objects.IN.WMS;

namespace PhoneRepairShop
{
    using WMSBase = WarehouseManagementSystem<PickPackShip, PickPackShip.Host>;

    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class LocationGoesLastInPickMode : PickPackShip.ScanExtension
    {
        [PXOverride]
        public virtual ScanMode<PickPackShip> DecorateScanMode(
            ScanMode<PickPackShip> original,
            Func<ScanMode<PickPackShip>, ScanMode<PickPackShip>> base_DecorateScanMode)
        {
            var mode = base_DecorateScanMode(original);

            if (mode is PickPackShip.PickMode pickMode)
                PatchPickMode(pickMode);

            return mode;
        }

        protected virtual void PatchPickMode(PickPackShip.PickMode pickMode)
        {
            pickMode
                .Intercept.CreateTransitions.ByReplace(basis =>
                {
                    return basis.StateFlow(flow => flow
                        .From<PickPackShip.PickMode.ShipmentState>()
                        .NextTo<WMSBase.InventoryItemState>()
                        .NextTo<WMSBase.LotSerialState>()
                        .NextTo<WMSBase.ExpireDateState>()
                        .NextTo<WMSBase.LocationState>());
                });
        }
    }
}
