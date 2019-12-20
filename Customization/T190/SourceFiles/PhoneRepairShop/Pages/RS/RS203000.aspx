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
	<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="RepairPrices" Width="100%" Height="100px" AllowAutoHide="false">
		<Template>
			<px:PXLayoutRule ID="PXLayoutRule1" runat="server" StartRow="True"></px:PXLayoutRule>
			<px:PXSelector runat="server" ID="CstPXSelector3" DataField="ServiceID" ></px:PXSelector>
			<px:PXSelector runat="server" ID="CstPXSelector1" DataField="DeviceID" ></px:PXSelector>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule4" StartColumn="True" />
			<px:PXNumberEdit runat="server" ID="CstPXNumberEdit2" DataField="Price" /></Template>
	</px:PXFormView>
</asp:Content>
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" AllowAutoHide="false">
		<Items>
			<px:PXTabItem Text="Repair Items">
				<Template>
					<px:PXGrid runat="server" ID="CstPXGrid5" SkinID="DetailsInTab" Width="100%">
						<Levels>
							<px:PXGridLevel DataMember="RepairItems" >
								<Columns>
									<px:PXGridColumn DataField="RepairItemType" Width="70" CommitChanges="true"></px:PXGridColumn>
									<px:PXGridColumn Type="CheckBox" DataField="Required" Width="80" CommitChanges="true"></px:PXGridColumn>
									<px:PXGridColumn DataField="InventoryID" Width="70" CommitChanges="true"></px:PXGridColumn>
									<px:PXGridColumn DataField="InventoryID_description" Width="280" ></px:PXGridColumn>
									<px:PXGridColumn DataField="BasePrice" Width="100" ></px:PXGridColumn>
									<px:PXGridColumn Type="CheckBox" DataField="IsDefault" Width="80" CommitChanges="true"></px:PXGridColumn></Columns>

							
								<RowTemplate>
									<px:PXSegmentMask runat="server" ID="CstPXSegmentMask7" DataField="InventoryID" AutoRefresh="True" ></px:PXSegmentMask></RowTemplate></px:PXGridLevel></Levels>
						<AutoSize Enabled="True" ></AutoSize></px:PXGrid></Template>
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
						<AutoSize Enabled="True" ></AutoSize></px:PXGrid></Template>
			</px:PXTabItem>
			<px:PXTabItem Text="Warranty">
				<Template>
					<px:PXGrid runat="server" ID="CstPXGrid9" Width="100%" SkinID="DetailsInTab">
						<Levels>
							<px:PXGridLevel DataMember="Warranty" >
								<Columns>
									<px:PXGridColumn CommitChanges="True" DataField="ContractID" Width="70" ></px:PXGridColumn>
									<px:PXGridColumn DataField="ContractTemplate__Description" Width="220" ></px:PXGridColumn>
									<px:PXGridColumn DataField="ContractTemplate__Duration" Width="70" ></px:PXGridColumn>
									<px:PXGridColumn DataField="ContractTemplate__DurationType" Width="70" ></px:PXGridColumn>
									<px:PXGridColumn DataField="ContractTemplate__Type" Width="70" ></px:PXGridColumn></Columns></px:PXGridLevel></Levels>
						<AutoSize Enabled="True" ></AutoSize></px:PXGrid></Template>
				</px:PXTabItem></Items>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
	</px:PXTab>
</asp:Content>
