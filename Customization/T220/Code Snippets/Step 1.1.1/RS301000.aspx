<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="RS301000.aspx.cs" Inherits="Page_RS301000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="PhoneRepairShop.RSSVWorkOrderEntry" PrimaryView="WorkOrders">
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="WorkOrders" Width="100%" Height="100px" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule runat="server" ID="PXLayoutRule1" StartRow="True" /></Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" AllowAutoHide="false">
		<Items>
			<px:PXTabItem Text="Repair Items">
				<Template>
					<px:PXGrid runat="server" ID="CstPXGrid1" SkinID="Details" Width="100%">
						<Levels>
							<px:PXGridLevel DataMember="RepairItems" /></Levels>
						<AutoSize Enabled="True" /></px:PXGrid></Template>
			</px:PXTabItem>
			<px:PXTabItem Text="Labor">
				<Template>
					<px:PXGrid runat="server" ID="CstPXGrid2" SkinID="Details" Width="100%">
						<Levels>
							<px:PXGridLevel DataMember="Labor" /></Levels>
						<AutoSize Enabled="True" /></px:PXGrid></Template>
			</px:PXTabItem>
		</Items>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
	</px:PXTab>
</asp:Content>
