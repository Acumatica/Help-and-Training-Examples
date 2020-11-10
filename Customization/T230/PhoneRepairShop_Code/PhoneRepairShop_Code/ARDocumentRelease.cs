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
                // Base.Persist(); // no need to call it, it is invoked at the end of the Release method
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
        public virtual void UpdateBalances(ARAdjust adj, ARRegister adjddoc, UpdateBalancesDelegate baseMethod)
        {
            baseMethod(adj, adjddoc);

            ARRegister ardoc = (ARRegister)adjddoc;
            ARRegister cached = (ARRegister)Base.ARDocument.Cache.Locate(ardoc);
            if (cached != null)
            {
                ardoc = cached;
            }

            RSSVSetup setupRecord = SelectFrom<RSSVSetup>.View.Select(Base);

            //ARInvoice invoice = SelectFrom<ARInvoice>.Where<ARInvoice.refNbr.IsEqual<ARRegister.refNbr.FromCurrent>>.View.SelectSingleBound(Base, new[] { adjddoc });
            // say that ARRegister has all needed fields of ARInvoice
            RSSVWorkOrder order = SelectFrom<RSSVWorkOrder>.Where<RSSVWorkOrder.invoiceNbr.IsEqual<ARRegister.refNbr.FromCurrent>>.View.SelectSingleBound(Base, new[] { adjddoc });
            if (order != null)
            {
                var paidPercent = (ardoc.CuryOrigDocAmt - ardoc.CuryDocBal) * 100 / adjddoc.CuryOrigDocAmt;
                if (paidPercent > setupRecord.PrepaymentPercent)
                {
                    order.Status = WorkOrderStatusConstants.ReadyForAssignment;
                    UpdWorkOrder.Update(order);
                    // Base.Persist(); // no need to call it, it is invoked at the end of the Release method
                }
            }  
        }

    }
}
