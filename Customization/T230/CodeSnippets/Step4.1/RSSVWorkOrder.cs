using PX.Data.BQL.Fluent;
using PX.Objects.SO;

...
[PXSelector(typeof(SearchFor<SOInvoice.refNbr>.
    Where<SOInvoice.docType.IsEqual<ARDocType.invoice>>))]