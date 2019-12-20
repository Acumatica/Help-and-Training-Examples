<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="RS301000.aspx.cs" Inherits="Page_RS301000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="PhoneRepairShop.RSSVWorkOrderEntry"
        PrimaryView="WorkOrders"
        >
		<CallbackCommands>
            <px:PXDSCallbackCommand Visible="false" CommitChanges="true" Name="Complete" >
            </px:PXDSCallbackCommand>
		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="WorkOrders" Width="100%" Height="180px" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ID="PXLayoutRule1" runat="server" ControlSize="SM" LabelsWidth="S" StartRow="True"></px:PXLayoutRule>
			<px:PXSelector runat="server" ID="CstPXSelector10" DataField="OrderNbr" ></px:PXSelector>
			<px:PXDropDown runat="server" ID="CstPXDropDown14" DataField="Status" CommitChanges="true"></px:PXDropDown>
			<px:PXCheckBox CommitChanges="True" runat="server" ID="CstPXCheckBox9" DataField="Hold"></px:PXCheckBox>
			<px:PXDateTimeEdit Size="sm" runat="server" ID="CstPXDateTimeEdit6" DataField="DateCreated" ></px:PXDateTimeEdit>
			<px:PXDateTimeEdit Size="sm" runat="server" ID="CstPXDateTimeEdit5" DataField="DateCompleted" ></px:PXDateTimeEdit>
			<px:PXLayoutRule runat="server" ID="CstLayoutRule19" ColumnSpan="3" ></px:PXLayoutRule>
			<px:PXTextEdit runat="server" ID="CstPXTextEdit7" DataField="Description" ></px:PXTextEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule15" StartColumn="True" ControlSize="SM" LabelsWidth="S" ></px:PXLayoutRule>
			<px:PXSegmentMask runat="server" ID="CstPXSegmentMask4" DataField="CustomerID" ></px:PXSegmentMask>
			<px:PXSelector runat="server" ID="CstPXSelector13" DataField="ServiceID" CommitChanges="true"></px:PXSelector>
			<px:PXSelector runat="server" ID="CstPXSelector8" DataField="DeviceID" CommitChanges="true"></px:PXSelector>
			<px:PXSelector runat="server" ID="CstPXSelector3" DataField="Assignee" ></px:PXSelector>
			<px:PXDropDown runat="server" ID="CstPXDropDown12" DataField="Priority" ></px:PXDropDown>
			<px:PXLayoutRule ControlSize="SM" LabelsWidth="S" runat="server" ID="CstPXLayoutRule16" StartColumn="True" ></px:PXLayoutRule>
			<px:PXNumberEdit Size="SM" runat="server" ID="CstPXNumberEdit11" DataField="OrderTotal" ></px:PXNumberEdit>
			<px:PXSelector ID="edInvoiceNbr" runat="server" DataField="InvoiceNbr"
                AllowEdit="True" Enabled ="false" /></Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" AllowAutoHide="false">
		<Items>
			<px:PXTabItem Text="Repair Items">
				<Template>
					<px:PXGrid SyncPosition="True" runat="server" ID="CstPXGrid5" SkinID="DetailsInTab" Width="100%">
						<Levels>
							<px:PXGridLevel DataMember="RepairItems" >
								<Columns>
									<px:PXGridColumn DataField="RepairItemType" Width="70" CommitChanges="true"></px:PXGridColumn>
									<px:PXGridColumn DataField="InventoryID" Width="70" CommitChanges="true"></px:PXGridColumn>
									<px:PXGridColumn DataField="InventoryID_description" Width="280" ></px:PXGridColumn>
									<px:PXGridColumn DataField="BasePrice" Width="100" ></px:PXGridColumn>
									</Columns>

							
								<RowTemplate>
                                    <px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
									<px:PXLayoutRule LabelsWidth="SM" ControlSize="M" GroupCaption="Repair Item" runat="server" ID="CstPXLayoutRule20" StartGroup="True" ></px:PXLayoutRule>
									<px:PXDropDown runat="server" ID="CstPXDropDown18" DataField="RepairItemType" ></px:PXDropDown>
									<px:PXSegmentMask runat="server" ID="CstPXSegmentMask8" DataField="InventoryID" AutoRefresh="True" ></px:PXSegmentMask>
									<px:PXTextEdit runat="server" ID="CstPXTextEdit17" DataField="InventoryID_description" ></px:PXTextEdit>
			                        <px:PXLayoutRule GroupCaption="Price Info" LabelsWidth="S" runat="server" ID="CstPXLayoutRule14" StartColumn="True" ></px:PXLayoutRule>
									<px:PXNumberEdit runat="server" ID="CstPXNumberEdit16" DataField="BasePrice" ></px:PXNumberEdit>
																</RowTemplate></px:PXGridLevel></Levels>
						<AutoSize Enabled="True" ></AutoSize>
						<Mode AllowFormEdit="True" ></Mode></px:PXGrid></Template>
			</px:PXTabItem>
			<px:PXTabItem Text="Labor">
				<Template>
					<px:PXGrid Width="100%" runat="server" ID="CstPXGrid8" SkinID="DetailsInTab">
						<Levels>
							<px:PXGridLevel DataMember="Labor" >
								<Columns>
									<px:PXGridColumn DataField="InventoryID" Width="70" CommitChanges="True" ></px:PXGridColumn>
									<px:PXGridColumn DataField="InventoryID_description" Width="280" ></px:PXGridColumn>
									<px:PXGridColumn DataField="DefaultPrice" Width="100" ></px:PXGridColumn>
									<px:PXGridColumn DataField="Quantity" Width="100" CommitChanges="true" ></px:PXGridColumn>
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
		</Items>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
	</px:PXTab>
</asp:Content>
