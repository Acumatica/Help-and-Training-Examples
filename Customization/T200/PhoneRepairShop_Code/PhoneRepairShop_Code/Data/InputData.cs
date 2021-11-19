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

        }
    }
}