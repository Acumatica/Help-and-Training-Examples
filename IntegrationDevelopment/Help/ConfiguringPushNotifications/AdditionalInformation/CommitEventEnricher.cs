using PX.Common;
using PX.Data.PushNotifications;

public class CommitEventEnricher : ICommitEventEnricher
{
    public void Enrich(IQueueEvent commitEvent)
    {
        var businessDate = PXContext.PXIdentity?.BusinessDate;
        var userName = PXContext.PXIdentity?.IdentityName;
        commitEvent.AdditionalInfo.Add(nameof(businessDate), businessDate);
        commitEvent.AdditionalInfo.Add(nameof(userName), userName);
    }
}
