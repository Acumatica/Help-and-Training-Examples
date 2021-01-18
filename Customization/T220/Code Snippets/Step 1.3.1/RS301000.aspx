<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="RS301000.aspx.cs" Inherits="Page_RS301000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%" TypeName="PhoneRepairShop.RSSVWorkOrderEntry" PrimaryView="WorkOrders">
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="WorkOrders" Width="100%" Height=" " AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule15" StartRow="True" ControlSize="SM" LabelsWidth="S"></px:PXLayoutRule>
			<px:PXSelector runat="server" ID="CstPXSelector10" DataField="OrderNbr" ></px:PXSelector>
			<px:PXDropDown runat="server" ID="CstPXDropDown14" DataField="Status" ></px:PXDropDown>
			<px:PXCheckBox runat="server" CommitChanges="true" ID="CstPXCheckBox18" DataField="Hold" />
			<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit6" DataField="DateCreated" ></px:PXDateTimeEdit>
			<px:PXDateTimeEdit runat="server" ID="CstPXDateTimeEdit5" DataField="DateCompleted" ></px:PXDateTimeEdit>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule18" ColumnSpan="3" ></px:PXLayoutRule>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit7" DataField="Description" ></px:PXTextEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule17" StartColumn="True" ControlSize="XM" LabelsWidth="S"></px:PXLayoutRule>
			<px:PXSegmentMask runat="server" ID="CstPXSegmentMask4" DataField="CustomerID" ></px:PXSegmentMask>
			<px:PXSelector CommitChanges="true" runat="server" ID="CstPXSelector13" DataField="ServiceID" ></px:PXSelector>
			<px:PXSelector CommitChanges="true" runat="server" ID="CstPXSelector8" DataField="DeviceID" ></px:PXSelector>
			<px:PXSelector runat="server" ID="CstPXSelector3" DataField="Assignee" ></px:PXSelector>
			<px:PXDropDown runat="server" ID="CstPXDropDown12" DataField="Priority" ></px:PXDropDown>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule19" StartColumn="True" ControlSize="M" LabelsWidth="S"/>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit9" DataField="InvoiceNbr" ></px:PXTextEdit>
			<px:PXNumberEdit runat="server" ID="CstPXNumberEdit11" DataField="OrderTotal" ></px:PXNumberEdit></Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" AllowAutoHide="false">
		<Items>
			<px:PXTabItem Text="Repair Items">
				<Template>
					<px:PXGrid SyncPosition="True" runat="server" ID="CstPXGrid1" SkinID="Details" Width="100%">
						<Levels>
							<px:PXGridLevel DataMember="RepairItems" >
								<RowTemplate>
									<px:PXLayoutRule ID="PXLayoutRule2" runat="server" StartRow="True"></px:PXLayoutRule>
									<px:PXLayoutRule runat="server" ID="CstLayoutRule20" StartGroup="True" GroupCaption="Repair Item" ControlSize="M" LabelsWidth="SM"></px:PXLayoutRule>
									<px:PXDropDown runat="server" ID="CstPXDropDown24" DataField="RepairItemType" />
									<px:PXSegmentMask runat="server" ID="CstPXSegmentMask22" DataField="InventoryID" />
									<px:PXTextEdit runat="server" ID="CstPXTextEdit23" DataField="InventoryID_description" />
									<px:PXLayoutRule runat="server" ID="PXLayoutRule1" StartGroup="True" GroupCaption="Price Info" LabelsWidth="S" StartColumn = "True"></px:PXLayoutRule>
									<px:PXNumberEdit runat="server" ID="CstPXNumberEdit21" DataField="BasePrice" ></px:PXNumberEdit>
									</RowTemplate></px:PXGridLevel></Levels>
						<AutoSize Enabled="True" /></px:PXGrid></Template>
			</px:PXTabItem>
			<px:PXTabItem Text="Labor">
				<Template>
					<px:PXGrid runat="server" ID="CstPXGrid2" SkinID="Details" Width="100%">
						<Levels>
							<px:PXGridLevel DataMember="Labor" /></Levels>
						<AutoSize Enabled="True"></AutoSize>
						<Mode AllowFormEdit="True" ></Mode></px:PXGrid></Template>
			</px:PXTabItem>
		</Items>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
	</px:PXTab>
</asp:Content>
