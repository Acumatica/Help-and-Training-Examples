using PX.Data.WorkflowAPI;
using PhoneRepairShop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using PX.Data.BQL;
using PX.Data;
using PX.Objects.CS;
using System.CodeDom;
using PX.Data.BQL.Fluent;
using static PX.Data.WorkflowAPI.BoundedTo<PhoneRepairShop.RSSVWorkOrderEntry, PhoneRepairShop.RSSVWorkOrder>;
using PX.Objects.AR;
using PX.Objects.Common;

namespace PhoneRepairShop.Workflows
{
	// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
	public class RSSVWorkOrderWorkflow : PX.Data.PXGraphExtension<RSSVWorkOrderEntry>
	{
		...

		public override void Configure(PXScreenConfiguration config)
		{
			...
			context.AddScreenConfigurationFor(screen =>
				screen
					.StateIdentifierIs<RSSVWorkOrder.status>()
					.AddDefaultFlow(...)
					.WithActions(actions =>
					{
						...
						actions.Add(g => g.Assign, 
							c => c.WithCategory(
								processingCategory, g => g.PutOnHold)
							.MassProcessingScreen<RSSVAssignProcess>()
							.InBatchMode());
						...
					})
					.WithHandlers(...)
					.WithCategories(...)
			);
		}
	}
}
