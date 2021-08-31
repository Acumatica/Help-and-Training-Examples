using System;
using System.Collections.Generic;
using PX.Commerce.Core;

namespace WooCommerceTest
{
    public class WooCommercesConnectorDescriptor : IConnectorDescriptor
    {
        protected IList<EntityInfo> _entities;

        public WooCommercesConnectorDescriptor(IList<EntityInfo> entities)
        {
            _entities = entities;
        }

		public virtual Guid? GenerateExternID(BCExternNotification message)
		{
			throw new NotImplementedException();
		}
		public virtual Guid? GenerateLocalID(BCLocalNotification message)
		{
			throw new NotImplementedException();
		}
        public List<Tuple<string, string, string>> GetExternalFields(string type, int? binding, string entity)
        {
            List<Tuple<string, string, string>> fieldsList = new List<Tuple<string, string, string>>();
            if (entity != BCEntitiesAttribute.Customer && entity != BCEntitiesAttribute.Address) return fieldsList;

            return fieldsList;
        }
    }
}
