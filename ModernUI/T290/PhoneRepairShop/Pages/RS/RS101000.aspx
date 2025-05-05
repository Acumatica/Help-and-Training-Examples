<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="RS101000.aspx.cs" Inherits="Page_RS101000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="PhoneRepairShop.RSSVSetupMaint"
        PrimaryView="Setup"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Setup" Width="100%" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule LabelsWidth="SM" ControlSize="SM" ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXSelector AllowEdit="True" runat="server" ID="CstPXSelector2" DataField="NumberingID" ></px:PXSelector>
			<px:PXSegmentMask runat="server" ID="CstPXSegmentMask4" DataField="WalkInCustomerID" />
			<px:PXSelector runat="server" ID="CstPXSelector1" DataField="DefaultEmployee" />
			<px:PXNumberEdit runat="server" ID="CstPXNumberEdit3" DataField="PrepaymentPercent" /></Template>
		<AutoSize Container="Window" Enabled="True" MinHeight="200" ></AutoSize>
	</px:PXFormView>
</asp:Content>

