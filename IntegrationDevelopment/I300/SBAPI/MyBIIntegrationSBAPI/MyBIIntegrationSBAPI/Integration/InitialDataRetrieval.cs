using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBIIntegrationSBAPI.MyBIIntegration;
using System.IO;

namespace MyBIIntegrationSBAPI.Integration
{
    class InitialDataRetrieval
    {
        //Retrieving the list of customers
        public static void ExportCustomers(Screen screen)
        {
            Console.WriteLine("Retrieving the list of customers with contacts...");
            //Get the schema of the Customers (AR303000) form and
            //configure the sequence of commands
            AR303000Content custSchema =
            PX.Soap.Helper.GetSchema<AR303000Content>(screen);
            var commands = new Command[]
            {
                //Get the values of the needed elements
                custSchema.CustomerSummary.ServiceCommands.EveryCustomerID,
                //Customer summary
                custSchema.CustomerSummary.CustomerID,
                custSchema.CustomerSummary.CustomerName,
                //General Info tab, Financial Settings
                custSchema.GeneralInfoFinancialSettings.CustomerClass,
                //General Info tab, Main Contact
                custSchema.GeneralInfoMainContact.Email,
                custSchema.GeneralInfoMainContact.Phone1,
                //General Info tab, Main Address
                custSchema.GeneralInfoMainAddress.AddressLine1,
                custSchema.GeneralInfoMainAddress.AddressLine2,
                custSchema.GeneralInfoMainAddress.City,
                custSchema.GeneralInfoMainAddress.State,
                custSchema.GeneralInfoMainAddress.PostalCode
            };
            //Export the customer data
            string[][] customerData =
            screen.AR303000Export(commands, null, 0, true, false);
            //Save the data to a CSV file
            StreamWriter file = new StreamWriter("Customers.csv");
            {
                foreach (string[] rows in customerData)
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
