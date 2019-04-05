using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStoreIntegration.Default;
using System.IO;

namespace MyStoreIntegration.Integration
{
    class PerformanceOptimization
    {
        //Retrieving the list of sales orders of a customer 
        public static void ExportSalesOrders(DefaultSoapClient soapClient)
        {
            Console.WriteLine("Getting the list of sales orders of a customer...");

            //Customer data
            string customerID = "C000000003";

            //Specify the customer ID of a customer whose sales orders should be exported
            //and the fields to be returned
            SalesOrder ordersToBeFound = new SalesOrder
            {
                //Return only the specified values
                ReturnBehavior = ReturnBehavior.OnlySpecified,

                //Specify the customer whose sales order should be returned
                CustomerID = new StringSearch { Value = customerID },

                //Specify the fields in Summary and Details to be returned
                OrderType = new StringReturn(),
                OrderNbr = new StringReturn(),
                CustomerOrder = new StringReturn(),
                Date = new DateTimeReturn(),
                OrderedQty = new DecimalReturn(),
                OrderTotal = new DecimalReturn(),
                Details = new SalesOrderDetail[]
                {
                    new SalesOrderDetail
                    {
                        InventoryID = new StringReturn(),
                        OrderQty = new DecimalReturn(),
                        UnitPrice = new DecimalReturn()
                    }
                }
            };

            //Get the list of sales orders with details
            Entity[] soList = soapClient.GetList(ordersToBeFound);

            //Save results to a CSV file
            using (StreamWriter file = new StreamWriter(string.Format(@"SalesOrderDetails_Customer_{0}.csv", customerID)))
            {
                //Add headers to the file
                file.WriteLine("OrderType;OrderNbr;CustomerID;CustomerOrder;Date;OrderedQty;OrderTotal;InventoryID;OrderQty;UnitPrice;");

                //Write the values for each sales order
                foreach (SalesOrder salesOrder in soList)
                {
                    foreach (SalesOrderDetail detail in salesOrder.Details)
                    { 
                        file.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};",
                            //Document summary
                            salesOrder.OrderType.Value,
                            salesOrder.OrderNbr.Value,
                            salesOrder.CustomerID.Value,
                            salesOrder.CustomerOrder.Value,
                            salesOrder.Date.Value,
                            salesOrder.OrderedQty.Value,
                            salesOrder.OrderTotal.Value,
                            detail.InventoryID.Value,
                            detail.OpenQty.Value,
                            detail.UnitCost.Value));
                    }
                }
            }

            //Specify the fields in Shipments to be returned 
            ordersToBeFound = new SalesOrder
            {
                ReturnBehavior = ReturnBehavior.OnlySpecified,

                CustomerID = new StringSearch { Value = customerID },

                OrderType = new StringReturn(),
                OrderNbr = new StringReturn(),
                Shipments = new SalesOrderShipment[]
                {
                    new SalesOrderShipment
                    {
                        ShipmentNbr = new StringReturn(),
                        InvoiceNbr = new StringReturn()
                    }
                }
            };

            //Get the list of sales orders of the customer
            soList = soapClient.GetList(ordersToBeFound);

            //Save results to a CSV file
            using (StreamWriter file = new StreamWriter(string.Format(@"SalesOrderShipments_Customer_{0}.csv", customerID)))
            {
                //Add headers to the file
                file.WriteLine("OrderType;OrderNbr;ShipmentNbr;InvoiceNbr;");

                //Write the values for each sales order
                foreach (SalesOrder salesOrder in soList)
                {
                    foreach (SalesOrderShipment shipment in salesOrder.Shipments)
                    {
                        file.WriteLine(string.Format("{0};{1};{2};{3};",
                            //Document summary
                            salesOrder.OrderType.Value,
                            salesOrder.OrderNbr.Value,
                            shipment.ShipmentNbr.Value,
                            shipment.InvoiceNbr.Value));
                    }
                }
            }
        }

        //Retrieving the list of payments of a customer
        public static void ExportPayments(DefaultSoapClient soapClient)
        {
            Console.WriteLine("Retrieving the list of payments of a customer...");

            //Input data
            string customerID = "C000000003";
            string docType = "Payment";

            //Select the payments that should be exported
            Payment soPaymentsToBeFound = new Payment
            {
                ReturnBehavior = ReturnBehavior.OnlySpecified,

                Type = new StringSearch { Value = docType },
                CustomerID = new StringSearch { Value = customerID },

                ReferenceNbr = new StringReturn()
            };
            Entity[] payments = soapClient.GetList(soPaymentsToBeFound);

            //Retrieve the payments one by one
            foreach (Payment payment in payments)
            {
                Payment soPaymentToBeRetrieved = new Payment
                {
                    ReturnBehavior = ReturnBehavior.OnlySpecified,

                    Type = new StringSearch
                    {
                        Value = payment.Type.Value,
                    },
                    ReferenceNbr = new StringSearch
                    {
                        Value = payment.ReferenceNbr.Value,
                    },

                    ApplicationDate = new DateTimeReturn(),
                    Status = new StringReturn(),
                    ApplicationHistory = new[]
                    {
                        new PaymentApplicationHistoryDetail
                        {
                            DisplayDocType = new StringReturn(),
                            DisplayRefNbr = new StringReturn()
                        }
                    }
                };
                Payment result = (Payment)soapClient.Get(soPaymentToBeRetrieved);

                //Save results to a CSV file
                using (StreamWriter file = new StreamWriter(
                    string.Format(@"Payment_{0}.csv", payment.ReferenceNbr.Value)))
                {
                    file.Write(string.Format("{0};{1};{2};{3};",
                            //Document summary
                            result.Type.Value,
                            result.ReferenceNbr.Value,
                            result.ApplicationDate.Value,
                            result.Status.Value));
                    foreach (PaymentApplicationHistoryDetail detail in result.ApplicationHistory)
                    {
                        file.Write(string.Format("{0};{1};",                            
                            //Application details
                            detail.DisplayDocType.Value,
                            detail.DisplayRefNbr.Value));
                    }
                }
            }
        }
    }
}
