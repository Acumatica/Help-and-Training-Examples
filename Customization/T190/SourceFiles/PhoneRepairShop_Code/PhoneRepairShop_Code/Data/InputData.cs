using System;
using PX.Data;
using Customization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using PX.Objects.IN;
using PX.Data.BQL.Fluent;
using PX.Data.BQL;

namespace PhoneRepairShop
{
    //The customization plug-in fills the database with the data entered in the T210 and T220 courses 
    public class InputData : CustomizationPlugin
    {
        //This method executed after customization was published and website was restarted.  
        public override void UpdateDatabase()
        {
            #region T200Data

            RSSVRepairServiceMaint repairServiceGraph = PXGraph.CreateInstance<RSSVRepairServiceMaint>();

            #region RSSVRepairService

            RSSVRepairService repairService = SelectFrom<RSSVRepairService>.View.ReadOnly.Select(repairServiceGraph);
            if (repairService == null)
            {
                using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "InputData\\RSSVRepairService.csv"))
                {
                    string header = file.ReadLine();
                    if (header != null)
                    {
                        string[] headerParts = header.Split(';');
                        while (true)
                        {
                            string line = file.ReadLine();
                            if (line != null)
                            {
                                string[] lineParts = line.Split(';');
                                IDictionary<string, string> dic = headerParts.Select((k, i) => new { k, v = lineParts[i] }).ToDictionary(x => x.k, x => x.v);
                                RSSVRepairService service = new RSSVRepairService
                                {
                                    ServiceCD = dic["ServiceCD"],
                                    Description = dic["Description"],
                                    Active = Convert.ToBoolean(Convert.ToInt32(dic["Active"])),
                                    WalkInService = Convert.ToBoolean(Convert.ToInt32(dic["WalkInService"])),
                                    Prepayment = Convert.ToBoolean(Convert.ToInt32(dic["Prepayment"])),
                                    PreliminaryCheck = Convert.ToBoolean(Convert.ToInt32(dic["PreliminaryCheck"]))
                                };
                                repairServiceGraph.RepairService.Insert(service);
                                repairServiceGraph.Actions.PressSave();
                                repairServiceGraph.Clear();
                            }
                            else break;
                        }
                    }
                    this.WriteLog("RSSVRepairService updated");
                }
            }
            #endregion

            RSSVDeviceMaint deviceGraph = PXGraph.CreateInstance<RSSVDeviceMaint>();

            #region RSSVDevice

            RSSVDevice servDevice = SelectFrom<RSSVDevice>.View.ReadOnly.Select(deviceGraph);
            if (servDevice == null)
            {
                using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "InputData\\RSSVDevice.csv"))
                {
                    string header = file.ReadLine();
                    if (header != null)
                    {
                        string[] headerParts = header.Split(';');
                        while (true)
                        {
                            string line = file.ReadLine();
                            if (line != null)
                            {
                                string[] lineParts = line.Split(';');
                                IDictionary<string, string> dic = headerParts.Select((k, i) => new { k, v = lineParts[i] }).ToDictionary(x => x.k, x => x.v);
                                RSSVDevice device = new RSSVDevice
                                {
                                    DeviceCD = dic["DeviceCD"],
                                    Description = dic["Description"],
                                    Active = Convert.ToBoolean(Convert.ToInt32(dic["Active"])),
                                    AvgComplexityOfRepair = dic["AvgComplexityOfRepair"]
                                };
                                deviceGraph.ServDevices.Insert(device);
                                deviceGraph.Actions.PressSave();
                                deviceGraph.Clear();
                            }
                            else break;
                        }
                    }
                    this.WriteLog("RSSVDevice updated");
                }
            }
            #endregion

            #endregion

            #region T210Data
            var iiEntry = PXGraph.CreateInstance<InventoryItemMaint>();
            #region InventoryItem
            //Add data to InventoryItem

            //InventoryItem invItem = SelectFrom<InventoryItem>.Where<InventoryItemExt.usrRepairItem.IsEqual<True>>.View.ReadOnly.Select(iiEntry);            
            //if (invItem == null)
            //{
                using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "InputData\\InventoryItem.csv"))
                {
                    string header = file.ReadLine();
                    if (header != null)
                    {
                        string[] headerParts = header.Split(';');
                        while (true)
                        {
                            string line = file.ReadLine();
                        if (line != null)
                        {
                            string[] lineParts = line.Split(';');
                            IDictionary<string, string> dic = headerParts.Select((k, i) => new { k, v = lineParts[i] }).ToDictionary(x => x.k, x => x.v);
                            InventoryItem invItem = SelectFrom<InventoryItem>.Where<InventoryItem.inventoryID.IsEqual<@P.AsInt>>.View.ReadOnly.Select(iiEntry, Convert.ToInt32(dic["InventoryID"]));
                            if (invItem == null)
                            {
                                var iItem = new InventoryItem
                                {
                                    InventoryCD = dic["InventoryCD"],
                                    ItemClassID = Convert.ToInt32(dic["ItemClassID"])
                                };
                                iItem = PXCache<InventoryItem>.CreateCopy(iiEntry.Item.Insert(iItem));
                                iItem.Descr = dic["Descr"];
                                iItem.BasePrice = Convert.ToDecimal(dic["BasePrice"]);
                                iItem = PXCache<InventoryItem>.CreateCopy(iiEntry.Item.Update(iItem));
                                iItem.DfltSiteID = Convert.ToInt32(dic["DfltSiteID"]);
                                iItem = PXCache<InventoryItem>.CreateCopy(iiEntry.Item.Update(iItem));
                                //var extItem = PXCache<InventoryItem>.GetExtension<InventoryItemExt>(iItem);
                                //extItem.UsrRepairItem = true;
                                //extItem.UsrRepairItemType = dic["UsrRepairItemType"];
                                //iItem = PXCache<InventoryItem>.CreateCopy(iiEntry.Item.Update(iItem));
                                iiEntry.Actions.PressSave();
                                iiEntry.Clear();
                            }
                        }
                        else break;
                        }
                    }
                    this.WriteLog("InventoryItem updated");
                }
            //}
            #endregion

            var repairPriceGraph = PXGraph.CreateInstance<RSSVRepairPriceMaint>();

            #region RSSVRepairPrice
            //Add data to RSSVRepairPrice
            RSSVRepairPrice repairPrice = SelectFrom<RSSVRepairPrice>.View.ReadOnly.Select(repairPriceGraph);
            if (repairPrice == null)
            {
                using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "InputData\\RSSVRepairPrice.csv"))
                {
                    string header = file.ReadLine();
                    if (header != null)
                    {
                        string[] headerParts = header.Split(';');
                        while (true)
                        {
                            string line = file.ReadLine();
                            if (line != null)
                            {
                                string[] lineParts = line.Split(';');
                                IDictionary<string, string> dic = headerParts.Select((k, i) => new { k, v = lineParts[i] }).ToDictionary(x => x.k, x => x.v);
                                RSSVRepairPrice price = new RSSVRepairPrice
                                {
                                    DeviceID = Convert.ToInt32(dic["DeviceID"]),
                                    ServiceID = Convert.ToInt32(dic["ServiceID"]),
                                    //Price = Convert.ToDecimal(dic["Price"]),
                                    RepairItemLineCntr = Convert.ToInt32(dic["RepairItemLineCntr"])
                                };
                                repairPriceGraph.RepairPrices.Insert(price);
                                repairPriceGraph.Actions.PressSave();
                                repairPriceGraph.Clear();
                            }
                            else break;
                        }
                    }
                    this.WriteLog("RSSVRepairPrice updated");
                }
            }
            #endregion

            #region RSSVRepairItem
            //Add data to RSSVRepairItem
            RSSVRepairItem repairItem = SelectFrom<RSSVRepairItem>.View.ReadOnly.Select(repairPriceGraph);
            if (repairItem == null)
            {
                using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "InputData\\RSSVRepairItem.csv"))
                {
                    string header = file.ReadLine();
                    if (header != null)
                    {
                        string[] headerParts = header.Split(';');
                        while (true)
                        {
                            string line = file.ReadLine();
                            if (line != null)
                            {
                                string[] lineParts = line.Split(';');
                                IDictionary<string, string> dic = headerParts.Select((k, i) => new { k, v = lineParts[i] }).ToDictionary(x => x.k, x => x.v);
                                RSSVRepairItem item = new RSSVRepairItem
                                {
                                    ServiceID = Convert.ToInt32(dic["ServiceID"]),
                                    DeviceID = Convert.ToInt32(dic["DeviceID"]),
                                    LineNbr = Convert.ToInt32(dic["LineNbr"]),
                                    RepairItemType = Convert.ToString(dic["RepairItemType"]),
                                    InventoryID = Convert.ToInt32(dic["InventoryID"]),
                                    Required = Convert.ToBoolean(Convert.ToInt32(dic["Required"])),
                                    IsDefault = Convert.ToBoolean(Convert.ToInt32(dic["IsDefault"])),
                                    BasePrice = Convert.ToDecimal(dic["BasePrice"])
                                };
                                repairPriceGraph.RepairItems.Insert(item);
                                repairPriceGraph.Actions.PressSave();
                                repairPriceGraph.Clear();
                            }
                            else break;
                        }
                    }
                    this.WriteLog("RSSVRepairItem updated");
                }
            }
            #endregion

            #region RSSVLabor
            //Add data to RSSVLabor
            RSSVLabor labor = SelectFrom<RSSVLabor>.View.ReadOnly.Select(repairPriceGraph);
            if (labor == null)
            {
                using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "InputData\\RSSVLabor.csv"))
                {
                    string header = file.ReadLine();
                    if (header != null)
                    {
                        string[] headerParts = header.Split(';');
                        while (true)
                        {
                            string line = file.ReadLine();
                            if (line != null)
                            {
                                string[] lineParts = line.Split(';');
                                IDictionary<string, string> dic = headerParts.Select((k, i) => new { k, v = lineParts[i] }).ToDictionary(x => x.k, x => x.v);
                                RSSVLabor priceLabor = new RSSVLabor
                                {
                                    InventoryID = Convert.ToInt32(dic["InventoryID"]),
                                    DeviceID = Convert.ToInt32(dic["DeviceID"]),
                                    ServiceID = Convert.ToInt32(dic["ServiceID"]),
                                    DefaultPrice = Convert.ToDecimal(dic["DefaultPrice"]),
                                    Quantity = Convert.ToDecimal(dic["Quantity"]),
                                    ExtPrice = Convert.ToDecimal(dic["ExtPrice"])
                                };
                                repairPriceGraph.Labor.Insert(priceLabor);
                                repairPriceGraph.Actions.PressSave();
                                repairPriceGraph.Clear();
                            }
                            else break;
                        }
                    }
                    this.WriteLog("RSSVLabor updated");
                }
            }
            #endregion

            #region RSSVWarranty
            //Add data to RSSVWarranty
            RSSVWarranty warranty = SelectFrom<RSSVWarranty>.View.ReadOnly.Select(repairPriceGraph);
            if (warranty == null)
            {
                using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "InputData\\RSSVWarranty.csv"))
                {
                    string header = file.ReadLine();
                    if (header != null)
                    {
                        string[] headerParts = header.Split(';');
                        while (true)
                        {
                            string line = file.ReadLine();
                            if (line != null)
                            {
                                string[] lineParts = line.Split(';');
                                IDictionary<string, string> dic = headerParts.Select((k, i) => new { k, v = lineParts[i] }).ToDictionary(x => x.k, x => x.v);
                                RSSVWarranty priceWarranty = new RSSVWarranty
                                {
                                    ContractID = Convert.ToInt32(dic["ContractID"]),
                                    DeviceID = Convert.ToInt32(dic["DeviceID"]),
                                    ServiceID = Convert.ToInt32(dic["ServiceID"]),
                                    DefaultWarranty = Convert.ToBoolean(Convert.ToInt32(dic["DefaultWarranty"]))
                                };
                                repairPriceGraph.Warranty.Insert(priceWarranty);
                                repairPriceGraph.Actions.PressSave();
                                repairPriceGraph.Clear();
                            }
                            else break;
                        }
                    }
                    this.WriteLog("RSSVWarranty updated");
                }
            }
            #endregion

            #endregion

            #region T220Data & T240Data

            var setupGraph = PXGraph.CreateInstance<RSSVSetupMaint>();

            #region RSSVSetup
            //Add data to RSSVSetup
            RSSVSetup setup = SelectFrom<RSSVSetup>.View.ReadOnly.Select(setupGraph);
            if (setup == null)
            {
                using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "InputData\\RSSVSetup.csv"))
                {
                    string header = file.ReadLine();
                    if (header != null)
                    {
                        string[] headerParts = header.Split(';');
                        while (true)
                        {
                            string line = file.ReadLine();
                            if (line != null)
                            {
                                string[] lineParts = line.Split(';');
                                IDictionary<string, string> dic = headerParts.Select((k, i) => new { k, v = lineParts[i] }).ToDictionary(x => x.k, x => x.v);

                                RSSVSetup settings = new RSSVSetup
                                {
                                    NumberingID = dic["NumberingID"],
                                    WalkInCustomerID = Convert.ToInt32(dic["WalkInCustomerID"]),
                                    DefaultEmployee = Guid.Parse(dic["DefaultEmployee"]),
                                    PrepaymentPercent = Convert.ToDecimal(dic["PrepaymentPercent"])
                                };
                                setupGraph.Setup.Insert(settings);
                                setupGraph.Actions.PressSave();
                                setupGraph.Clear();
                            }
                            else break;
                        }
                    }
                    this.WriteLog("RSSVSetup updated");
                }
            }
            #endregion

            var workOrderGraph = PXGraph.CreateInstance<RSSVWorkOrderEntry>();

            #region RSSVWorkOrder
            //Add data to RSSVWorkOrder
            RSSVWorkOrder workOrder = SelectFrom<RSSVWorkOrder>.View.ReadOnly.Select(workOrderGraph);
			if (workOrder == null)
			{
				using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "InputData\\RSSVWorkOrder.csv"))
				{
					string header = file.ReadLine();
					if (header != null)
					{
						string[] headerParts = header.Split(';');
						while (true)
						{
							string line = file.ReadLine();
							if (line != null)
							{
								string[] lineParts = line.Split(';');
								IDictionary<string, string> dic = headerParts.Select((k, i) => new { k, v = lineParts[i] }).ToDictionary(x => x.k, x => x.v);
                                RSSVWorkOrder order = new RSSVWorkOrder
                                {
                                    OrderNbr = dic["OrderNbr"],
                                    CustomerID = Convert.ToInt32(dic["CustomerID"]),
                                    DateCreated = Convert.ToDateTime(dic["DateCreated"]),
                                    Status = dic["Status"],
                                    Hold = Convert.ToBoolean(Convert.ToInt32(dic["Hold"])),
                                    Description = dic["Description"],
                                    DeviceID = Convert.ToInt32(dic["DeviceID"]),
                                    ServiceID = Convert.ToInt32(dic["ServiceID"]),
                                    //OrderTotal = Convert.ToDecimal(dic["OrderTotal"]),
                                    RepairItemLineCntr = Convert.ToInt32(dic["RepairItemLineCntr"]),
                                    Priority = dic["Priority"]
                                };
                                if (dic["DateCompleted"] != "NULL") order.DateCompleted = Convert.ToDateTime(dic["DateCompleted"]);
                                //if (dic["InvoiceNbr"] != "NULL") order.InvoiceNbr = dic["InvoiceNbr"];
                                if (dic["Assignee"] != "NULL") order.Assignee = Guid.Parse(dic["Assignee"]);
                                workOrderGraph.WorkOrders.Insert(order);
                                workOrderGraph.Actions.PressSave();
                                workOrderGraph.Clear();
        }
							else break;
    }
					}
					this.WriteLog("RSSVWorkOrder updated");
				}
			}
			#endregion

			#region RSSVWorkOrderItem
			//Add data to RSSVWorkOrderItem
			RSSVWorkOrderItem workOrderItem = SelectFrom<RSSVWorkOrderItem>.View.ReadOnly.Select(workOrderGraph);
			if (workOrderItem == null)
			{
				using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "InputData\\RSSVWorkOrderItem.csv"))
				{
					string header = file.ReadLine();
					if (header != null)
					{
						string[] headerParts = header.Split(';');
						while (true)
						{
							string line = file.ReadLine();
							if (line != null)
							{
								string[] lineParts = line.Split(';');
								IDictionary<string, string> dic = headerParts.Select((k, i) => new { k, v = lineParts[i] }).ToDictionary(x => x.k, x => x.v);                                //workOrderGraph.WorkOrders.Current = workOrderGraph.WorkOrders.Search<RSSVWorkOrder.orderNbr>(dic["OrderNbr"]);
                                RSSVWorkOrderItem orderItem = new RSSVWorkOrderItem
                                {
                                    OrderNbr = dic["OrderNbr"],
                                    LineNbr = Convert.ToInt32(dic["LineNbr"]),
                                    RepairItemType = dic["RepairItemType"],
                                    InventoryID = Convert.ToInt32(dic["InventoryID"]),
                                    BasePrice = Convert.ToDecimal(dic["BasePrice"])
                                };
                                workOrderGraph.RepairItems.Insert(orderItem);                              
                                workOrderGraph.Actions.PressSave();
                                workOrderGraph.Clear();
							}
							else break;
						}
					}
					this.WriteLog("RSSVWorkOrderItem updated");
				}
			}
			#endregion

			#region RSSVWorkOrderLabor
			//Add data to RSSVWorkOrderLabor
			RSSVWorkOrderLabor workOrderLabor = SelectFrom<RSSVWorkOrderLabor>.View.ReadOnly.Select(workOrderGraph);
			if (workOrderLabor == null)
			{
				using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "InputData\\RSSVWorkOrderLabor.csv"))
				{
					string header = file.ReadLine();
					if (header != null)
					{
						string[] headerParts = header.Split(';');
						while (true)
						{
							string line = file.ReadLine();
							if (line != null)
							{
								string[] lineParts = line.Split(';');
								IDictionary<string, string> dic = headerParts.Select((k, i) => new { k, v = lineParts[i] }).ToDictionary(x => x.k, x => x.v);
                                RSSVWorkOrderLabor laborItem = new RSSVWorkOrderLabor
                                {
                                    OrderNbr = dic["OrderNbr"],                                    
                                    InventoryID = Convert.ToInt32(dic["InventoryID"]),
                                    DefaultPrice = Convert.ToDecimal(dic["DefaultPrice"]),
                                    Quantity = Convert.ToDecimal(dic["Quantity"]),
                                    ExtPrice = Convert.ToDecimal(dic["ExtPrice"])
                                };
                                workOrderGraph.Labor.Insert(laborItem);                                
                                workOrderGraph.Actions.PressSave();
                                workOrderGraph.Clear();
							}
							else break;
						}
					}
					this.WriteLog("RSSVWorkOrderLabor updated");
				}
			}
            #endregion

            #region RSSVEmployeeWorkOrderQty
            //Add data to RSSVEmployeeWorkOrderQty
            RSSVEmployeeWorkOrderQty qty = SelectFrom<RSSVEmployeeWorkOrderQty>.View.ReadOnly.Select(workOrderGraph);
            if (qty == null)
            {
                using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "InputData\\RSSVEmployeeWorkOrderQty.csv"))
                {
                    string header = file.ReadLine();
                    if (header != null)
                    {
                        string[] headerParts = header.Split(';');
                        while (true)
                        {
                            string line = file.ReadLine();
                            if (line != null)
                            {
                                string[] lineParts = line.Split(';');
                                IDictionary<string, string> dic = headerParts.Select((k, i) => new { k, v = lineParts[i] }).ToDictionary(x => x.k, x => x.v);
                                //we skip graph logic for RSSVEmployeeWorkOrderQty because there is a PXAccumulator attribute for its field
                                PXDatabase.Insert<RSSVEmployeeWorkOrderQty>(
                                    new PXDataFieldAssign<RSSVEmployeeWorkOrderQty.userid>(Guid.Parse(dic["UserID"])),
                                    new PXDataFieldAssign<RSSVEmployeeWorkOrderQty.nbrOfAssignedOrders>(Convert.ToInt32(dic["NbrOfAssignedOrders"])),
                                    new PXDataFieldAssign<RSSVWorkOrder.createdByID>(workOrderGraph.Accessinfo.UserID),
                                    new PXDataFieldAssign<RSSVWorkOrder.createdByScreenID>(dic["CreatedByScreenID"]),
                                    new PXDataFieldAssign<RSSVWorkOrder.createdDateTime>(workOrderGraph.Accessinfo.BusinessDate),
                                    new PXDataFieldAssign<RSSVWorkOrder.lastModifiedByID>(workOrderGraph.Accessinfo.UserID),
                                    new PXDataFieldAssign<RSSVWorkOrder.lastModifiedByScreenID>(dic["LastModifiedByScreenID"]),
                                    new PXDataFieldAssign<RSSVWorkOrder.lastModifiedDateTime>(workOrderGraph.Accessinfo.BusinessDate),
                                    new PXDataFieldAssign<RSSVWorkOrder.noteid>(Guid.NewGuid().ToString())
                                    );
                            }
                            else break;
                        }
                    }
                    this.WriteLog("RSSVWorkOrderLabor updated");
                }
            }
            #endregion

            #endregion
        }
    }
}