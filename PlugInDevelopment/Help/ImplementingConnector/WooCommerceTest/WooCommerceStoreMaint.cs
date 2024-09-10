using PX.Commerce.Core;
using PX.Commerce.Objects;
using PX.Data;
using System;
using System.Collections;
using PX.Data.BQL.Fluent;

namespace WooCommerceTest
{
    public class WooCommerceStoreMaint : BCStoreMaint
    {

        public SelectFrom<BCBindingWooCommerce>.
            Where<BCBindingWooCommerce.bindingID.
                IsEqual<BCBinding.bindingID.FromCurrent>>.View CurrentBindingWooCommerce;

        public WooCommerceStoreMaint()
        {
            base.Bindings.WhereAnd<Where<BCBinding.connectorType.IsEqual<WooCommerceConnector.WCConnectorType>>>();
        }

        #region Actions
        public PXAction<BCBinding> TestConnection;
        [PXButton]
        [PXUIField(DisplayName = "Test Connection", Enabled = false)]
        protected virtual IEnumerable testConnection(PXAdapter adapter)
        {
            Actions.PressSave();

            BCBinding binding = Bindings.Current;
            BCBindingWooCommerce bindingWooCommerce = CurrentBindingWooCommerce.Current ?? CurrentBindingWooCommerce.Select();
            if (binding == null || bindingWooCommerce == null || bindingWooCommerce.StoreBaseUrl == null)
            {
                throw new PXException(BCMessages.TestConnectionFailedParameters);
            }

            PXLongOperation.StartOperation(this, delegate
            {
                throw new NotImplementedException();
            });

            return adapter.Get();
        }
        #endregion

        [PXMergeAttributes(Method = MergeMethod.Append)]
        [PXCustomizeBaseAttribute(typeof(BCConnectorsAttribute), "DefaultConnector", WooCommerceConnector.TYPE)]
        public virtual void _(Events.CacheAttached<BCBinding.connectorType> e) { }

        public override void _(Events.RowSelected<BCBinding> e)
        {
            base._(e);

            BCBinding row = e.Row as BCBinding;
            if (row == null) return;

            //Actions
            TestConnection.SetEnabled(row.BindingID > 0 && row.ConnectorType == WooCommerceConnector.TYPE);
        }

        public override void _(Events.RowInserted<BCBinding> e)
        {
            base._(e);

            bool dirty = CurrentBindingWooCommerce.Cache.IsDirty;
            CurrentBindingWooCommerce.Insert();
            CurrentBindingWooCommerce.Cache.IsDirty = dirty;
        }

        public override void _(Events.RowSelected<BCBindingExt> e)
        {
            base._(e);

            BCBindingExt row = e.Row as BCBindingExt;
            if (row == null) return;
            PXDefaultAttribute.SetPersistingCheck<BCBindingExt.refundAmountItemID>(e.Cache, row, PXPersistingCheck.Nothing);
        }

    }
}