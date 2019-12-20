using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Objects.AR;
using PX.Data;
using PhoneRepairShop;
using PX.Data.BQL.Fluent;

namespace PX.Objects.AR
{
    public class ARReleaseProcess_Extension : PXGraphExtension<ARReleaseProcess>
    {

        public PXSelect<RSSVWorkOrder> UpdWorkOrder;

        public delegate void CloseInvoiceAndClearBalancesDelegate(ARRegister ardoc, int? adjNbr);
        [PXOverride]
        public virtual void CloseInvoiceAndClearBalances(ARRegister ardoc, int? adjNbr, CloseInvoiceAndClearBalancesDelegate baseMethod)
        {
            RSSVWorkOrder order = SelectFrom<RSSVWorkOrder>.Where<RSSVWorkOrder.invoiceNbr.IsEqual<ARRegister.refNbr.FromCurrent>>.View.SelectSingleBound(Base, new[] { ardoc });
            if (order!=null)
            {
                order.Status = WorkOrderStatusConstants.Paid;
                UpdWorkOrder.Update(order);
                Base.Persist();
            }

            baseMethod(ardoc, adjNbr);
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
