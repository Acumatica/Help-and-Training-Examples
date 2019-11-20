using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBIIntegrationSBAPI.MyBIIntegration;
using MyBIIntegrationSBAPI.Integration;

namespace MyBIIntegrationSBAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            using (Screen screen = new Screen())
            {
                //Specify the connection parameters
                screen.CookieContainer = new System.Net.CookieContainer();
                screen.Url = Properties.Settings.
                Default.MyBIIntegrationSBAPI_MyBIIntegration_Screen;
                //Sign in to Acumatica ERP
                screen.Login
                (
                Properties.Settings.Default.Username,
                Properties.Settings.Default.Password
                );
                try
                {
                    //Retrieving the list of customers with contacts
                    //InitialDataRetrieval.ExportCustomers(screen);
                    //Retrieving the quantities of stock items
                    //InitialDataRetrievalGI.ExportItemQuantities(screen);
                    //Retrieving the list of stock items modified within the past day
                    RetrievalOfDelta.ExportStockItems(screen);
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
                    screen.Logout();
                }
            }
        }
    }
}
