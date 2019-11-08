using System;
using IdentityModel.Client;
using MyStoreIntegration.Default;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using System.Net;
using MyStoreIntegration.Integration;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace MyStoreIntegration
{
    class Program
    {
        static void Main(string[] args)
        {
            //This code is necessary only if you connect to the website
            //through the HTTPS connection and
            //you need to use custom validation of an SSL certificate
            //(for example, if the website uses a self-signed certificate).
            ServicePointManager.ServerCertificateValidationCallback += new
            RemoteCertificateValidationCallback(ValidateRemoteCertificate);

            //Discover the token endpoint
            DiscoveryClient discoveryClient = new DiscoveryClient(Properties.Settings.Default.IdentityEndpoint);
            DiscoveryResponse discoveryResponse = discoveryClient.GetAsync().Result;

            //Obtain and use the access token
            using (TokenClient tokenClient =
                new TokenClient(discoveryResponse.TokenEndpoint,
                Properties.Settings.Default.ClientID,
                Properties.Settings.Default.ClientSecret))
            {
                tokenClient.BasicAuthenticationHeaderStyle = BasicAuthenticationHeaderStyle.Rfc2617;
                var result = tokenClient
                .RequestResourceOwnerPasswordAsync(Properties.Settings.Default.Username, Properties.Settings.Default.Password, Properties.Settings.Default.Scope).Result;

                string accessToken = result.AccessToken;


                //Using the Default/18.200.001 endpoint
                using (DefaultSoapClient soapClient = new DefaultSoapClient())
                    try
                    {
                        soapClient.Endpoint.Behaviors.Add(new AccessTokenAdderBehavior(accessToken));
                        //PerformanceOptimization.ExportPayments(soapClient);

                        //Retrieving the list of sales orders of a customer
                        //PerformanceOptimization.ExportSalesOrders(soapClient);

                        //Retrieving the attached files
                        Attachments.ExportStockItemFiles(soapClient);
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
                        soapClient.Logout();
                    }
            }
            
        }

        //A supplementary class that adds the access token to each request to the service
        private class AccessTokenAdderBehavior : IEndpointBehavior, IClientMessageInspector
        {
            private readonly string _accessToken;

            public AccessTokenAdderBehavior(string accessToken)
            {
                _accessToken = accessToken;
            }

            void IEndpointBehavior.Validate(ServiceEndpoint endpoint) { }

            void IEndpointBehavior.AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters) { }

            void IEndpointBehavior.ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher) { }

            void IEndpointBehavior.ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime) => clientRuntime.ClientMessageInspectors.Add(this);

            object IClientMessageInspector.BeforeSendRequest(ref Message request, IClientChannel channel)
            {
                var httpRequestMessageProperty = new HttpRequestMessageProperty();
                httpRequestMessageProperty.Headers.Add(HttpRequestHeader.Authorization, "Bearer " + _accessToken);
                request.Properties.Remove(HttpRequestMessageProperty.Name);
                request.Properties.Add(HttpRequestMessageProperty.Name, httpRequestMessageProperty);
                return null;
            }

            void IClientMessageInspector.AfterReceiveReply(ref Message reply, object correlationState)
            {
            }
        }

        //Callback, which is used to validate the certificate of
        //an Acumatica ERP website in an SSL conversation
        private static bool ValidateRemoteCertificate(object sender,
        X509Certificate cert, X509Chain chain, SslPolicyErrors policyErrors)
        {
            //For simplicity, this callback always returns true.
            //In a real integration application, you must check an SSL
            //certificate here.
            return true;
        }
    }
}
