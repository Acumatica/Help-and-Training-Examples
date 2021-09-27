using System;
using System.Collections.Generic;
using System.Linq;
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
            Guid? noteId = message.Fields.First(v => v.Key.EndsWith("NoteID", StringComparison.InvariantCultureIgnoreCase) && v.Value != null).Value.ToGuid();
            Byte[] bytes = new Byte[16];
            BitConverter.GetBytes(WooCommerceConnector.TYPE.GetHashCode()).CopyTo(bytes, 0); //Connector
            BitConverter.GetBytes(message.Entity.GetHashCode()).CopyTo(bytes, 4); //EntityType
            BitConverter.GetBytes(message.Binding.GetHashCode()).CopyTo(bytes, 8); //Store
            BitConverter.GetBytes(noteId.GetHashCode()).CopyTo(bytes, 12); //ID
            return new Guid(bytes);
        }
        public List<Tuple<string, string, string>> GetExternalFields(string type, int? binding, string entity)
        {
            List<Tuple<string, string, string>> fieldsList = new List<Tuple<string, string, string>>();
            if (entity != BCEntitiesAttribute.Customer && entity != BCEntitiesAttribute.Address) return fieldsList;

            return fieldsList;
        }
    }
}
