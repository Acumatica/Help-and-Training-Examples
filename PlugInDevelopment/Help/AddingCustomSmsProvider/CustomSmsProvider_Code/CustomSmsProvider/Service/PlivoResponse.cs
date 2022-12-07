using System.Net;

namespace PX.SmsProvider.Plivo
{
    public class PlivoResponse
    {
        /// <summary>
        /// HTTP status code
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Content string
        /// </summary>
        public string Content { get; }

        /// <summary>
        /// Create a new Response 
        /// </summary>
        /// <param name="statusCode">HTTP status code</param>
        /// <param name="content">Content string</param>
        public PlivoResponse(HttpStatusCode statusCode, string content)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
}