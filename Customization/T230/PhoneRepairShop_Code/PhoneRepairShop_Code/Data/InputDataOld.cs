using System;
using PX.Data;
using Customization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using PX.Objects.IN;
using PX.Data.BQL.Fluent;

namespace PhoneRepairShop
{
	//The customization plug-in fills the database with the data entered in the T210 and T220 courses 
	public class InputData : CustomizationPlugin
	{
		//This method executed after customization was published and website was restarted.  
		public override void UpdateDatabase()
		{
			RSSVRepairPriceMaint graph = PXGraph.CreateInstance<RSSVRepairPriceMaint>();
			Guid UserID = graph.Accessinfo.UserID;

			#region T210Data

			#region RSSVRepairPrice
			//Add data to RSSVRepairPrice
			RSSVRepairPrice repairPrice = SelectFrom<RSSVRepairPrice>.View.ReadOnly.Select(graph);
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
								PXDatabase.Insert<RSSVRepairPrice>(

									//new PXDataFieldAssign(typeof(RSSVRepairPrice.priceID).Name, PXDbType.Int, Convert.ToInt32(dic["PriceID"])),
									new PXDataFieldAssign<RSSVRepairPrice.deviceID>(Convert.ToInt32(dic["DeviceID"])),
									new PXDataFieldAssign<RSSVRepairPrice.serviceID>(Convert.ToInt32(dic["ServiceID"])),
									new PXDataFieldAssign<RSSVRepairPrice.minimalPrice>(Convert.ToDecimal(dic["MinimalPrice"])),
									new PXDataFieldAssign<RSSVRepairPrice.repairItemLineCntr>(Convert.ToInt32(dic["RepairItemLineCntr"])),
									new PXDataFieldAssign<RSSVRepairPrice.createdByID>(UserID),
									new PXDataFieldAssign<RSSVRepairPrice.createdByScreenID>(dic["CreatedByScreenID"]),
									new PXDataFieldAssign<RSSVRepairPrice.createdDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVRepairPrice.lastModifiedByID>(UserID),
									new PXDataFieldAssign<RSSVRepairPrice.lastModifiedByScreenID>(dic["LastModifiedByScreenID"]),
									new PXDataFieldAssign<RSSVRepairPrice.lastModifiedDateTime>(graph.Accessinfo.BusinessDate),
									//new PXDataFieldAssign<RSSVRepairPrice.tstamp>(Convert.ToByte(dic["tstamp"])),
									new PXDataFieldAssign<RSSVRepairPrice.noteid>(Guid.NewGuid().ToString())
									);
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
			RSSVRepairItem repairItem = SelectFrom<RSSVRepairItem>.View.ReadOnly.Select(graph);
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
								PXDatabase.Insert<RSSVRepairItem>(
									new PXDataFieldAssign<RSSVRepairItem.serviceID>(Convert.ToInt32(dic["ServiceID"])),
									new PXDataFieldAssign<RSSVRepairItem.deviceID>(Convert.ToInt32(dic["DeviceID"])),
									new PXDataFieldAssign<RSSVRepairItem.lineNbr>(Convert.ToInt32(dic["LineNbr"])),
									new PXDataFieldAssign<RSSVRepairItem.repairItemType>(dic["RepairItemType"]),
									new PXDataFieldAssign<RSSVRepairItem.inventoryID>(Convert.ToInt32(dic["InventoryID"])),
									new PXDataFieldAssign<RSSVRepairItem.required>(Convert.ToInt32(dic["Required"])),
									new PXDataFieldAssign<RSSVRepairItem.isDefault>(Convert.ToInt32(dic["IsDefault"])),
									new PXDataFieldAssign<RSSVRepairItem.basePrice>(Convert.ToDecimal(dic["BasePrice"])),
									new PXDataFieldAssign<RSSVRepairItem.createdByID>(UserID),
									new PXDataFieldAssign<RSSVRepairItem.createdByScreenID>(dic["CreatedByScreenID"]),
									new PXDataFieldAssign<RSSVRepairItem.createdDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVRepairItem.lastModifiedByID>(UserID),
									new PXDataFieldAssign<RSSVRepairItem.lastModifiedByScreenID>(dic["LastModifiedByScreenID"]),
									new PXDataFieldAssign<RSSVRepairItem.lastModifiedDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVRepairItem.noteid>(Guid.NewGuid().ToString())
									);
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
			RSSVLabor labor = SelectFrom<RSSVLabor>.View.ReadOnly.Select(graph);
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
								PXDatabase.Insert<RSSVLabor>(
									new PXDataFieldAssign<RSSVLabor.inventoryID>(Convert.ToInt32(dic["InventoryID"])),
									new PXDataFieldAssign<RSSVLabor.deviceID>(Convert.ToInt32(dic["DeviceID"])),
									new PXDataFieldAssign<RSSVLabor.serviceID>(Convert.ToInt32(dic["ServiceID"])),
									new PXDataFieldAssign<RSSVLabor.defaultPrice>(Convert.ToDecimal(dic["DefaultPrice"])),
									new PXDataFieldAssign<RSSVLabor.quantity>(Convert.ToDecimal(dic["Quantity"])),
									new PXDataFieldAssign<RSSVLabor.extPrice>(Convert.ToDecimal(dic["ExtPrice"])),
									new PXDataFieldAssign<RSSVLabor.createdByID>(UserID),
									new PXDataFieldAssign<RSSVLabor.createdByScreenID>(dic["CreatedByScreenID"]),
									new PXDataFieldAssign<RSSVLabor.createdDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVLabor.lastModifiedByID>(UserID),
									new PXDataFieldAssign<RSSVLabor.lastModifiedByScreenID>(dic["LastModifiedByScreenID"]),
									new PXDataFieldAssign<RSSVLabor.lastModifiedDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVLabor.noteid>(Guid.NewGuid().ToString())
									);
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
			RSSVWarranty warranty = SelectFrom<RSSVWarranty>.View.ReadOnly.Select(graph);
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
								PXDatabase.Insert<RSSVWarranty>(
									new PXDataFieldAssign<RSSVWarranty.contractID>(Convert.ToInt32(dic["ContractID"])),
									new PXDataFieldAssign<RSSVWarranty.deviceID>(Convert.ToInt32(dic["DeviceID"])),
									new PXDataFieldAssign<RSSVWarranty.serviceID>(Convert.ToInt32(dic["ServiceID"])),
									new PXDataFieldAssign<RSSVWarranty.defaultWarranty>(Convert.ToInt32(dic["DefaultWarranty"])),
									new PXDataFieldAssign<RSSVWarranty.createdByID>(UserID),
									new PXDataFieldAssign<RSSVWarranty.createdByScreenID>(dic["CreatedByScreenID"]),
									new PXDataFieldAssign<RSSVWarranty.createdDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVWarranty.lastModifiedByID>(UserID),
									new PXDataFieldAssign<RSSVWarranty.lastModifiedByScreenID>(dic["LastModifiedByScreenID"]),
									new PXDataFieldAssign<RSSVWarranty.lastModifiedDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVWarranty.noteid>(Guid.NewGuid().ToString())
									);
							}
							else break;
						}
					}
					this.WriteLog("RSSVWarranty updated");
				}
			}
			#endregion

			#region InventoryItem
			//Add data to InventoryItem
			InventoryItem invItem = SelectFrom<InventoryItem>.Where<InventoryItemExt.usrRepairItem.IsEqual<True>>.View.ReadOnly.Select(graph);
			if (invItem == null)
			{
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
								PXDatabase.Insert<InventoryItem>(
									new PXDataFieldAssign<InventoryItem.inventoryID>(Convert.ToInt32(dic["InventoryID"])),
									new PXDataFieldAssign<InventoryItem.descr>(dic["Descr"]),
									new PXDataFieldAssign<InventoryItem.basePrice>(Convert.ToDecimal(dic["BasePrice"])),
									new PXDataFieldAssign<InventoryItem.itemClassID>(Convert.ToInt32(dic["ItemClassID"])),
									new PXDataFieldAssign<InventoryItemExt.usrRepairItem>(Convert.ToInt32(dic["UsrRepairItem"])),
									new PXDataFieldAssign<InventoryItemExt.usrRepairItemType>(dic["UsrRepairItemType"]),
									new PXDataFieldAssign<InventoryItem.createdByID>(UserID),
									new PXDataFieldAssign<InventoryItem.createdByScreenID>(dic["CreatedByScreenID"]),
									new PXDataFieldAssign<InventoryItem.createdDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<InventoryItem.lastModifiedByID>(UserID),
									new PXDataFieldAssign<InventoryItem.lastModifiedByScreenID>(dic["LastModifiedByScreenID"]),
									new PXDataFieldAssign<InventoryItem.lastModifiedDateTime>(graph.Accessinfo.BusinessDate)
									);
							}
							else break;
						}
					}
					this.WriteLog("InventoryItem updated");
				}
			}
			#endregion

			#region RSSVStockItemDevice
			//Add data to RSSVStockItemDevice
			RSSVStockItemDevice stockItemDevice = SelectFrom<RSSVStockItemDevice>.View.ReadOnly.Select(graph);
			if (stockItemDevice == null)
			{
				using (StreamReader file = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "InputData\\RSSVStockItemDevice.csv"))
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
								PXDatabase.Insert<RSSVStockItemDevice>(
									new PXDataFieldAssign<RSSVStockItemDevice.deviceID>(Convert.ToInt32(dic["DeviceID"])),
									new PXDataFieldAssign<RSSVStockItemDevice.inventoryID>(Convert.ToInt32(dic["InventoryID"])),
									new PXDataFieldAssign<RSSVStockItemDevice.createdByID>(UserID),
									new PXDataFieldAssign<RSSVStockItemDevice.createdByScreenID>(dic["CreatedByScreenID"]),
									new PXDataFieldAssign<RSSVStockItemDevice.createdDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVStockItemDevice.lastModifiedByID>(UserID),
									new PXDataFieldAssign<RSSVStockItemDevice.lastModifiedByScreenID>(dic["LastModifiedByScreenID"]),
									new PXDataFieldAssign<RSSVStockItemDevice.lastModifiedDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVStockItemDevice.noteid>(Guid.NewGuid().ToString())
									);
							}
							else break;
						}
					}
					this.WriteLog("RSSVStockItemDevice updated");
				}
			}
			#endregion

			#endregion

			#region T220Data

			#region RSSVWorkOrder
			//Add data to RSSVWorkOrder
			RSSVWorkOrder workOrder = SelectFrom<RSSVWorkOrder>.View.ReadOnly.Select(graph);
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
								PXDatabase.Insert<RSSVWorkOrder>(
									new PXDataFieldAssign<RSSVWorkOrder.orderNbr>(dic["OrderNbr"]),
									new PXDataFieldAssign<RSSVWorkOrder.customerID>(Convert.ToInt32(dic["CustomerID"])),
									new PXDataFieldAssign<RSSVWorkOrder.dateCreated>(Convert.ToDateTime(dic["DateCreated"])),
									//new PXDataFieldAssign<RSSVWorkOrder.dateCompleted>(Convert.ToDateTime(dic["DateCompleted"])),
									new PXDataFieldAssign<RSSVWorkOrder.status>(dic["Status"]),
									new PXDataFieldAssign<RSSVWorkOrder.hold>(Convert.ToInt32(dic["Hold"])),
									new PXDataFieldAssign<RSSVWorkOrder.description>(dic["Description"]),
									new PXDataFieldAssign<RSSVWorkOrder.deviceID>(Convert.ToInt32(dic["DeviceID"])),
									new PXDataFieldAssign<RSSVWorkOrder.serviceID>(Convert.ToInt32(dic["ServiceID"])),
									new PXDataFieldAssign<RSSVWorkOrder.orderTotal>(Convert.ToDecimal(dic["OrderTotal"])),
									new PXDataFieldAssign<RSSVWorkOrder.repairItemLineCntr>(Convert.ToInt32(dic["RepairItemLineCntr"])),
									new PXDataFieldAssign<RSSVWorkOrder.priority>(dic["Priority"]),
									new PXDataFieldAssign<RSSVWorkOrder.createdByID>(UserID),
									new PXDataFieldAssign<RSSVWorkOrder.createdByScreenID>(dic["CreatedByScreenID"]),
									new PXDataFieldAssign<RSSVWorkOrder.createdDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVWorkOrder.lastModifiedByID>(UserID),
									new PXDataFieldAssign<RSSVWorkOrder.lastModifiedByScreenID>(dic["LastModifiedByScreenID"]),
									new PXDataFieldAssign<RSSVWorkOrder.lastModifiedDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVWorkOrder.noteid>(Guid.NewGuid().ToString())
									);
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
			RSSVWorkOrderItem workOrderItem = SelectFrom<RSSVWorkOrderItem>.View.ReadOnly.Select(graph);
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
								IDictionary<string, string> dic = headerParts.Select((k, i) => new { k, v = lineParts[i] }).ToDictionary(x => x.k, x => x.v);
								PXDatabase.Insert<RSSVWorkOrderItem>(
									new PXDataFieldAssign<RSSVWorkOrderItem.orderNbr>(dic["OrderNbr"]),
									new PXDataFieldAssign<RSSVWorkOrderItem.deviceID>(Convert.ToInt32(dic["DeviceID"])),
									new PXDataFieldAssign<RSSVWorkOrderItem.serviceID>(Convert.ToInt32(dic["ServiceID"])),
									new PXDataFieldAssign<RSSVWorkOrderItem.lineNbr>(Convert.ToInt32(dic["LineNbr"])),
									new PXDataFieldAssign<RSSVWorkOrderItem.repairItemType>(dic["RepairItemType"]),
									new PXDataFieldAssign<RSSVWorkOrderItem.inventoryID>(Convert.ToInt32(dic["InventoryID"])),
									new PXDataFieldAssign<RSSVWorkOrderItem.basePrice>(Convert.ToDecimal(dic["BasePrice"])),
									new PXDataFieldAssign<RSSVWorkOrderItem.createdByID>(UserID),
									new PXDataFieldAssign<RSSVWorkOrderItem.createdByScreenID>(dic["CreatedByScreenID"]),
									new PXDataFieldAssign<RSSVWorkOrderItem.createdDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVWorkOrderItem.lastModifiedByID>(UserID),
									new PXDataFieldAssign<RSSVWorkOrderItem.lastModifiedByScreenID>(dic["LastModifiedByScreenID"]),
									new PXDataFieldAssign<RSSVWorkOrderItem.lastModifiedDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVWorkOrderItem.noteid>(Guid.NewGuid().ToString())
									);
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
			RSSVWorkOrderLabor workOrderLabor = SelectFrom<RSSVWorkOrderLabor>.View.ReadOnly.Select(graph);
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
								PXDatabase.Insert<RSSVWorkOrderLabor>(
									new PXDataFieldAssign<RSSVWorkOrderLabor.inventoryID>(Convert.ToInt32(dic["InventoryID"])),
									new PXDataFieldAssign<RSSVWorkOrderLabor.deviceID>(Convert.ToInt32(dic["DeviceID"])),
									new PXDataFieldAssign<RSSVWorkOrderLabor.serviceID>(Convert.ToInt32(dic["ServiceID"])),
									new PXDataFieldAssign<RSSVWorkOrderLabor.orderNbr>(dic["OrderNbr"]),
									new PXDataFieldAssign<RSSVWorkOrderLabor.defaultPrice>(Convert.ToDecimal(dic["DefaultPrice"])),
									new PXDataFieldAssign<RSSVWorkOrderLabor.quantity>(Convert.ToDecimal(dic["Quantity"])),
									new PXDataFieldAssign<RSSVWorkOrderLabor.extPrice>(Convert.ToDecimal(dic["ExtPrice"])),
									new PXDataFieldAssign<RSSVWorkOrderLabor.createdByID>(UserID),
									new PXDataFieldAssign<RSSVWorkOrderLabor.createdByScreenID>(dic["CreatedByScreenID"]),
									new PXDataFieldAssign<RSSVWorkOrderLabor.createdDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVWorkOrderLabor.lastModifiedByID>(UserID),
									new PXDataFieldAssign<RSSVWorkOrderLabor.lastModifiedByScreenID>(dic["LastModifiedByScreenID"]),
									new PXDataFieldAssign<RSSVWorkOrderItem.lastModifiedDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVWorkOrderLabor.noteid>(Guid.NewGuid().ToString())
									);
							}
							else break;
						}
					}
					this.WriteLog("RSSVWorkOrderItem updated");
				}
			}
			#endregion

			#region RSSVSetup
			//Add data to RSSVSetup
			RSSVSetup setup = SelectFrom<RSSVSetup>.View.ReadOnly.Select(graph);
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
								PXDatabase.Insert<RSSVSetup>(
									new PXDataFieldAssign<RSSVSetup.numberingId>(dic["NumberingId"]),
									new PXDataFieldAssign<RSSVSetup.walkInCustomerID>(Convert.ToInt32(dic["WalkInCustomerID"])),
									new PXDataFieldAssign<RSSVSetup.createdByID>(UserID),
									new PXDataFieldAssign<RSSVSetup.createdByScreenID>(dic["CreatedByScreenID"]),
									new PXDataFieldAssign<RSSVSetup.createdDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVSetup.lastModifiedByID>(UserID),
									new PXDataFieldAssign<RSSVSetup.lastModifiedByScreenID>(dic["LastModifiedByScreenID"]),
									new PXDataFieldAssign<RSSVSetup.lastModifiedDateTime>(graph.Accessinfo.BusinessDate),
									new PXDataFieldAssign<RSSVSetup.noteid>(Guid.NewGuid().ToString())
									);
							}
							else break;
						}
					}
					this.WriteLog("RSSVSetup updated");
				}
			}
			#endregion

			#endregion
		}
	}
}