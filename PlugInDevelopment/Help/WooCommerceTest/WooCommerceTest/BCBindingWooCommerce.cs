using PX.Commerce.Core;
using PX.Data;
using PX.Data.ReferentialIntegrity.Attributes;

namespace WooCommerceTest
{
    [PXCacheName("WooCommerce Settings")]
    public class BCBindingWooCommerce : IBqlTable
    {
        public class PK : PrimaryKeyOf<BCBindingWooCommerce>.By<BCBindingWooCommerce.bindingID>
        {
            public static BCBindingWooCommerce Find(PXGraph graph, int? binding) => FindBy(graph, binding);
        }

        #region BindingID
        [PXDBInt(IsKey = true)]
        [PXDBDefault(typeof(BCBinding.bindingID))]
        [PXUIField(DisplayName = "Store", Visible = false)]
        [PXParent(typeof(Select<BCBinding, Where<BCBinding.bindingID, Equal<Current<BCBindingWooCommerce.bindingID>>>>))]
        public int? BindingID { get; set; }
        public abstract class bindingID : PX.Data.BQL.BqlInt.Field<bindingID> { }
        #endregion

        //Connection
        #region StoreBaseUrl
        [PXDBString(50, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "API Path")]
        [PXDefault()]
        public virtual string StoreBaseUrl { get; set; }
        public abstract class storeBaseUrl : PX.Data.BQL.BqlString.Field<storeBaseUrl> { }
        #endregion
        #region StoreXAuthClient
        [PXRSACryptString(IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Consumer Key")]
        [PXDefault()]
        public virtual string StoreXAuthClient { get; set; }
        public abstract class storeXAuthClient : PX.Data.BQL.BqlString.Field<storeXAuthClient> { }
        #endregion
        #region StoreXAuthToken
        [PXRSACryptString(IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Consumer Secret")]
        [PXDefault()]
        public virtual string StoreXAuthToken { get; set; }
        public abstract class storeXAuthToken : PX.Data.BQL.BqlString.Field<storeXAuthToken> { }
        #endregion

        #region WooCommerceDefaultCurrency
        [PXDBString(12, IsUnicode = true)]
        [PXUIField(DisplayName = "Default Currency", IsReadOnly = true)]
        public virtual string WooCommerceDefaultCurrency { get; set; }
        public abstract class wooCommerceDefaultCurrency : PX.Data.BQL.BqlString.Field<wooCommerceDefaultCurrency> { }
        #endregion

        #region WooCommerceStoreTimeZone 
        [PXDBString(100, IsUnicode = true)]
        [PXUIField(DisplayName = "Store Time Zone", IsReadOnly = true)]
        public virtual string WooCommerceStoreTimeZone { get; set; }
        public abstract class wooCommerceStoreTimeZone : PX.Data.BQL.BqlString.Field<wooCommerceStoreTimeZone> { }
        #endregion

        #region StoreAdminURL
        [PXDBString(100, IsUnicode = true, InputMask = "")]
        [PXUIField(DisplayName = "Store Admin Path")]
        [PXDefault()]
        public virtual string StoreAdminUrl { get; set; }
        public abstract class storeAdminUrl : PX.Data.BQL.BqlString.Field<storeAdminUrl> { }
        #endregion

    }
    /*
    [PXPrimaryGraph(new Type[] { typeof(WCStoreMaint) },
            new Type[] { typeof(Where<BCBinding.connectorType, Equal<WCConnectorType>>) })]

    public class BCBindingWooCommerceExtension : PXCacheExtension<BCBindingExt>
    {
        //public Int32? RefundAmountItemID { get; set; }
    }
    */
}
