using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyStoreIntegration.Default;
using MyStoreIntegration.Helper;

namespace MyStoreIntegration.Integration
{
    class CreditCardPayments
    {
        //Creating a credit card customer payment method  
        public static void CreateCustomerPaymentMethod(DefaultSoapClient soapClient)
        {
            Console.WriteLine("Creating a credit card customer payment method...");

            //Customer payment method data
            string customerID = "C000000003";
            string paymentMethod = "VISA";
            string paymentProfileID = "35596199"; //Payment Profile ID value, which was obtained from Authorize.Net
            string cashAccount = "102050MYST";
            string customerProfileID = "37100472";

            //Specify the parameters of the created customer payment method
            CustomerPaymentMethod paymMethToBeCreated = new CustomerPaymentMethod
            {
                CustomerID = new StringValue { Value = customerID },
                PaymentMethod = new StringValue { Value = paymentMethod },
                CashAccount = new StringValue { Value = cashAccount },
                CustomerProfileID = new StringValue { Value = customerProfileID },
                Details = new[]
                        {
                            new CustomerPaymentMethodDetail
                            {
                                Name = new StringValue{Value = "Payment Profile ID"},
                                Value = new StringValue{Value = paymentProfileID}
                            }
                        }
            };


            //Create the customer payment method
            CustomerPaymentMethod createdMethod = (CustomerPaymentMethod)soapClient.Put(paymMethToBeCreated);

            //Display the card number of the created payment method
            Console.WriteLine("Card number: " + createdMethod.CardAccountNbr.Value);
            Console.WriteLine();
            Console.WriteLine("Press any key to continue");
            Console.ReadLine();
        }
    }
}
