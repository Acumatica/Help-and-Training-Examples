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
		public void PerformPersist(PXGraph.IPersistPerformer persister,
		            Action<PXGraph.IPersistPerformer> base_PerformPersist)
		{
			base_PerformPersist(persister);
			persister.Update<RSSVWorkOrder>();
		}
    }
}
