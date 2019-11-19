using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStoreIntegrationSBAPI.SO643000;
using System.IO;
using PX.Soap;

namespace MyStoreIntegrationSBAPI.Integration
{
    class Report
    {
        //Getting the printable version of an invoice
        //on the Invoice & Memo (S0643000) report form
        public static void GetPrintableInvoice(Screen context)
        {
            //Invoice data
            string docType = "Invoice";
            string invoiceNbr = "INV000045";
            Console.WriteLine("Generating the printable version of an invoice...");
            //Get the schema of the Invoice & Memo form (S0643000)
            //by using the screen-based API wrapper
            Content invoiceFormSchema =
                PX.Soap.Helper.GetSchema<Content>(context);
            //Specify the needed invoice and get a PDF version of it
            var commands = new Command[]
            {
                new Value
                {
                    Value = docType,
                    LinkedCommand = invoiceFormSchema.Parameters.DocumentType
                },
                new Value
                {
                    Value = invoiceNbr,
                    LinkedCommand = invoiceFormSchema.Parameters.ReferenceNumber
                },
                invoiceFormSchema.ReportResults.PdfContent
            };
            //Submit the commands to the form
            Content[] pdfInvoice = context.Submit(commands);
            //Save the result in a PDF file
            if (pdfInvoice != null && pdfInvoice.Length > 0)
            {
                File.WriteAllBytes(string.Format(@"Invoice_{0}.pdf", invoiceNbr),
                Convert.FromBase64String(pdfInvoice[0].ReportResults.PdfContent.Value));
            }
        }
    }
}
