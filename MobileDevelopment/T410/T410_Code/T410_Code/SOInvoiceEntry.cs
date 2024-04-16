using PX.Data;


namespace PX.Objects.SO
{
  // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
  public class SOInvoiceEntry_Extension : PXGraphExtension<PX.Objects.SO.SOInvoiceEntry>
  {
    
    public PXAction<PX.Objects.AR.ARInvoice> TestURL;
    [PXButton]
    [PXUIField(DisplayName = "Test URL")]
    protected void testURL()
    {
      throw new PXRedirectToUrlException("http://www.acumatica.com",
        "Redirect:http://www.acumatica.com");
    }
  }
}