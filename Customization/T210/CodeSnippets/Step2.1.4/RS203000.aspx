<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="RS203000.aspx.cs" Inherits="Page_RS203000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="PhoneRepairShop.RSSVRepairPriceMaint"
        PrimaryView="RepairPrices"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="RepairPrices" Width="100%" Height="" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ControlSize="m" LabelsWidth="s" ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXSelector runat="server" ID="CstPXSelector3" DataField="ServiceID" />
			<px:PXSelector runat="server" ID="CstPXSelector1" DataField="DeviceID" />
			<px:PXLayoutRule ControlSize="m" LabelsWidth="sm" runat="server" ID="CstPXLayoutRule4" StartColumn="True" ></px:PXLayoutRule>
			<px:PXNumberEdit runat="server" ID="CstPXNumberEdit2" DataField="Price" /></Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" AllowAutoHide="false">
		<Items>
			<%-- The added code --%>
			<px:PXTabItem Text="Repair Items">
				<Template>
					<px:PXGrid Width="100%" SkinID="Details" runat="server" 
						ID="CstPXGrid5">
						<Levels>
							<px:PXGridLevel DataMember="RepairItems" >
								<Columns>
									<px:PXGridColumn DataField="RepairItemType" 
										Width="70" />
									<px:PXGridColumn Type="CheckBox" 
										DataField="Required" Width="80" >										
									</px:PXGridColumn>
									<px:PXGridColumn DataField="InventoryID" 
										Width="70" />
									<px:PXGridColumn 
										DataField="InventoryID_description" 
										Width="280" />
									<px:PXGridColumn DataField="BasePrice" 
										Width="100" />
									<px:PXGridColumn Type="CheckBox" 
										DataField="IsDefault" Width="80" >										
									</px:PXGridColumn>
								</Columns></px:PXGridLevel></Levels>
						<AutoSize Enabled="True" ></AutoSize>
						<Mode InitNewRow="True" /></px:PXGrid></Template>
			</px:PXTabItem>
			<%-- The end of added code --%>
			<px:PXTabItem Text="Tab item 2">
				<Template>
					
				</Template>
			</px:PXTabItem>
		</Items>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
	</px:PXTab>
</asp:Content>