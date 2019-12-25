using PhoneRepairShop;
using PX.Data;
using PX.Data.BQL.Fluent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PX.Objects.AR
{
    public class ARPaymentEntry_Extension : PXGraphExtension<ARPaymentEntry>
    {
        public virtual void _(Events.FieldDefaulting<ARPayment,
                      ARPaymentExt.usrPrepaymentPercent> e)
        {
            ARPayment payment = (ARPayment)e.Row;
            RSSVSetup setupRecord = SelectFrom<RSSVSetup>.View.Select(Base);
            e.NewValue = setupRecord.PrepaymentPercent;
        }
    }
}
