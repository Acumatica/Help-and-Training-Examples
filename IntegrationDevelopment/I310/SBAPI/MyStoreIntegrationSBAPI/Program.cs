using System;
using MyStoreIntegrationSBAPI.SO643000;
using MyStoreIntegrationSBAPI.Integration;

namespace MyStoreIntegrationSBAPI
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
                Default.MyStoreIntegrationSBAPI_SO643000_Screen;
                //Sign in to Acumatica ERP
                screen.Login
                (
                "admin",
                "123"
                );
                try
                {
                    Reports.GetPrintableInvoice(screen);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue");
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