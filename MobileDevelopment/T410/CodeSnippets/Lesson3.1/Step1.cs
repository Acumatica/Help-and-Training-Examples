using PX.Data;


namespace PX.Objects.SO
{
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