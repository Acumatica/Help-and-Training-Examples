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
	<px:PXFormView Width="100%" runat="server" ID="CstFormView1" DataMember="Filter" >
		<Template>
			<px:PXLayoutRule LabelsWidth="xm" runat="server" ID="CstPXLayoutRule6" StartColumn="True" ></px:PXLayoutRule>
			<px:PXDropDown runat="server" ID="CstPXDropDown2" DataField="Priority" CommitChanges="True" ></px:PXDropDown>
			<px:PXNumberEdit CommitChanges="True" runat="server" ID="CstPXNumberEdit4" DataField="TimeWithoutAction" ></px:PXNumberEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule5" StartColumn="True" ></px:PXLayoutRule>
			<px:PXSelector CommitChanges="True" runat="server" ID="CstPXSelector3" DataField="ServiceID" ></px:PXSelector></Template></px:PXFormView>
	<px:PXGrid SyncPosition="True" AllowPaging="True" AdjustPageSize="Auto" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Inquire" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="WorkOrders">
			    <Columns>
				<px:PXGridColumn Type="CheckBox" AllowCheckAll="True" TextAlign="Center" DataField="Selected" Width="60" ></px:PXGridColumn>
				<px:PXGridColumn DataField="OrderNbr" Width="140" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Description" Width="220" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ServiceID" Width="140" ></px:PXGridColumn>
				<px:PXGridColumn DataField="DeviceID" Width="140" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Priority" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn CommitChanges="True" DataField="AssignTo" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="NbrOfAssignedOrders" Width="100" ></px:PXGridColumn>
				<px:PXGridColumn DataField="TimeWithoutAction" Width="100" ></px:PXGridColumn></Columns>
			
				<RowTemplate>
					<px:PXSelector runat="server" ID="CstPXSelector7" DataField="Assignee" AutoRefresh="True" ></px:PXSelector>
								<px:PXSelector runat="server" ID="CstPXSelector8" DataField="AssignTo" AutoRefresh="True" ></px:PXSelector></RowTemplate></px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
		<ActionBar >
		</ActionBar>
	</px:PXGrid></asp:Content>
