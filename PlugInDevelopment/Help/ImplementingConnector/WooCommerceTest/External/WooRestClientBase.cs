using PX.Commerce.BigCommerce;
using PX.Commerce.Core;
using PX.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Commerce.Core.REST;
using Polly;
using PX.Commerce.BigCommerce.API.REST;
using System.Net.Http;

namespace WooCommerceTest
{
    public abstract class WooRestClientBase :
        RetryCapableRESTClientBase<WooRestClientBase.HttpCallContext>
    {
        protected WooRestClientBase(int attempts) : base(attempts)
        {
        }

        protected override ValueTask<bool> IsFailure(Outcome<HttpCallContext> result,
            int attemptNumber, int attempts) =>
            new((result.Exception is not null
                || (int)result.Result.Response.StatusCode is 429)
                && attempts >= attemptNumber);

        protected override ValueTask HandleError(HttpCallContext ctx,
            int attemptNumber, int attempts)
        {
            if (attempts == attemptNumber)
                throw new PXException(BCMessages.RetryLimitIsExceeded);

            return new ValueTask();
        }

        protected override ValueTask<TimeSpan?> GetDelay(HttpCallContext context,
            int attemptNumber)
        {
            if (context is null)
                return new(TimeSpan.FromSeconds(attemptNumber + 1));

            var response = context.Response;
            if ((int)response.StatusCode == 429)
            {
                if ((response.Headers.TryGetValues(
                    BigCommerceConstants.Headers.RateLimitResetMs, out var values)
                    || response.Headers.TryGetValues(
                        BigCommerceConstants.Headers.RateLimitWindowMs, out values))
                    && int.TryParse(values.FirstOrDefault(), out var delay))
                    return new ValueTask<TimeSpan?>(TimeSpan.FromMilliseconds(delay));

                return new ValueTask<TimeSpan?>(TimeSpan.FromSeconds(attemptNumber + 1));
            }

            return new ValueTask<TimeSpan?>((TimeSpan?)null);
        }

        public class HttpCallContext(IBCRestResponse Response, HttpRequestMessage Request, string Body)
        {
            public IBCRestResponse Response { get; } = Response;
            public HttpRequestMessage Request { get; } = Request;
            public string Body { get; } = Body;
        }
    }
}
