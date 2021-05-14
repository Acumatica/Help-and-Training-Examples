using PX.Api;
using PX.Api.Models;
using PX.Common;
using PX.Data;
using PX.Data.BQL;
using PX.Data.BQL.Fluent;
using PX.Data.WorkflowAPI;
using PX.Objects;
using PX.Objects.AP;
using PX.Objects.AR;
using PX.Objects.Common.Discount;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.DR;
using PX.Objects.GL;
using PX.Objects.IN;
using PX.Objects.PO;
using PX.Objects.RUTROT;
using PX.Objects.SO;
using PX.SM;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CRLocation = PX.Objects.CR.Standalone.Location;
using ItemStats = PX.Objects.IN.Overrides.INDocumentRelease.ItemStats;
using SiteStatus = PX.Objects.IN.Overrides.INDocumentRelease.SiteStatus;
using PhoneRepairShop;

namespace PX.Objects.IN
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class InventoryItemMaint_Extension : PXGraphExtension<InventoryItemMaint>
    {
        #region Data Views
        public SelectFrom<RSSVStockItemDevice>.
            Where<RSSVStockItemDevice.inventoryID.
                IsEqual<InventoryItem.inventoryID.FromCurrent>>.View
            CompatibleDevices;
        #endregion

        #region Event Handlers
        ...
        #endregion
    }
}