using System;
using PX.Data;
using PhoneRepairShop.Workflows;
using System.Collections.Generic;

namespace PhoneRepairShop
{
    public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
    {
        public PXCancel<RSSVWorkOrder> Cancel;
        public PXProcessing<RSSVWorkOrder, 
            Where<RSSVWorkOrder.status.IsEqual<
            RSSVWorkOrderWorkflow.States.readyForAssignment>>> WorkOrders;

        public RSSVAssignProcess()
        {
            WorkOrders.SetProcessCaption("Assign");
            WorkOrders.SetProcessAllCaption("Assign All");
        }
        
        protected virtual void _(Events.RowSelected<RSSVWorkOrder> e) 
        {
            WorkOrders.SetProcessWorkflowAction<RSSVWorkOrderEntry>(
                g => g.Assign);
        }
        
        ...
    }
}