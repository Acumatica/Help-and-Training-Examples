using System;
using MyStoreIntegration.Default;
using System.Threading;
namespace MyStoreIntegration.Helper
{
    class LongRunProcessor
    {
        //A supplementary method for monitoring of the status of
        //long-running operations
        public static ProcessResult GetProcessResult(
        DefaultSoapClient soapClient, InvokeResult invokeResult)
        {
            while (true)
            {
                var processResult = soapClient.GetProcessStatus(invokeResult);
                switch (processResult.Status)
                {
                    case ProcessStatus.Aborted:
                        throw new SystemException("Process status: " +
                        processResult.Status + "; Error: " +
                        processResult.Message);
                    case ProcessStatus.NotExists:
                    case ProcessStatus.Completed:
                        //Go to normal processing.
                        return processResult;
                    case ProcessStatus.InProcess:
                        //Pause for 1 second.
                        Thread.Sleep(1000);
                        if (processResult.Seconds > 30)
                            throw new TimeoutException();
                        continue;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }
    }
}
