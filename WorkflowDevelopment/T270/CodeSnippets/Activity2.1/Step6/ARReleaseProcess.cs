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
    }
}
