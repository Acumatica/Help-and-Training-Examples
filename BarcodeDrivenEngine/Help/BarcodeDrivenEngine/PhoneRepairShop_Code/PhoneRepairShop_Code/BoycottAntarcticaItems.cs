using System;
using PX.Data;
using PX.BarcodeProcessing;
using PX.Common;
using PX.Objects.PO.WMS;

namespace PhoneRepairShopWMS
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class BoycottAntarcticaItems : ReceivePutAway.ScanExtension
    {
        [PXOverride]
        public virtual ScanState<ReceivePutAway> DecorateScanState(
            ScanState<ReceivePutAway> original,
            Func<ScanState<ReceivePutAway>, 
                ScanState<ReceivePutAway>> base_DecorateScanState)
        {
            var state = base_DecorateScanState(original);

            if (state is ReceivePutAway.InventoryItemState itemState 
                && itemState.ModeCode == ReceivePutAway.ReceiveMode.Value)
                PatchInventoryItemStateInReceiveMode(itemState); 

            return state;
        }

        protected virtual void PatchInventoryItemStateInReceiveMode(
            ReceivePutAway.InventoryItemState itemState)
        {
            itemState
                .Intercept.Validate.ByAppend((basis, item) =>
                {
                    (var xref, var inventory) = item;
                    if (inventory.CountryOfOrigin == "AQ")
                        return Validation.Fail(Msg.CannotReceiveItem, 
                            inventory.InventoryCD);

                    return Validation.Ok;
                });
        }

        [PXLocalizable]
        public abstract class Msg
        {
            public const string CannotReceiveItem = 
                "The {0} item cannot be received.";
        }
    }
}
