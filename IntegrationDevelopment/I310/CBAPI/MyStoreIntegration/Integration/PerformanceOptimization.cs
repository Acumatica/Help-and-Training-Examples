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

                //Specify the fields in Summary, Details, and Shipments to be returned
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
                },
                Shipments = new SalesOrderShipment[]
                {
                    new SalesOrderShipment
                    {
                        ShipmentNbr = new StringReturn(),
                        InvoiceNbr = new StringReturn()
                    }
                }
            };

            //Get the list of sales orders with details
            Entity[] soList = soapClient.GetList(ordersToBeFound);

            //Save results to a CSV file
            using (StreamWriter file = new StreamWriter(string.Format(@"SalesOrder_Customer_{0}.csv", customerID)))
            {
                //Add headers to the file
                file.WriteLine("OrderType;OrderNbr;CustomerID;CustomerOrder;Date;OrderedQty;OrderTotal;InventoryID;OrderQty;UnitPrice;ShipmentNbr;InvoiceNbr;");

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
                    foreach (SalesOrderShipment shipment in salesOrder.Shipments)
                    {
                        file.WriteLine(string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};",
                            //Document summary
                            salesOrder.OrderType.Value,
                            salesOrder.OrderNbr.Value,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            null,
                            shipment.ShipmentNbr.Value,
                            shipment.InvoiceNbr.Value));
                    }
                }
            }
        }

        //Retrieving the list of sales orders of a customer in batches
        public static void ExportSalesOrdersInBatches(DefaultSoapClient soapClient)
        {
            Console.WriteLine("Getting the list of sales orders of a customer in batches...");

            //Customer data
            string customerID = "C000000003";

            //Row counters
            int count1 = 0;
            int count2 = 4;

            //Empty list indentifier
            bool empty = false;

            while (!empty)
            {
                //Specify the customer ID of a customer whose sales orders should be exported
                //and the fields to be returned
                SalesOrder ordersToBeFound = new SalesOrder
                {
                    //Return only the specified values
                    ReturnBehavior = ReturnBehavior.OnlySpecified,

                    //Specify the customer whose sales order should be returned
                    CustomerID = new StringSearch { Value = customerID },

                    //Specify the fields in Summary to be returned
                    OrderType = new StringReturn(),
                    OrderNbr = new StringReturn(),
                    OrderTotal = new DecimalReturn(),

                    //Specify the row numbers to be returned
                    RowNumber = new LongSearch { Value = count1, Value2 = count2, Condition = LongCondition.IsBetween }
                };

                //Get the list of sales orders with details
                Entity[] soList = soapClient.GetList(ordersToBeFound);

                if (soList.Count() > 0)
                {
                    //Save results to a CSV file
                    using (StreamWriter file = new StreamWriter(string.Format(@"SalesOrder_Customer_{0}_{1}.csv", customerID, count2 + 1)))
                    {

                        //Add headers to the file
                        file.WriteLine("OrderType;OrderNbr;CustomerID;OrderTotal;");

                        //Write the values for each sales order
                        foreach (SalesOrder salesOrder in soList)
                        {
                            file.WriteLine(string.Format("{0};{1};{2};{3};",
                                //Document summary
                                salesOrder.OrderType.Value,
                                salesOrder.OrderNbr.Value,
                                salesOrder.CustomerID.Value,
                                salesOrder.OrderTotal.Value
                                ));
                        }
                    }
                    count1 += 5;
                    count2 += 5;
                }
                else
                {
                    empty = true;
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
