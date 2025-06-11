<%@ Page Language="C#" MasterPageFile="~/MasterPages/ListView.master" AutoEventWireup="true" ValidateRequest="false" CodeFile="RS501000.aspx.cs" Inherits="Page_RS501000" Title="Untitled Page" %>
<%@ MasterType VirtualPath="~/MasterPages/ListView.master" %>

<asp:Content ID="cont1" ContentPlaceHolderID="phDS" Runat="Server">
	<px:PXDataSource ID="ds" runat="server" Visible="True" Width="100%"
        TypeName="PhoneRepairShop.RSSVAssignProcess"
        PrimaryView="Filter"
        >
		<CallbackCommands>

		</CallbackCommands>
	</px:PXDataSource>
</asp:Content>
<asp:Content ID="cont2" ContentPlaceHolderID="phL" runat="Server">
	<px:PXFormView runat="server" ID="CstFormView1" DataMember="Filter" Width="100%" >
		<Template>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule4" StartColumn="True" LabelsWidth="XM" ></px:PXLayoutRule>
			<px:PXDropDown CommitChanges="True" runat="server" ID="CstPXDropDown2" DataField="Priority" ></px:PXDropDown>
			<px:PXNumberEdit CommitChanges="True" runat="server" ID="CstPXNumberEdit4" DataField="TimeWithoutAction" ></px:PXNumberEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule5" StartColumn="True" />
			<px:PXSelector CommitChanges="True" runat="server" ID="CstPXSelector3" DataField="ServiceID" ></px:PXSelector></Template></px:PXFormView>
	<px:PXGrid SyncPosition="True" AllowPaging="True" AdjustPageSize="Auto" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Inquire" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="WorkOrders">
			    <Columns>
				<px:PXGridColumn Type="CheckBox" AllowCheckAll="True" TextAlign="Center" DataField="Selected"></px:PXGridColumn>
				<px:PXGridColumn DataField="OrderNbr"></px:PXGridColumn>
				<px:PXGridColumn DataField="Description"></px:PXGridColumn>
				<px:PXGridColumn DataField="ServiceID"></px:PXGridColumn>
				<px:PXGridColumn DataField="DeviceID"></px:PXGridColumn>
				<px:PXGridColumn DataField="Priority"></px:PXGridColumn>
				<px:PXGridColumn CommitChanges="True" DataField="AssignTo"></px:PXGridColumn>
				<px:PXGridColumn DataField="NbrOfAssignedOrders" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="TimeWithoutAction" Width="100" ></px:PXGridColumn></Columns>
			
				<RowTemplate>
					<px:PXSelector runat="server" ID="CstPXSelector7" DataField="AssignTo" AutoRefresh="True" ></px:PXSelector></RowTemplate></px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
		<ActionBar >
		</ActionBar>
	</px:PXGrid></asp:Content>
