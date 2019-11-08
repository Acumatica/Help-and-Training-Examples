using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStoreIntegration.Default;

namespace MyStoreIntegration.Integration
{
    class CreationOfRecords
    {
        //Creating a shipment 
        public static void CreateShipment(DefaultSoapClient soapClient)
        {
            Console.WriteLine("Creating a shipment...");

            //Shipment data
            string shipmentType = "Shipment";
            string customerID = "C000000003";
            string warehouse = "MAIN";
            //Sales order with the Open status for the specified customer
            string firstOrderNbr = "000004";
            string firstOrderType = "SO";
            //Sales order with the Open status for the specified customer
            string secondOrderNbr = "000006";
            string secondOrderType = "SO";

            //Find the first sales order to be shipped
            SalesOrder orderToBeFound1 = new SalesOrder
            {
                ReturnBehavior = ReturnBehavior.OnlySpecified,
                OrderType = new StringSearch { Value = firstOrderType },
                OrderNbr = new StringSearch { Value = firstOrderNbr },
                Details = new SalesOrderDetail[]
                {
                    new SalesOrderDetail
                    {
                        ReturnBehavior = ReturnBehavior.OnlySpecified,
                        InventoryID = new StringReturn(),
                        WarehouseID = new StringReturn()
                    }
                }
            };
            SalesOrder orderForShipment1 = (SalesOrder)soapClient.Get(orderToBeFound1);

            //Find the second sales order to be shipped
            SalesOrder orderToBeFound2 = new SalesOrder
            {
                ReturnBehavior = ReturnBehavior.OnlySpecified,
                OrderType = new StringSearch { Value = secondOrderType },
                OrderNbr = new StringSearch { Value = secondOrderNbr },
                Details = new SalesOrderDetail[]
                {
                    new SalesOrderDetail
                    {
                        ReturnBehavior = ReturnBehavior.OnlySpecified,
                        InventoryID = new StringReturn(),
                        WarehouseID = new StringReturn()
                    }
                }
            };
            SalesOrder orderForShipment2 = (SalesOrder)soapClient.Get(orderToBeFound2);

            //Select all stock items from the sales orders for shipment
            List<ShipmentDetail> shipmentDetails = new List<ShipmentDetail>();
            foreach (SalesOrderDetail item in orderForShipment1.Details)
            {
                shipmentDetails.Add(new ShipmentDetail
                {
                    OrderType = orderForShipment1.OrderType,
                    OrderNbr = orderForShipment1.OrderNbr,
                    InventoryID = item.InventoryID,
                    WarehouseID = item.WarehouseID
                });
            }
            foreach (SalesOrderDetail item in orderForShipment2.Details)
            {
                shipmentDetails.Add(new ShipmentDetail
                {
                    OrderType = orderForShipment2.OrderType,
                    OrderNbr = orderForShipment2.OrderNbr,
                    InventoryID = item.InventoryID,
                    WarehouseID = item.WarehouseID
                });
            }

            //Specify the values of a new shipment
            Shipment shipment = new Shipment
            {
                ReturnBehavior = ReturnBehavior.OnlySpecified,
                Type = new StringValue { Value = shipmentType },
                ShipmentNbr = new StringReturn(),
                Status = new StringReturn(),
                CustomerID = new StringValue { Value = customerID },
                WarehouseID = new StringValue { Value = warehouse },
                Details = shipmentDetails.ToArray()
            };

            //Create a shipment with the specified values
            Shipment newShipment = (Shipment)soapClient.Put(shipment);

            //Display the summary of the created record
            Console.WriteLine("Shipment number: " + newShipment.ShipmentNbr.Value);
            Console.WriteLine("Shipment type: " + newShipment.Type.Value);
            Console.WriteLine("Shipment status: " + newShipment.Status.Value);
            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }

        //Creating a stock item with attributes
        public static void CreateStockItem(DefaultSoapClient soapClient)
        {
            Console.WriteLine("Creating a stock item with attributes...");

            //Stock item data
            string inventoryID = "BASESERV2";
            string itemDescription = "Baseline level of performance";
            string itemClass = "STOCKITEM"; //Item class that has attributes defined
            string attributeName1 = "Operation System"; //An attribute of the item class (STOCKITEM)
            string attributeValue1 = "Windows";
            string attributeName2 = "Version Of Software"; //An attribute of the item class (STOCKITEM)
            string attributeValue2 = "Server 2012 R2";

            //Specify the values of the new stock item
            StockItem stockItemToBeCreated = new StockItem
            {
                ReturnBehavior = ReturnBehavior.OnlySpecified,
                InventoryID = new StringValue { Value = inventoryID },
                Description = new StringValue { Value = itemDescription },
                ItemClass = new StringValue { Value = itemClass },
                Attributes = new[]
                {
                    new AttributeValue
                    {
                        AttributeID = new StringValue { Value = attributeName1 },
                        Value = new StringValue { Value = attributeValue1 }
                    },
                    new AttributeValue
                    {
                        AttributeID = new StringValue { Value = attributeName2 },
                        Value = new StringValue { Value = attributeValue2 }
                    }
                }

            };

            //Create a stock item with the specified values
            StockItem newStockItem = (StockItem)soapClient.Put(stockItemToBeCreated);

            //Display the summary of the created stock item
            Console.WriteLine("Inventory ID: " + newStockItem.InventoryID.Value);
            foreach (AttributeValue attr in newStockItem.Attributes)
            {
                Console.WriteLine("Attribute name: " + attr.AttributeID.Value);
                Console.WriteLine("Attribute value: " + attr.Value.Value);
            }
            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }
    }
}
