<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="RS301000.aspx.cs" Inherits="Page_RS301000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="PhoneRepairShop.RSSVWorkOrderEntry"
        PrimaryView="WorkOrders"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="WorkOrders" Width="100%" Height="" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXSelector runat="server" ID="CstPXSelector10" DataField="OrderNbr" />
			<px:PXDropDown runat="server" ID="CstPXDropDown14" DataField="Status" />
			<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit6" DataField="DateCreated" />
			<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit5" DataField="DateCompleted" />
			<px:PXDropDown runat="server" ID="CstPXDropDown12" DataField="Priority" />
			<px:PXSegmentMask runat="server" ID="CstPXSegmentMask4" DataField="CustomerID" />
			<px:PXSelector runat="server" ID="CstPXSelector13" DataField="ServiceID" />
			<px:PXSelector runat="server" ID="CstPXSelector8" DataField="DeviceID" />
			<px:PXSelector runat="server" ID="CstPXSelector3" DataField="Assignee" />
			<px:PXTextEdit runat="server" ID="CstPXTextEdit7" DataField="Description" />
			<px:PXNumberEdit runat="server" ID="CstPXNumberEdit11" DataField="OrderTotal" />
			<px:PXTextEdit runat="server" ID="CstPXTextEdit9" DataField="InvoiceNbr" /></Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" AllowAutoHide="false">
		<Items>
			<px:PXTabItem Text="Repair Items">
				<Template>
					<px:PXGrid SyncPosition="True" Width="100%" runat="server" ID="CstPXGrid1" SkinID="Details">
						<Levels>
							<px:PXGridLevel DataMember="RepairItems" >
								<Columns>
									<px:PXGridColumn CommitChanges="True" DataField="RepairItemType" Width="70" ></px:PXGridColumn>
									<px:PXGridColumn CommitChanges="True" DataField="InventoryID" Width="70" ></px:PXGridColumn>
									<px:PXGridColumn DataField="InventoryID_description" Width="280" ></px:PXGridColumn>
									<px:PXGridColumn CommitChanges="True" DataField="BasePrice" Width="100" ></px:PXGridColumn></Columns>
								<RowTemplate>
									<px:PXSegmentMask runat="server" ID="CstPXSegmentMask15" DataField="InventoryID" AutoRefresh="True" ></px:PXSegmentMask></RowTemplate></px:PXGridLevel></Levels>
						<AutoSize Enabled="True" ></AutoSize>
						<Mode InitNewRow="True" ></Mode></px:PXGrid></Template>
			</px:PXTabItem>
			<px:PXTabItem Text="Labor">
				<Template>
					<px:PXGrid Width="100%" SkinID="Details" runat="server" ID="CstPXGrid2">
						<Levels>
							<px:PXGridLevel DataMember="Labor" >
								<Columns>
									<px:PXGridColumn CommitChanges="True" DataField="InventoryID" Width="70" ></px:PXGridColumn>
									<px:PXGridColumn DataField="InventoryID_description" Width="280" ></px:PXGridColumn>
									<px:PXGridColumn CommitChanges="True" DataField="DefaultPrice" Width="100" ></px:PXGridColumn>
									<px:PXGridColumn CommitChanges="True" DataField="Quantity" Width="100" ></px:PXGridColumn>
									<px:PXGridColumn DataField="ExtPrice" Width="100" ></px:PXGridColumn></Columns></px:PXGridLevel></Levels>
						<AutoSize Enabled="True" ></AutoSize>
						<Mode InitNewRow="True" ></Mode></px:PXGrid></Template>
			</px:PXTabItem>
		</Items>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
	</px:PXTab>
</asp:Content>
