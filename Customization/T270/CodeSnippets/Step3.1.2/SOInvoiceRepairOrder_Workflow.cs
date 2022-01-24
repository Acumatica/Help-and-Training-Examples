public class SOInvoiceOrder_Workflow : PXGraphExtension<SOInvoiceEntry_Workflow, 
    SOInvoiceEntry>
{
    public override void Configure(PXScreenConfiguration config)
    {
        Configure(config.GetScreenConfigurationContext<SOInvoiceEntry, ARInvoice>());
    }

    protected virtual void Configure(WorkflowContext<SOInvoiceEntry, 
        ARInvoice> context)
    {
    }
}