using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStoreIntegration.Default;
using System.IO;

namespace MyStoreIntegration.Integration
{
    class Attachments
    {
        //Retrieving the files that are attached to a stock item
        public static void ExportStockItemFiles(DefaultSoapClient soapClient)
        {
            Console.WriteLine(
             "Retrieving the files that are attached to a stock item...");

            //Parameters of filtering
            string inventoryID = "AAMACHINE1";

            //Filter the items by inventory ID
            StockItem stockItemToBeFound = new StockItem
            {
                InventoryID = new StringSearch { Value = inventoryID },
                ImageUrl = new StringReturn(),
                ReturnBehavior = ReturnBehavior.OnlySpecified
            };

            //Get the stock item record
            StockItem stockItem = (StockItem)soapClient.Get(stockItemToBeFound);

            //Get the files that are attached to the stock item and 
            //save them on a local disc
            if (stockItem != null && stockItem.ImageUrl != null)
            {
                //Get the attached files
                Default.File[] files = soapClient.GetFiles(stockItem);

                //Save the files on disc
                foreach (Default.File file in files)
                {
                    //The file name obtained from Acumatica ERP has the following 
                    //format: Stock Items (<Inventory ID>)\<File Name>
                    string fileName = Path.GetFileName(file.Name);
                    System.IO.File.WriteAllBytes(fileName, file.Content);
                }
            }
        }
    }
}
