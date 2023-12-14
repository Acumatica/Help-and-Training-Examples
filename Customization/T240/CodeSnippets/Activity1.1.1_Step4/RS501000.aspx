<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="RS501000.aspx.cs" Inherits="Page_RS501000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="PhoneRepairShop.RSSVAssignProcess"
        PrimaryView="WorkOrders"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
	<px:PXGrid AllowPaging="True" AdjustPageSize="Auto" ID="grid" runat="server" 
		DataSourceID="ds" Width="100%" Height="150px" SkinID="Inquire" 
		AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="WorkOrders">
			    <Columns>
			        <px:PXGridColumn Type="CheckBox" AllowCheckAll="True" 
			        	TextAlign="Center" DataField="Selected"></px:PXGridColumn>
					<px:PXGridColumn DataField="OrderNbr"></px:PXGridColumn>
					<px:PXGridColumn DataField="Description"></px:PXGridColumn>
					<px:PXGridColumn DataField="ServiceID"></px:PXGridColumn>
					<px:PXGridColumn DataField="DeviceID"></px:PXGridColumn>
					<px:PXGridColumn DataField="Priority"></px:PXGridColumn>
					<px:PXGridColumn DataField="Assignee"></px:PXGridColumn>
			    </Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
		<ActionBar >
		</ActionBar>
	</px:PXGrid>
</asp:Content>