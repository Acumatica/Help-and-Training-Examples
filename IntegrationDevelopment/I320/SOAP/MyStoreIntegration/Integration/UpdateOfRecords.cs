using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStoreIntegration.Default;

namespace MyStoreIntegration.Integration
{
    class UpdateOfRecords
    {
        //Updating a customer record
        public static void UpdateCustomer(DefaultSoapClient soapClient)
        {
            Console.WriteLine("Updating a customer record...");

            //Customer data
            string customerMainContactEmail = "info@jevy-comp.con"; //Specify the email address of a customer that exists in the system
            string customerClass = "INTL"; //Specify one of the customer classes that are configured in the system
            string contactTitle = "Mr.";
            string contactFirstName = "Jack";
            string contactLastName = "Green";
            string contactEmail = "green@jevy-comp.con";

            //Select the needed customer record and 
            //specify the values that should be updated
            Customer customerToBeUpdated = new Customer
            {
                ReturnBehavior = ReturnBehavior.OnlySpecified,
                CustomerID = new StringReturn(),
                MainContact = new Contact
                {
                    //Search for the customer record by email address
                    Email = new StringSearch { Value = customerMainContactEmail },
                },
                CustomerClass = new StringValue { Value = customerClass },
                //Specify the values of the customer billing contact
                BillingContactSameAsMain = new BooleanValue { Value = false },
                BillingContact = new Contact
                {
                    Email = new StringValue { Value = contactEmail },
                    Attention = new StringValue { Value = contactTitle + " " + contactFirstName + " " + contactLastName }
                }
            };

            //Update the customer record with the specified values
            Customer updCustomer = (Customer)soapClient.Put(customerToBeUpdated);

            //Display the ID and customer class of the updated record
            Console.WriteLine("Customer ID: " + updCustomer.CustomerID.Value);
            Console.WriteLine("Customer class: " + updCustomer.CustomerClass.Value);
            Console.WriteLine("Billing contact name: " + updCustomer.BillingContact.Attention.Value);
            Console.WriteLine("Billing contact email: " + updCustomer.BillingContact.Email.Value);
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
        }

        //Updating the detail lines of a sales order
        public static void UpdateSO(DefaultSoapClient soapClient)
        {
            Console.WriteLine("Updating a sales order...");

            //Sales order data
            string orderType = "SO";
            //A unique value that identifies a sales order
            string customerOrder = "SO248-563-06";                                                    
            string firstItemInventoryID = "CONTABLE1";
            string firstItemWarehouse = "MAIN";
            string secondItemInventoryID = "AALEGO500";
            string secondItemWarehouse = "MAIN";
            decimal secondItemQuantity = 4;

            //Find the sales order to be updated
            SalesOrder soToBeFound = new SalesOrder
            {
                ReturnBehavior = ReturnBehavior.OnlySpecified,
                OrderType = new StringSearch { Value = orderType },
                CustomerOrder = new StringSearch { Value = customerOrder },
                OrderNbr = new StringReturn(),
                Details = new SalesOrderDetail[]
                {
                    new SalesOrderDetail
                    {
                        ReturnBehavior = ReturnBehavior.All
                    }
                }
            };
            SalesOrder order = (SalesOrder)soapClient.Get(soToBeFound);

            //Find the line to be deleted and mark it for deletion
            //The Single method makes the program find 
            //the only SalesOrderDetail of order.Details 
            //that has the specified InventoryID and WarehouseID
            SalesOrderDetail orderLine = order.Details.Single(
                orderLineToBeDeleted =>
                    orderLineToBeDeleted.InventoryID.Value == firstItemInventoryID &&
                    orderLineToBeDeleted.WarehouseID.Value == firstItemWarehouse);
            orderLine.Delete = true;

            //Find the line to be updated and update the quantity in it
            orderLine = order.Details.Single(
            orderLineToBeUpdated =>
                    orderLineToBeUpdated.InventoryID.Value == secondItemInventoryID &&
                    orderLineToBeUpdated.WarehouseID.Value == secondItemWarehouse);
            orderLine.OrderQty = new DecimalValue { Value = secondItemQuantity };

            //Specify the additional values to be returned
            order.OrderedQty = new DecimalReturn();
            order.OrderTotal = new DecimalReturn();

            //Update the sales order
            order = (SalesOrder)soapClient.Put(order);

            //Display the summary of the updated record
            Console.WriteLine("Order type: " + order.OrderType.Value);
            Console.WriteLine("Order number: " + order.OrderNbr.Value);
            Console.WriteLine("Ordered quantity: " + order.OrderedQty.Value);
            Console.WriteLine("Order total: " + order.OrderTotal.Value);
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
        }
    }
}
