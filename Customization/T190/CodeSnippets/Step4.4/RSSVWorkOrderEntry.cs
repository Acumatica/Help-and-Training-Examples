using PX.Data;
using PX.Data.BQL.Fluent;
using System.Linq;
using PX.Data.BQL;
using PX.Objects.SO;
using PX.Objects.AR;
using System.Collections;
using System.Collections.Generic;

namespace PhoneRepairShop
{
	// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod
	// extension should be constantly active
	public class RSSVWorkOrderEntry : PXGraph<RSSVWorkOrderEntry, RSSVWorkOrder>
	{
		...
		
		#region Actions
		 
		...
		
		public PXAction<RSSVWorkOrder> ActionsMenuItem;
		[PXButton(SpecialType = PXSpecialButtonType.ActionsFolder)]
		[PXUIField(DisplayName = "Actions")]
		protected virtual IEnumerable actionsMenuItem(PXAdapter adapter)
		{
			return adapter.Get();
		}

		#endregion
	}

	// Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod
	// extension should be constantly active
	public class RSSVWorkOrderEntry_Extension : PXGraphExtension<RSSVWorkOrderEntry>
	{
		public override void Initialize()
		{
			base.Initialize();
			Base.ActionsMenuItem.AddMenuAction(Base.CreateInvoiceAction);
		}
	}
}