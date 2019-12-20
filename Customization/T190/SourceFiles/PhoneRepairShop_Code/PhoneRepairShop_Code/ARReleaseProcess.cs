using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PX.Data;
using PhoneRepairShop;
using PX.Data.BQL.Fluent;

namespace PX.Objects.AR
{
    /// <exclude/>
    public class ARReleaseProcess_Extension :
        PXGraphExtension<ARReleaseProcess>
    {
        public SelectFrom<RSSVWorkOrder>.View UpdWorkOrder;

        public delegate void CloseInvoiceAndClearBalancesDelegate(ARRegister ardoc,
            int? adjNbr);
        [PXOverride]
        public virtual void CloseInvoiceAndClearBalances(ARRegister ardoc, int?
        adjNbr,
        CloseInvoiceAndClearBalancesDelegate baseMethod)
        {
            RSSVWorkOrder order =
            SelectFrom<RSSVWorkOrder>.Where<RSSVWorkOrder.invoiceNbr.
            IsEqual<ARRegister.refNbr.FromCurrent>>.View.SelectSingleBound(Base, new[]
            { ardoc });
            if (order != null)
            {
                order.Status = WorkOrderStatusConstants.Paid;
                UpdWorkOrder.Update(order); // update cache
                                            // no need to call the Persist method
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

        public delegate void UpdateBalancesDelegate(ARAdjust adj, ARRegister adjddoc);
        [PXOverride]
        public virtual void UpdateBalances(ARAdjust adj,
        ARRegister adjddoc,
        UpdateBalancesDelegate baseMethod)
        {
            //baseMethod(adj, adjddoc);
            //ARRegister ardoc = (ARRegister)adjddoc;
            //ARRegister cached = (ARRegister)Base.ARDocument.Cache.Locate(ardoc);
            //if (cached != null)
            //{
            //    ardoc = cached;
            //}
            //var payment = SelectFrom<ARPayment>.Where<ARPayment.refNbr.IsEqual
            //<ARAdjust.adjgRefNbr.FromCurrent>>.View.
            //SelectSingleBound(Base, new[] { adjddoc });
            //var paymentExt = PXCache<ARPayment>.GetExtension<ARPaymentExt>(payment);
            //RSSVWorkOrder order = SelectFrom<RSSVWorkOrder>.
            //Where<RSSVWorkOrder.invoiceNbr.IsEqual<ARRegister.refNbr.FromCurrent>>.
            //View.SelectSingleBound(Base, new[] { adjddoc });
            //if (order != null && paymentExt != null && order.Status == WorkOrderStatusConstants.PendingPayment)
            //{
            //    var paidPercent = (ardoc.CuryOrigDocAmt - ardoc.CuryDocBal) * 100 /
            //    adjddoc.CuryOrigDocAmt;
            //    if (paidPercent >= paymentExt.UsrPrepaymentPercent)
            //    {
            //        order.Status = WorkOrderStatusConstants.ReadyForAssignment;
            //        UpdWorkOrder.Update(order);
            //        // no need to call the Persist method
            //    }
            //}
        }
    }
}
