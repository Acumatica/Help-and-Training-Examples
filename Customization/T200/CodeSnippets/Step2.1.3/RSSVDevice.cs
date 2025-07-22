using PX.Data;
using PX.Data.BQL;
using System;

namespace PhoneRepairShop
{
	[PXCacheName(Messages.RSSVDevice)]
	public class RSSVDevice : PXBqlTable, IBqlTable
	{
		#region DeviceID
		public abstract class deviceID : BqlInt.Field<deviceID> { }

		[PXDBIdentity]
		public virtual int? DeviceID
		{
			get;
			set;
		}
		#endregion
	}
}
