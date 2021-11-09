<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="WorkOrders"
  Width="100%" Height="" AllowAutoHide="false">
  <Template>
    <px:PXLayoutRule ControlSize="SM" LabelsWidth="S" ID="PXLayoutRule1"
                 runat="server" StartRow="True"></px:PXLayoutRule>
    <px:PXSelector runat="server" ID="CstPXSelector11" DataField="OrderNbr" ></px:PXSelector>
    <px:PXDropDown runat="server" ID="CstPXDropDown20" DataField="Status" ></px:PXDropDown>
    <px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit6" DataField="DateCreated" ></px:PXDateTimeEdit>
    <px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit5" DataField="DateCompleted" ></px:PXDateTimeEdit>
    <px:PXDropDown runat="server" ID="CstPXDropDown13" DataField="Priority" ></px:PXDropDown>
    <px:PXLayoutRule runat="server" ID="CstPXLayoutRule16"
                 StartColumn="True" ControlSize="XM" LabelsWidth="S" ></px:PXLayoutRule>
    <px:PXSegmentMask CommitChanges="True" runat="server" ID="CstPXSegmentMask4" DataField="CustomerID" ></px:PXSegmentMask>
    <px:PXSelector runat="server" ID="CstPXSelector14" DataField="ServiceID" ></px:PXSelector>
    <px:PXSelector runat="server" ID="CstPXSelector8" DataField="DeviceID" ></px:PXSelector>
    <px:PXSelector runat="server" ID="CstPXSelector3" DataField="Assignee" ></px:PXSelector>
    <px:PXLayoutRule runat="server" ID="CstLayoutRule18" ColumnSpan="2" ></px:PXLayoutRule>
    <px:PXTextEdit runat="server" ID="CstPXTextEdit7" DataField="Description" ></px:PXTextEdit>
    <px:PXLayoutRule runat="server" ID="CstPXLayoutRule17"
                 StartColumn="True" ControlSize="M" LabelsWidth="S" ></px:PXLayoutRule>
    <px:PXNumberEdit runat="server" ID="CstPXNumberEdit12" DataField="OrderTotal" ></px:PXNumberEdit>
    <px:PXTextEdit runat="server" ID="CstPXTextEdit10" DataField="InvoiceNbr" ></px:PXTextEdit>
  </Template>
</px:PXFormView>