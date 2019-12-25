using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data;
using PX.Objects.Common.Extensions;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.DR;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.TX;
using PX.Objects;
using PX.TM;
using System.Collections.Generic;
using System;

namespace PX.Objects.IN
{
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
            PX.Data.BQL.BqlString.Field<usrRepairItemType> { }
        #endregion
    }
}