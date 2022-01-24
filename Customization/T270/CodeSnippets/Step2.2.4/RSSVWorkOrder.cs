using System;
using PX.Data;
using PX.Objects.AR;
using PX.TM;
using PX.Objects.CS;
using PX.Data.WorkflowAPI;

namespace PhoneRepairShop
{
    [PXCacheName("Repair Work Order")]
    public class RSSVWorkOrder : IBqlTable
    {
        public class MyEvents : PXEntityEvent<ARRegister>.Container<MyEvents>
        {
            public PXEntityEvent<ARRegister> InvoiceGotPrepaid;
        }

        ...
    }
}