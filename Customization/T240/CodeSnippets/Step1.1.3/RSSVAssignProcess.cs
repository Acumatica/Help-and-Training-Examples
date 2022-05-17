using System;
using PX.Data;
using PhoneRepairShop.Workflows;
using PX.Data.BQL.Fluent;
using System.Collections.Generic;

namespace PhoneRepairShop
{
    public class RSSVAssignProcess : PXGraph<RSSVAssignProcess>
    {
        public PXCancel<RSSVWorkOrder> Cancel;
        public SelectFrom<RSSVWorkOrder>.
            Where<RSSVWorkOrder.status.
                IsEqual<RSSVWorkOrderWorkflow.States.readyForAssignment>>.
            ProcessingView WorkOrders;

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