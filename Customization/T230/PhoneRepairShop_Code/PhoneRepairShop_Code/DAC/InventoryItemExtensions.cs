using PX.Data;

namespace PX.Objects.IN
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public sealed class InventoryItemExt : PXCacheExtension<PX.Objects.IN.InventoryItem>
    {
        #region UsrRepairItem
        [PXDBBool]
        [PXUIField(DisplayName="Repair Item")]
        [PXDefault(false, PersistingCheck = PXPersistingCheck.Nothing)]
        public bool? UsrRepairItem { get; set; }
        public abstract class usrRepairItem : PX.Data.BQL.BqlBool.Field<usrRepairItem> { }
        #endregion

        #region UsrRepairItemType
        [PXDBString(2, IsFixed = true)]
        [PXStringList(
            new string[]
            {
                PhoneRepairShop.RepairItemTypeConstants.Battery,
                PhoneRepairShop.RepairItemTypeConstants.Screen,
                PhoneRepairShop.RepairItemTypeConstants.ScreenCover,
                PhoneRepairShop.RepairItemTypeConstants.BackCover,
                PhoneRepairShop.RepairItemTypeConstants.Motherboard
            },
            new string[]
            {
                PhoneRepairShop.Messages.Battery,
                PhoneRepairShop.Messages.Screen,
                PhoneRepairShop.Messages.ScreenCover,
                PhoneRepairShop.Messages.BackCover,
                PhoneRepairShop.Messages.Motherboard
            })]
        [PXUIField(DisplayName = "Repair Item Type", Enabled = false)]
        public string UsrRepairItemType { get; set; }
        public abstract class usrRepairItemType :
          PX.Data.BQL.BqlString.Field<usrRepairItemType>
        { }
        #endregion
    }
}