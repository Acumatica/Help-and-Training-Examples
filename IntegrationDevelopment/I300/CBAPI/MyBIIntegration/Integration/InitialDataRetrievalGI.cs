using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBIIntegration.ItemAvailabilityData;
using System.IO;

namespace MyBIIntegration.Integration
{
    class InitialDataRetrievalGI
    {
        //Retrieving the quantities of stock items
        public static void RetrieveItemQuantities(
            DefaultSoapClient soapClient)
        {
            Console.WriteLine("Retrieving the quantities of stock items...");

            //Return all columns of the generic inquiry
            ItemAvailabilityDataInquiry inqParameters = new ItemAvailabilityDataInquiry
            {
                //InventoryID = new StringValue { Value = "AALEGO500" },
                Results = new Result[]
                {
                    new Result
                    {
                        ReturnBehavior = ReturnBehavior.All
                    }
                }
            };

            //Retrieve the quantities of items
            ItemAvailabilityDataInquiry results = 
                (ItemAvailabilityDataInquiry)soapClient.Put(inqParameters);

            //Save results to a CSV file
            using (StreamWriter file = new StreamWriter("ItemAvailabilityData.csv"))
            {
                //Add headers to the file
                file.WriteLine(
                    "InventoryID;Description;Warehouse;QtyAvailable;QtyOnHand;");

                //Write the values for each item
                foreach ( Result result in results.Results)
                {
                    file.WriteLine(
                        string.Format("{0};{1};{2};{3};{4};",
                        result.InventoryID.Value,
                        result.Description.Value,
                        result.Warehouse.Value,
                        result.QtyAvailable.Value,
                        result.QtyOnHand.Value));
                }
            }
        }
    }
}
