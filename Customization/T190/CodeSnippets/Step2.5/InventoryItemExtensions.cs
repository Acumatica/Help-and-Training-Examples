using PX.Data.BQL.Fluent;
using PX.Data.BQL;
using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data;
using PX.Objects.Common.Extensions;
using PX.Objects.Common;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.DR;
using PX.Objects.EP;
using PX.Objects.GL;
using PX.Objects.IN.Matrix.Attributes;
using PX.Objects.IN.Matrix.Graphs;
using PX.Objects.IN;
using PX.Objects.TX;
using PX.Objects;
using PX.TM;
using SelectParentItemClass = PX.Data.BQL.Fluent.SelectFrom<PX.Objects.IN.INItemClass>.Where<PX.Objects.IN.INItemClass.itemClassID.IsEqual<PX.Objects.IN.InventoryItem.itemClassID.FromCurrent>>;
using SelectParentPostClass = PX.Data.BQL.Fluent.SelectFrom<PX.Objects.IN.INPostClass>.Where<PX.Objects.IN.INPostClass.postClassID.IsEqual<PX.Objects.IN.InventoryItem.postClassID.FromCurrent>>;
using System.Collections.Generic;
using System;

namespace PX.Objects.IN
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public sealed class InventoryItemExt : PXCacheExtension<PX.Objects.IN.InventoryItem>
    {
        #region UsrRepairItem
        [PXDBBool]
        [PXUIField(DisplayName = "Repair Item")]
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
        //////////The modified code
        [PXUIField(DisplayName = "Repair Item Type", Enabled = false)]
        //////////The end of modified code
        public string? UsrRepairItemType { get; set; }
        public abstract class usrRepairItemType :
          PX.Data.BQL.BqlString.Field<usrRepairItemType>
        { }
        #endregion
    }
}