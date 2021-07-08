using System;
using PX.Data;
using Customization;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using PX.Objects.IN;
using PX.Data.BQL.Fluent;
using System.Globalization;

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

            InventoryItem invItem = SelectFrom<InventoryItem>.Where<InventoryItemExt.usrRepairItem.IsEqual<True>>.View.ReadOnly.Select(iiEntry);
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
                                var iItem = new InventoryItem
                                {
                                    InventoryCD = dic["InventoryCD"],
                                    Descr = dic["Descr"],
                                    ItemClassID = Convert.ToInt32(dic["ItemClassID"]),
                                };
                                iItem = PXCache<InventoryItem>.CreateCopy(iiEntry.Item.Insert(iItem));
                                iItem.BasePrice = Decimal.Parse(dic["BasePrice"], NumberStyles.Any, CultureInfo.InvariantCulture);
                                iItem = PXCache<InventoryItem>.CreateCopy(iiEntry.Item.Update(iItem));
                                iItem.DfltSiteID = Convert.ToInt32(dic["DfltSiteID"]);
                                iItem = PXCache<InventoryItem>.CreateCopy(iiEntry.Item.Update(iItem));
                                var extItem = PXCache<InventoryItem>.GetExtension<InventoryItemExt>(iItem);
                                extItem.UsrRepairItem = true;
                                extItem.UsrRepairItemType = dic["UsrRepairItemType"];
                                iiEntry.Item.Update(iItem);
                                //iItem = PXCache<InventoryItem>.CreateCopy(iiEntry.Item.Update(iItem));
                                iiEntry.Actions.PressSave();
                                iiEntry.Clear();
                            }
                            else break;
                        }
                    }
                    this.WriteLog("InventoryItem updated");
                }
            }
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
                                    //Price = Decimal.Parse(dic["Price"], NumberStyles.Any, CultureInfo.InvariantCulture),
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
                                    BasePrice = Decimal.Parse(dic["BasePrice"], NumberStyles.Any, CultureInfo.InvariantCulture)
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
                                    DefaultPrice = Decimal.Parse(dic["DefaultPrice"], NumberStyles.Any, CultureInfo.InvariantCulture),
                                    Quantity = Decimal.Parse(dic["Quantity"], NumberStyles.Any, CultureInfo.InvariantCulture),
                                    ExtPrice = Decimal.Parse(dic["ExtPrice"], NumberStyles.Any, CultureInfo.InvariantCulture)
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

            #region RSSVStockItemDevice
            //Add data to RSSVStockItemDevice
            RSSVStockItemDevice stockItemDevice = SelectFrom<RSSVStockItemDevice>.View.ReadOnly.Select(iiEntry);
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
                                RSSVStockItemDevice device = new RSSVStockItemDevice
                                {
                                    DeviceID = Convert.ToInt32(dic["DeviceID"]),
                                    InventoryID = Convert.ToInt32(dic["InventoryID"]),
                                };
                                iiEntry.GetExtension<InventoryItemMaint_Extension>().CompatibleDevices.Insert(device);
                                iiEntry.Actions.PressSave();
                                iiEntry.Clear();
                            }
                            else break;
                        }
                    }
                    this.WriteLog("RSSVStockItemDevice updated");
                }
            }
            #endregion

            #endregion

        }
    }
}