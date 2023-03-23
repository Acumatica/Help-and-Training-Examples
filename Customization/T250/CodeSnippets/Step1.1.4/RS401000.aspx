<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="RS401000.aspx.cs" Inherits="Page_RS401000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
  <px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="PhoneRepairShop.RSSVPaymentPlanInq"
        PrimaryView="DetailsView"
        >
    <CallbackCommands>

    </CallbackCommands>
  </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
  <px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Details" AllowAutoHide="false">
    <Levels>
      <%-- The added code --%>
      <px:PXGridLevel DataMember="DetailsView">
          <Columns>
              <px:PXGridColumn DataField="OrderNbr" />
              <px:PXGridColumn DataField="Status" />
              <px:PXGridColumn DataField="InvoiceNbr" />
              <px:PXGridColumn DataField="PercentPaid" />
              <px:PXGridColumn DataField="ARInvoice__DueDate" />
              <px:PXGridColumn DataField="ARInvoice__CuryDocBal" />
          </Columns>
      </px:PXGridLevel>
      <%-- The end of added code --%>
    </Levels>
    <AutoSize Container="Window" Enabled="True" MinHeight="150" />
    <ActionBar >
    </ActionBar>
  </px:PXGrid>
</asp:Content>
