using System;
using PX.Data;
using PhoneRepairShop;
using PX.Data.BQL.Fluent;

namespace PX.Objects.AR
{
    public class ARReleaseProcess_Extension : PXGraphExtension<ARReleaseProcess>
    {
        public SelectFrom<RSSVWorkOrder>.View UpdWorkOrder;

        public delegate void CloseInvoiceAndClearBalancesDelegate(ARRegister ardoc);
        [PXOverride]
        public virtual void CloseInvoiceAndClearBalances(ARRegister ardoc,
                                                         CloseInvoiceAndClearBalancesDelegate baseMethod)
        {
            RSSVWorkOrder order = SelectFrom<RSSVWorkOrder>.Where<RSSVWorkOrder.invoiceNbr.
                                  IsEqual<ARRegister.refNbr.FromCurrent>>.View.SelectSingleBound(Base, new[] { ardoc });
            if (order != null)
            {
                order.Status = WorkOrderStatusConstants.Paid;
                UpdWorkOrder.Update(order);  // update cache
                                             // no need to call the Persist method
            }

            baseMethod(ardoc);
        }

        [PXOverride]
        public virtual void Persist(Action baseMethod)
        {
            using (PXTransactionScope ts = new PXTransactionScope())
            {
                baseMethod();
                UpdWorkOrder.Cache.Persist(PXDBOperation.Update);
                ts.Complete(Base);
            }
            UpdWorkOrder.Cache.Persisted(false);
        }

    }
}
