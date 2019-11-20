using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyBIIntegration.Default;
using System.IO;


namespace MyBIIntegration.Integration
{
    class InitialDataRetrieval
    {
        //Retrieving the list of customers with contacts
        public static void RetrieveListOfCustomers(
        DefaultSoapClient soapClient)
        {
            Console.WriteLine("Retrieving the list of customers with contacts...");
            //Specify the parameters of the stock items to be returned
            Customer customersToBeRetrieved = new Customer
            {
                //Return the values of only the specified fields
                ReturnBehavior = ReturnBehavior.OnlySpecified,
                //Specify the other fields to be returned
                CustomerID = new StringReturn(),
                CustomerName = new StringReturn(),
                CustomerClass = new StringReturn(),
                MainContact = new Contact
                {
                    ReturnBehavior = ReturnBehavior.OnlySpecified,
                    Email = new StringReturn(),
                    Phone1 = new StringReturn(),
                    Address = new Address
                    {
                        ReturnBehavior = ReturnBehavior.OnlySpecified,
                        AddressLine1 = new StringReturn(),
                        AddressLine2 = new StringReturn(),
                        City = new StringReturn(),
                        State = new StringReturn(),
                        PostalCode = new StringReturn()
                    }
                }
            };
            //Get the list of customers
            Entity[] customers = soapClient.GetList(customersToBeRetrieved);
            //Save the results to a CSV file
            using (StreamWriter file = new StreamWriter("Customers.csv"))
            {
                //Add headers to the file
                file.WriteLine(
                "CustomerID;CustomerName;CustomerClass;Email;Phone1;" +
                "AddressLine1;AddressLine2;City;State;PostalCode;");
                //Write the values for each item
                foreach (Customer customer in customers)
                {
                    file.WriteLine(
                    string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};",
                    customer.CustomerID.Value,
                    customer.CustomerName.Value,
                    customer.CustomerClass.Value,
                    customer.MainContact.Email.Value,
                    customer.MainContact.Phone1.Value,
                    customer.MainContact.Address.AddressLine1.Value,
                    customer.MainContact.Address.AddressLine2.Value,
                    customer.MainContact.Address.City.Value,
                    customer.MainContact.Address.State.Value,
                    customer.MainContact.Address.PostalCode.Value));
                }                
            }
        }
    }
}
