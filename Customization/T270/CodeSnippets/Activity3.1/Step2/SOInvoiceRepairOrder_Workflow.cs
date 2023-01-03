using PX.Data.WorkflowAPI;
using PX.Data;
using PX.Objects.SO;
using PX.Objects.AR;

namespace PhoneRepairShop
{
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
}
