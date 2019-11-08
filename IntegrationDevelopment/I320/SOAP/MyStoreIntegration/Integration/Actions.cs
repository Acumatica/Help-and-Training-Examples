using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStoreIntegration.Default;
using MyStoreIntegration.Helper;

namespace MyStoreIntegration.Integration
{
    class Actions
    {
        //Releasing an invoice on the Invoices form (SO303000)
        public static void ReleaseSOInvoice(DefaultSoapClient soapClient)
        {
            Console.WriteLine("Releasing an invoice...");

            //Invoice data
            string invoiceType = "Invoice";
            string referenceNbr = "INV000047";

            //Specify the values of a new invoice
            SalesInvoice soInvoice = new SalesInvoice
            {
                Type = new StringValue { Value = invoiceType },
                ReferenceNbr = new StringValue { Value = referenceNbr },
                Hold = new BooleanValue { Value = false }
            };

            //Release invoice
            InvokeResult invokeResult = soapClient.Invoke(soInvoice, new ReleaseSalesInvoice());

            //Monitor the status of the process
            ProcessResult processResult = LongRunProcessor.GetProcessResult(soapClient, invokeResult);

            //Get the confirmed shipment
            soInvoice = (SalesInvoice)soapClient.Get(new SalesInvoice { ID = processResult.EntityId });
            //Display the summary of the invoice
            Console.WriteLine("Invoice type: " + soInvoice.Type.Value);
            Console.WriteLine("Invoice number: " + soInvoice.ReferenceNbr.Value);
            Console.WriteLine("Invoice status: " + soInvoice.Status.Value);
            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }
    }
}
