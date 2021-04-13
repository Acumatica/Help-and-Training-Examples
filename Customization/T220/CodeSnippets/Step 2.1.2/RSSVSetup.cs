using System;
using PX.Data;

namespace PhoneRepairShop
{
	[PXCacheName("Repair Work Order Preferences")]
	[PXPrimaryGraph(typeof(RSSVSetupMaint))]
	public class RSSVSetup : IBqlTable
	{
		...
	}
}