<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="RS401000.aspx.cs" Inherits="Page_RS401000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
  <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="PhoneRepairShop.RSSVPaymentPlanInq"
        PrimaryView="Filter"
        >
    <CallbackCommands>

    </CallbackCommands>
  </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
  <px:PXFormView runat="server" ID="CstFormView1" DataSourceID="ds" 
      DataMember="Filter" Width="100%" >
    <Template>
      <px:PXLayoutRule LabelsWidth="XM" runat="server" ID="CstPXLayoutRule3" 
        StartColumn="True" ></px:PXLayoutRule>
      <px:PXSegmentMask CommitChanges="True" runat="server" ID="CstPXSegmentMask1" 
        DataField="CustomerID" ></px:PXSegmentMask>
      <px:PXSelector CommitChanges="True" runat="server" ID="CstPXSelector2" 
        DataField="ServiceID" ></px:PXSelector>
    </Template>
  </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
  <px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Details" AllowAutoHide="false">
    <Levels>
      <px:PXGridLevel DataMember="DetailsView">
          <Columns>
              <px:PXGridColumn DataField="OrderNbr" Width="72" />
              <px:PXGridColumn DataField="Status" Width="140" />
              <px:PXGridColumn DataField="InvoiceNbr" Width="72" />
              <px:PXGridColumn DataField="PercentPaid" Width="72" />
              <px:PXGridColumn DataField="ARInvoice__DueDate" Width="72" />
              <px:PXGridColumn DataField="ARInvoice__CuryDocBal" Width="100" />
          </Columns>
      </px:PXGridLevel>
    </Levels>
    <AutoSize Container="Window" Enabled="True" MinHeight="150" />
    <ActionBar >
    </ActionBar>
  </px:PXGrid>
</asp:Content>
