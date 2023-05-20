using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Authenticators.OAuth;

namespace WooCommerceTest
{
    public class Autentificator : IAuthenticator
    {
        private readonly string _consumerKey;
        private readonly string _consumerSecret;


        public Autentificator(string consumerKey, string consumerSecret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
        }

        public void Authenticate(IRestClient client, IRestRequest request)
        {
            request.BuildOAuth1QueryString((RestClient)client, _consumerKey, _consumerSecret);
        }
    }

    public static class RestRequestExtensions
    {
        public static IRestRequest BuildOAuth1QueryString(this IRestRequest request, RestClient client, string consumerKey, string consumerSecret)
        {
            var auth = OAuth1Authenticator.ForRequestToken(consumerKey, consumerSecret);
            auth.ParameterHandling = OAuthParameterHandling.UrlOrPostParameters;
            auth.Authenticate(client, request);

            //convert all these oauth params from cookie to querystring
            request.Parameters.ForEach(x =>
            {
                if (x.Name.StartsWith("oauth_"))
                    x.Type = ParameterType.QueryString;
            });

            return request;
        }
    }
}
