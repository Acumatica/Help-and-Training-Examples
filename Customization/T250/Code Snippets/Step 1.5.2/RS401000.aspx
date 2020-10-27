<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormDetail.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="RS401000.aspx.cs" Inherits="Page_RS401000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormDetail.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="PhoneRepairShop.RSSVPaymentPlanInq"
        PageLoadBehavior="PopulateSavedValues"
        PrimaryView="Filter"
        >
		<CallbackCommands>
		  <px:PXDSCallbackCommand Visible="false" CommitChanges="true" 
			  Name="ViewOrder" DependOnGrid="grid">
		  </px:PXDSCallbackCommand>
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>

...

<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Details" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="DetailsView">
			<Columns>
				<px:PXGridColumn DataField="OrderNbr" Width="72" LinkCommand="ViewOrder"/>
				<px:PXGridColumn DataField="Status" Width="140" />
				<px:PXGridColumn DataField="InvoiceNbr" Width="72" CommitChanges = true/>
				<px:PXGridColumn DataField="PercentPaid" Width="72" />
				<px:PXGridColumn DataField="ARInvoice__DueDate" Width="72" />
				<px:PXGridColumn DataField="ARInvoice__CuryDocBal" Width="100" />
				<px:PXGridColumn DataField="OrderType" Width="70" />
			</Columns>
<RowTemplate>
  <px:PXSelector ID="edInvoiceNbr" runat="server"
      DataField="InvoiceNbr" Enabled="False" AllowEdit="True" />
</RowTemplate>
</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
		<ActionBar >
		</ActionBar>
	</px:PXGrid>
</asp:Content>