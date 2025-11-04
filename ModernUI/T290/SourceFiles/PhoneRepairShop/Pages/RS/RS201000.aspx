<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="RS201000.aspx.cs" Inherits="Page_RS201000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="PhoneRepairShop.RSSVRepairServiceMaint"
        PrimaryView="RepairService"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
	<px:PXGrid AutoAdjustColumns="True" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Primary" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="RepairService">
			    <Columns>
				<px:PXGridColumn DataField="ServiceCD" Width="140" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Description" Width="180" ></px:PXGridColumn>
				<px:PXGridColumn Type="CheckBox" DataField="Active" Width="60" ></px:PXGridColumn>
				<px:PXGridColumn CommitChanges="True" Type="CheckBox" DataField="WalkInService" Width="60" ></px:PXGridColumn>
				<px:PXGridColumn Type="CheckBox" DataField="Prepayment" Width="60" ></px:PXGridColumn>
				<px:PXGridColumn CommitChanges="True" Type="CheckBox" DataField="PreliminaryCheck" Width="60" ></px:PXGridColumn></Columns>
			</px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
		<ActionBar >
		</ActionBar>
	</px:PXGrid>
</asp:Content>
