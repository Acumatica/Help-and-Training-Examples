﻿using PX.Data;
using PX.Objects.AR;
using System.Collections;
using PX.Objects.SO;

namespace PhoneRepairShop
{
    public class SOInvoiceEntry_Extension : PXGraphExtension<SOInvoiceEntry>
    {
        public PXAction<ARInvoice> ViewOrder;
        [PXButton, PXUIField(DisplayName = "View Repair Work Order")]
        protected virtual IEnumerable viewOrder(PXAdapter adapter)
        {
            var orderEntry = PXGraph.CreateInstance<RSSVWorkOrderEntry>();
            var order = orderEntry.WorkOrders.Search<RSSVWorkOrder.invoiceNbr>(
                Base.Document.Current.RefNbr);
            if (order == null)
                return adapter.Get();

            orderEntry.WorkOrders.Current = order;
            throw new PXRedirectRequiredException(orderEntry, true,
                nameof(ViewOrder))
            {
                Mode = PXBaseRedirectException.WindowMode.NewWindow
            };
        }
    }
}
