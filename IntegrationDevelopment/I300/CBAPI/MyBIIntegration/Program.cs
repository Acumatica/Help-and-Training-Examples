using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBIIntegration.Default;
using MyBIIntegration.Integration;
using MyBIIntegration.ItemAvailabilityData;

namespace MyBIIntegration
{
    class Program
    {
        static void Main(string[] args)
        {
            //Using the Default/18.200.001 endpoint
            using (Default.DefaultSoapClient soapClient =
                 new Default.DefaultSoapClient())
            {
                //Sign in to Acumatica ERP
                soapClient.Login
                (
                    Properties.Settings.Default.Username,
                    Properties.Settings.Default.Password,
                    Properties.Settings.Default.Tenant,
                    Properties.Settings.Default.Branch,
                    null
                );

                try
                {
                    //Retrieving the list of customers with contacts
                    //InitialDataRetrieval.RetrieveListOfCustomers(soapClient);
                    //Retrieving the list of stock items modified within the past day
                    RetrievalOfDelta.ExportStockItems(soapClient);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine();
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();
                }
                finally
                {
                    //Sign out from Acumatica ERP
                    soapClient.Logout();
                }
            }
            //Using the ItemAvailabilityData/0001 endpoint
            using (ItemAvailabilityData.DefaultSoapClient soapClient =
            new ItemAvailabilityData.DefaultSoapClient())
            {
                //Sign in to Acumatica ERP
                soapClient.Login
                (
                    Properties.Settings.Default.Username,
                    Properties.Settings.Default.Password,
                    Properties.Settings.Default.Tenant,
                    Properties.Settings.Default.Branch,
                    null
                );
                try
                {
                    //Retrieving the quantities of stock items
                    //InitialDataRetrievalGI.RetrieveItemQuantities(soapClient);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine();
                    Console.WriteLine("Press Enter to continue");
                    Console.ReadLine();
                }
                finally
                {
                    //Sign out from Acumatica ERP
                    soapClient.Logout();
                }
            }
        }
    }
}
