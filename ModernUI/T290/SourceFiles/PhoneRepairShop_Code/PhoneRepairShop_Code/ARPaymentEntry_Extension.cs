using PX.Data;
using PX.Data.BQL.Fluent;
using PX.Objects.AR;

namespace PhoneRepairShop
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod
    // extension should be constantly active
    public class ARPaymentEntry_Extension : PXGraphExtension<ARPaymentEntry>
    {
        public virtual void _(Events.FieldDefaulting<ARPayment,
                      ARPaymentExt.usrPrepaymentPercent> e)
        {
            ARPayment payment = (ARPayment)e.Row;
            RSSVSetup setupRecord = SelectFrom<RSSVSetup>.View.Select(Base);
            if (setupRecord != null)
            {
                e.NewValue = setupRecord.PrepaymentPercent;
            }
        }
    }
}
