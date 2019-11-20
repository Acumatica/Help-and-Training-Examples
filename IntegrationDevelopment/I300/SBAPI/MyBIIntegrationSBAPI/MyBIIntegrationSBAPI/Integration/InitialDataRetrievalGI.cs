using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBIIntegrationSBAPI.MyBIIntegration;
using System.IO;

namespace MyBIIntegrationSBAPI.Integration
{
    class InitialDataRetrievalGI
    {
        //Retrieving the item availability data by using a generic inquiry
        public static void ExportItemQuantities(Screen screen)
        {
            Console.WriteLine("Retrieving stock item quantities...");
            //Get the schema of the Item Availability Data generic inquiry
            //by using the screen-based API wrapper
            INGI0002Content schema =
                PX.Soap.Helper.GetSchema<INGI0002Content>(screen);
            //Configure the sequence of commands
            var commands = new Command[]
            {
                schema.Result.Warehouse,
                schema.Result.InventoryID,
                schema.Result.Description,
                schema.Result.QtyOnHand,
                schema.Result.QtyAvailable,
            };
            //Retrieve the list of items
            String[][] items = screen.INGI0002Export(commands, null, 0, true, false);
            //Save the data to a CSV file
            StreamWriter file = new StreamWriter("ItemAvailabilityData.csv");
            {
                foreach (string[] rows in items)
                {
                    foreach (string row in rows)
                    {
                        file.Write(row + ";");
                    }
                    file.WriteLine();
                }
            }
            file.Close();
        }
    }

}
