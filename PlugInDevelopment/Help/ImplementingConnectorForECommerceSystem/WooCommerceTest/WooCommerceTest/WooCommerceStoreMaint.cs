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
                SystemStatusProvider restClient = new SystemStatusProvider(WooCommerceConnector.GetRestClient(bindingWooCommerce));
                WooCommerceStoreMaint graph = PXGraph.CreateInstance<WooCommerceStoreMaint>();
                graph.Bindings.Current = binding;
                graph.CurrentBindingWooCommerce.Current = bindingWooCommerce;
                try
                {
                    var systemStatus = restClient.Get();

                    CurrentBindingWooCommerce.Current.WooCommerceDefaultCurrency = systemStatus.Settings.Currency;
                    CurrentBindingWooCommerce.Current.WooCommerceStoreTimeZone = systemStatus.Environment.DefaultTimezone;
                    Actions.PressSave();

                    if (systemStatus == null) throw new PXException(Messages.TestConnectionStoreNotFound);

                    graph.CurrentBindingWooCommerce.Cache.SetValueExt(binding, nameof(BCBindingWooCommerce.wooCommerceDefaultCurrency), systemStatus.Settings.Currency);
                    graph.CurrentBindingWooCommerce.Cache.SetValueExt(binding, nameof(BCBindingWooCommerce.wooCommerceStoreTimeZone), systemStatus.Environment.DefaultTimezone);
                    graph.CurrentBindingWooCommerce.Cache.IsDirty = true;
                    graph.CurrentBindingWooCommerce.Cache.Update(bindingWooCommerce);

                    graph.Persist();
                }
                catch (Exception ex)
                {
                    throw new PXException(ex, BCMessages.TestConnectionFailedGeneral, ex.Message);
                }
            });

            return adapter.Get();
        }
        #endregion

        protected virtual void _(Events.RowPersisting<BCBindingWooCommerce> e)
        {
            BCBindingWooCommerce row = e.Row as BCBindingWooCommerce;
            if (row == null || string.IsNullOrEmpty(row.StoreBaseUrl) || string.IsNullOrWhiteSpace(row.StoreXAuthClient) || string.IsNullOrWhiteSpace(row.StoreXAuthToken))
                return;

            SystemStatusProvider restClient = new SystemStatusProvider(WooCommerceConnector.GetRestClient(row));
            try
            {
                var store = restClient.Get();

                CurrentBindingWooCommerce.Cache.SetValueExt(row, nameof(row.WooCommerceDefaultCurrency), store.Settings?.Currency);
                CurrentBindingWooCommerce.Cache.SetValueExt(row, nameof(row.WooCommerceStoreTimeZone), store.Environment?.DefaultTimezone);
                CurrentBindingWooCommerce.Cache.IsDirty = true;
                CurrentBindingWooCommerce.Cache.Update(row);
            }
            catch (Exception) { }
        }

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