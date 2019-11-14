using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBIIntegration.Default;
using System.IO;

namespace MyBIIntegration.Integration
{
    class RetrievalOfDelta
    {
        //Retrieving the list of stock items
        public static void ExportStockItems(DefaultSoapClient soapClient)
        {
            Console.WriteLine("Retrieving the list of stock items...");
            //Specify the parameters of stock items to be returned
            StockItem stockItemsToBeFound = new StockItem
            {
                //Specify return behavior
                ReturnBehavior = ReturnBehavior.OnlySpecified,
                //Filter the items by the last modified date and status
                LastModified = new DateTimeSearch
                {
                    Value = DateTime.Now.AddDays(-1),
                    Condition = DateTimeCondition.IsGreaterThan
                },
                ItemStatus = new StringSearch { Value = "Active" },
                //Specify other fields to be returned
                InventoryID = new StringReturn(),
                Description = new StringReturn(),
                ItemClass = new StringReturn(),
                BaseUOM = new StringReturn(),
                WarehouseDetails = new StockItemWarehouseDetail[]
                {
                    new StockItemWarehouseDetail
                    {
                        ReturnBehavior = ReturnBehavior.OnlySpecified,
                        WarehouseID = new StringReturn(),
                        QtyOnHand = new DecimalReturn()
                    }
                }
            };
            //Get the list of stock items
            Entity[] stockItems = soapClient.GetList(stockItemsToBeFound);
            //Save the results to a CSV file
            using (StreamWriter file = new StreamWriter("StockItems.csv"))
            {
                //Add headers to the file
                file.WriteLine("InventoryID;Description;ItemClass;BaseUOM;" +
                "LastModified;WarehouseID;QtyOnHand;");
                //Write the values for each item
                foreach (StockItem stockItem in stockItems)
                {
                    foreach (
                    StockItemWarehouseDetail detail in stockItem.WarehouseDetails)
                    {
                        file.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6}",
                        stockItem.InventoryID.Value,
                        stockItem.Description.Value,
                        stockItem.ItemClass.Value,
                        stockItem.BaseUOM.Value,
                        stockItem.LastModified.Value,
                        detail.WarehouseID.Value,
                        detail.QtyOnHand.Value));
                    }
                }
            }
        }
    }
}
