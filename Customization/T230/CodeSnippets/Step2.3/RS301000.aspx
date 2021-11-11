<%@ Page Language="C#" MasterPageFile="~/MasterPages/FormTab.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="RS301000.aspx.cs" Inherits="Page_RS301000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/FormTab.master" %>

...

<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
	<px:PXTab ID="tab" runat="server" Width="100%" Height="150px" DataSourceID="ds" AllowAutoHide="false">
		<Items>
...
			<px:PXTabItem Text="Labor">
				<Template>
					<px:PXGrid SkinID="Details" Width="100%" runat="server" ID="CstPXGrid2">
						<Levels>
							<px:PXGridLevel DataMember="Labor" >
								<Columns>
									<px:PXGridColumn DataField="InventoryID" Width="70" />
									<px:PXGridColumn DataField="InventoryID_description" Width="280" />
									<px:PXGridColumn DataField="DefaultPrice" Width="100" />
									<px:PXGridColumn CommitChanges="True" DataField="Quantity" Width="100" ></px:PXGridColumn>
									<px:PXGridColumn DataField="ExtPrice" Width="100" /></Columns></px:PXGridLevel></Levels>
						<ActionBar>
							<CustomItems>
								<px:PXToolBarButton Text="UpdateLaborPrices">
									<AutoCallBack Command="UpdateLaborPrices" Target="ds" />
								</px:PXToolBarButton>
							</CustomItems>
						</ActionBar>
						<AutoSize Enabled="True" ></AutoSize></px:PXGrid></Template>
			</px:PXTabItem>
		</Items>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" />
	</px:PXTab>
</asp:Content>