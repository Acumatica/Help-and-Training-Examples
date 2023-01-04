using System;
using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;

namespace PhoneRepairShop
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class ARReleaseProcess_Extension : PXGraphExtension<ARReleaseProcess>
    {
        public SelectFrom<RSSVWorkOrder>.View UpdWorkOrder;

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

        public delegate void UpdateBalancesDelegate(ARAdjust adj,
            ARRegister adjddoc, ARTran adjdtran);
        [PXOverride]
        public virtual void UpdateBalances(ARAdjust adj,
            ARRegister adjddoc, ARTran adjdtran,
            UpdateBalancesDelegate baseMethod)
        {
            baseMethod(adj, adjddoc, adjdtran);

            ARRegister ardoc = adjddoc;
            ARRegister cached = (ARRegister)Base.ARDocument.Cache.Locate(ardoc);
            if (cached != null)
            {
                ardoc = cached;
            }

            RSSVWorkOrder order = SelectFrom<RSSVWorkOrder>.
                Where<RSSVWorkOrder.invoiceNbr.
                IsEqual<ARRegister.refNbr.FromCurrent>>
                .View.SelectSingleBound(Base, new[] { ardoc });

            if (order != null &&
                order.Status == WorkOrderStatusConstants.PendingPayment)
            {
                var payment = SelectFrom<ARPayment>.
                    Where<ARPayment.docType.
                        IsEqual<ARAdjust.adjgDocType.FromCurrent>.
                        And<ARPayment.refNbr.
                        IsEqual<ARAdjust.adjgRefNbr.FromCurrent>>>
                    .View.SelectSingleBound(Base, new[] { ardoc });

                if (payment != null)
                {
                    var paidPercent = (ardoc.CuryOrigDocAmt - ardoc.CuryDocBal) * 100
                        / ardoc.CuryOrigDocAmt;
                    var paymentExt = PXCache<ARPayment>.
                        GetExtension<ARPaymentExt>(payment);
                    if (paidPercent >= paymentExt.UsrPrepaymentPercent)
                    {
                        RSSVWorkOrder.MyEvents
                          .Select(e => e.InvoiceGotPrepaid)
                          .FireOn(Base, ardoc);
                        // No need to call the Persist method.
                    }
                }
            }
        }
    }
}
