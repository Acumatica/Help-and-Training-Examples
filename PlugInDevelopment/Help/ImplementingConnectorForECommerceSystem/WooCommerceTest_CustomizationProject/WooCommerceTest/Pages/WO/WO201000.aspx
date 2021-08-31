<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="WO201000.aspx.cs" Inherits="Page_WC201000" Title="Untitled Page" %>

<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" runat="Server">
    <px:PXDataSource PageLoadBehavior="GoFirstRecord" ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="WooCommerceTest.WooCommerceStoreMaint"
        PrimaryView="Bindings">
        <CallbackCommands>
        </CallbackCommands>
    </px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" runat="Server">
    <px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Bindings" Width="100%" Height="100px" AllowAutoHide="false">
        <Template>
            <px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
            <px:PXDropDown runat="server" ID="CstPXDropDown10" DataField="ConnectorType" />
            <px:PXSelector runat="server" ID="CstPXSelector9" DataField="BindingName" />
            <px:PXLayoutRule runat="server" ID="CstPXLayoutRule13" StartColumn="True" />
            <px:PXCheckBox runat="server" ID="CstPXCheckBox11" DataField="IsActive" />
            <px:PXCheckBox runat="server" ID="CstPXCheckBox12" DataField="IsDefault" />
        </Template>
    </px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" runat="Server">
    <px:PXTab DataMember="CurrentStore" ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" AllowAutoHide="false">
        <Items>
<px:PXTabItem Text="Connection Settings">
    <Template>
        <px:PXLayoutRule runat="server" ID="CstLayoutRule26" StartColumn="True"></px:PXLayoutRule>
        <px:PXFormView runat="server" ID="CstFormView14" DataMember="CurrentBindingWooCommerce">
            <Template>
                <px:PXLayoutRule runat="server" ID="CstLayoutRule23" ColumnWidth="XL" LabelsWidth="SM" StartRow="True" StartColumn="True"></px:PXLayoutRule>
                <px:PXTextEdit runat="server" ID="CstPXTextEdit15" DataField="StoreAdminUrl"></px:PXTextEdit>
                <px:PXLayoutRule GroupCaption="REST Settings" ColumnWidth="XL" LabelsWidth="SM" StartGroup="True" runat="server" ID="CstLayoutRule24"></px:PXLayoutRule>
                <px:PXTextEdit runat="server" ID="CstPXTextEdit16" DataField="StoreBaseUrl"></px:PXTextEdit>
                <px:PXTextEdit runat="server" ID="CstPXTextEdit17" DataField="StoreXAuthClient"></px:PXTextEdit>
                <px:PXTextEdit runat="server" ID="CstPXTextEdit18" DataField="StoreXAuthToken"></px:PXTextEdit>
                <px:PXLayoutRule runat="server" ID="CstLayoutRule25"></px:PXLayoutRule>
            </Template>
        </px:PXFormView>
        <px:PXLayoutRule GroupCaption="Store Properties" runat="server" ID="CstPXLayoutRule29" StartRow="True"></px:PXLayoutRule>
        <px:PXFormView runat="server" ID="CstFormView30" DataMember="CurrentBindingWooCommerce" RenderStyle="Simple">
            <Template>
                <px:PXTextEdit runat="server" ID="CstPXTextEdit31" DataField="WooCommerceDefaultCurrency"></px:PXTextEdit>
                <px:PXTextEdit runat="server" ID="CstPXTextEdit32" DataField="WooCommerceStoreTimeZone"></px:PXTextEdit>
            </Template>
        </px:PXFormView>
    </Template>
</px:PXTabItem>
            <px:PXTabItem Text="Entity Settings">
                <Template>
                    <px:PXGrid runat="server" ID="CstPXGrid42">
                        <Levels>
                            <px:PXGridLevel DataMember="Entities">
                                <Columns>
                                    <px:PXGridColumn Type="CheckBox" CommitChanges="True" DataField="IsActive" Width="60"></px:PXGridColumn>
                                    <px:PXGridColumn LinkCommand="Navigate" DataField="EntityType" Width="70"></px:PXGridColumn>
                                    <px:PXGridColumn CommitChanges="True" DataField="Direction" Width="70"></px:PXGridColumn>
                                    <px:PXGridColumn CommitChanges="True" DataField="PrimarySystem" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ImportRealTimeStatus" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="ExportRealTimeStatus" Width="120"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="RealTimeMode" Width="130"></px:PXGridColumn>
                                    <px:PXGridColumn DataField="MaxAttemptCount" Width="120"></px:PXGridColumn>
                                </Columns>
                            </px:PXGridLevel>
                        </Levels>
                        <ActionBar>
                            <Actions>
                                <AddNew ToolBarVisible="False"></AddNew>
                                <Delete ToolBarVisible="False"></Delete>
                                <ExportExcel ToolBarVisible="False"></ExportExcel>
                            </Actions>
                        </ActionBar>
                    </px:PXGrid>
                </Template>
            </px:PXTabItem>
            <px:PXTabItem Text="Customer Settings">
				<Template>
					<px:PXLayoutRule runat="server" StartGroup="False" ControlSize="M" LabelsWidth="M" StartColumn="True">
					</px:PXLayoutRule>
					<px:PXLayoutRule GroupCaption="Customer" runat="server" ID="CstPXLayoutRule79" StartGroup="True"></px:PXLayoutRule>
					<px:PXSelector AllowEdit="True" ID="edCustomerClassID" runat="server" DataField="CustomerClassID" CommitChanges="True">
					</px:PXSelector>
					<px:PXSelector runat="server" ID="CstPXSelector27" DataField="CustomerNumberingID" AllowEdit="True"></px:PXSelector>
				</Template>
				<ContentLayout ControlSize="XM" LabelsWidth="M"></ContentLayout>
			</px:PXTabItem>
        </Items>
        <AutoSize Container="Window" Enabled="True" MinHeight="150"></AutoSize>
    </px:PXTab>
</asp:Content>
