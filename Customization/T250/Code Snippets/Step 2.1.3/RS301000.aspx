<px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" AllowAutoHide="false">
  <Items>
    ...
    <px:PXTabItem Text="Payment Info">
      <Template>
        <px:PXFormView DataMember="Payments" runat="server" ID="CstFormView27" >
          <Template>
            <px:PXTextEdit runat="server" ID="CstPXTextEdit31" DataField="InvoiceNbr" ></px:PXTextEdit>
            <px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit30" DataField="DueDate" ></px:PXDateTimeEdit>
            <px:PXTextEdit runat="server" ID="CstPXTextEdit28" DataField="AdjgRefNbr" ></px:PXTextEdit>
            <px:PXNumberEdit runat="server" ID="CstPXNumberEdit29" DataField="CuryAdjdAmt" ></px:PXNumberEdit></Template></px:PXFormView></Template></px:PXTabItem>
  </Items>
  <AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
</px:PXTab>