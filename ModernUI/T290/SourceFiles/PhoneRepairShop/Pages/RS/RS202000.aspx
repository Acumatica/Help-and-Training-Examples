<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormView.master"
AutoEventWireup="true" ValidateRequest="false" CodeFile="RS202000.aspx.cs"
Inherits="Page_RS202000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormView.master" %>
<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
    TypeName="PhoneRepairShop.RSSVDeviceMaint"
    PrimaryView="ServDevices"
>
<CallbackCommands>
</CallbackCommands>
</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
<px:PXFormView ID="form"
    runat="server" DataSourceID="ds" DataMember="ServDevices"
    Width="100%" AllowAutoHide="false">
<Template>
    <px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"
        ControlSize="M" LabelsWidth="S" />
    <px:PXSelector ID="DeviceCD" runat="server" DataField="DeviceCD" />
    <px:PXTextEdit ID="Description" runat="server" DataField="Description"
        DefaultLocale="" />
    <px:PXLayoutRule ID="PXLayoutRule2" runat="server" StartColumn="True" 
        ControlSize="M" LabelsWidth="S" />
    <px:PXCheckBox ID="Active" runat="server" DataField="Active" />
    <px:PXDropDown ID="AvgComplexityOfRepair" runat="server"
        DataField="AvgComplexityOfRepair" />
</Template>
<AutoSize Container="Window" Enabled="True" MinHeight="200" />
</px:PXFormView>
</asp:Content>