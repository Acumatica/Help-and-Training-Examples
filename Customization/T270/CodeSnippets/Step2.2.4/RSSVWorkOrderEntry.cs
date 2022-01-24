public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
{
    ...

    #region Workflow Event Handlers 
    ...
    //Event handler for a workflow event
    public PXWorkflowEventHandler<RSSVWorkOrder, ARRegister> OnInvoiceGotPrepaid;
    #endregion
}