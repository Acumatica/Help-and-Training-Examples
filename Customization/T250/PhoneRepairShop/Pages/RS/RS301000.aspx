<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="RS301000.aspx.cs" Inherits="Page_RS301000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="PhoneRepairShop.RSSVWorkOrderEntry"
        PrimaryView="WorkOrders"
        >
		<CallbackCommands>
			<px:PXDSCallbackCommand Visible="false" CommitChanges="true" Name="Complete" ></px:PXDSCallbackCommand>
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="WorkOrders" Width="100%" Height="" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ControlSize="SM" LabelsWidth="S" ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXSelector runat="server" ID="CstPXSelector11" DataField="OrderNbr" ></px:PXSelector>
			<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown15" DataField="Status" ></px:PXDropDown>
			<px:PXCheckBox CommitChanges="True" runat="server" ID="CstPXCheckBox9" DataField="Hold" ></px:PXCheckBox>
			<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit6" DataField="DateCreated" ></px:PXDateTimeEdit>
			<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit5" DataField="DateCompleted" ></px:PXDateTimeEdit>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule18" ColumnSpan="3" ></px:PXLayoutRule>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit7" DataField="Description" ></px:PXTextEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule16" StartColumn="True" ControlSize="XM" LabelsWidth="S" ></px:PXLayoutRule>
			<px:PXSegmentMask CommitChanges="True" runat="server" ID="CstPXSegmentMask4" DataField="CustomerID" ></px:PXSegmentMask>
			<px:PXSelector CommitChanges="True" runat="server" ID="CstPXSelector14" DataField="ServiceID" ></px:PXSelector>
			<px:PXSelector CommitChanges="True" runat="server" ID="CstPXSelector8" DataField="DeviceID" ></px:PXSelector>
			<px:PXSelector runat="server" ID="CstPXSelector3" DataField="Assignee" ></px:PXSelector>
			<px:PXDropDown runat="server" ID="CstPXDropDown13" DataField="Priority" ></px:PXDropDown>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule17" StartColumn="True" ControlSize="M" LabelsWidth="S" ></px:PXLayoutRule>
			<px:PXNumberEdit runat="server" ID="CstPXNumberEdit12" DataField="OrderTotal" ></px:PXNumberEdit>
			<px:PXSelector ID="edInvoiceNbr" runat="server" DataField="InvoiceNbr" Enabled="False" AllowEdit="True" /></Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" AllowAutoHide="true">
		<Items>
			<px:PXTabItem Text="Repair Items">
				<Template>
					<px:PXGrid SyncPosition="True" SkinID="Details" Width="100%" runat="server" ID="CstPXGrid1">
						<Levels>
							<px:PXGridLevel DataMember="RepairItems" >
								<Columns>
									<px:PXGridColumn DataField="RepairItemType" Width="70" ></px:PXGridColumn>
									<px:PXGridColumn CommitChanges="True" DataField="InventoryID" Width="70" ></px:PXGridColumn>
									<px:PXGridColumn DataField="InventoryID_description" Width="280" ></px:PXGridColumn>
									<px:PXGridColumn DataField="BasePrice" Width="100" ></px:PXGridColumn></Columns>
								<RowTemplate>
									<px:PXLayoutRule runat="server" ID="CstPXLayoutRule19" StartRow="True" ></px:PXLayoutRule>
									<px:PXLayoutRule ControlSize="M" LabelsWidth="SM" runat="server" ID="CstPXLayoutRule26" StartGroup="True" GroupCaption="Repair Item" ></px:PXLayoutRule>
									<px:PXDropDown runat="server" ID="CstPXDropDown23" DataField="RepairItemType" ></px:PXDropDown>
									<px:PXSegmentMask runat="server" ID="CstPXSegmentMask21" DataField="InventoryID" ></px:PXSegmentMask>
									<px:PXTextEdit runat="server" ID="CstPXTextEdit22" DataField="InventoryID_description" ></px:PXTextEdit>
									<px:PXLayoutRule LabelsWidth="S" StartColumn="True" runat="server" ID="CstPXLayoutRule24" StartGroup="True" GroupCaption="Price Info" ></px:PXLayoutRule>
									<px:PXNumberEdit runat="server" ID="CstPXNumberEdit20" DataField="BasePrice" ></px:PXNumberEdit></RowTemplate></px:PXGridLevel></Levels>
						<AutoSize Enabled="True" ></AutoSize>
						<Mode AllowFormEdit="True" ></Mode></px:PXGrid></Template>
			</px:PXTabItem>
			<px:PXTabItem Text="Labor">
				<Template>
					<px:PXGrid SkinID="Details" Width="100%" runat="server" ID="CstPXGrid2">
						<Levels>
							<px:PXGridLevel DataMember="Labor" >
								<Columns>
									<px:PXGridColumn DataField="InventoryID" Width="70" ></px:PXGridColumn>
									<px:PXGridColumn DataField="InventoryID_description" Width="280" ></px:PXGridColumn>
									<px:PXGridColumn DataField="DefaultPrice" Width="100" ></px:PXGridColumn>
									<px:PXGridColumn CommitChanges="True" DataField="Quantity" Width="100" ></px:PXGridColumn>
									<px:PXGridColumn DataField="ExtPrice" Width="100" ></px:PXGridColumn></Columns></px:PXGridLevel></Levels>
						<ActionBar>
							<CustomItems>
								<px:PXToolBarButton Text="Complete">
									<AutoCallBack Command="Complete" Target="ds">
										<Behavior CommitChanges="True" ></Behavior>
									</AutoCallBack>
								</px:PXToolBarButton>
							</CustomItems>
						</ActionBar>
						<AutoSize Enabled="True" ></AutoSize></px:PXGrid></Template>
			</px:PXTabItem>
			<px:PXTabItem Text="Payment Info">
				<Template>
					<px:PXFormView DataMember="Payments" runat="server" ID="CstFormView27" >
						<Template>
							<px:PXTextEdit runat="server" ID="CstPXTextEdit31" DataField="InvoiceNbr" />
							<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit30" DataField="DueDate" />
							<px:PXTextEdit runat="server" ID="CstPXTextEdit28" DataField="AdjgRefNbr" />
							<px:PXNumberEdit runat="server" ID="CstPXNumberEdit29" DataField="CuryAdjdAmt" /></Template></px:PXFormView></Template></px:PXTabItem></Items>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
	</px:PXTab>
</asp:Content>