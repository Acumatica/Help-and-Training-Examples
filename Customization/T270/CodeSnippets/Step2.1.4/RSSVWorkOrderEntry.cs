using PX.Data.WorkflowAPI;

namespace PhoneRepairShop
{
    public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
    {
        #region Workflow Event Handlers 
        public PXWorkflowEventHandler<RSSVWorkOrder, ARInvoice> OnCloseDocument;
        #endregion
    }
}