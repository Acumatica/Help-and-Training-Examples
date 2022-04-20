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
			<px:PXLayoutRule ControlSize="S" runat="server" ID="CstPXLayoutRule4" StartColumn="True" LabelsWidth="XM" ></px:PXLayoutRule>
			<px:PXDropDown runat="server" ID="CstPXDropDown2" DataField="Priority" CommitChanges="True" ></px:PXDropDown>
			<px:PXNumberEdit LabelWidth="" CommitChanges="True" runat="server" ID="CstPXNumberEdit4" DataField="TimeWithoutAction" ></px:PXNumberEdit>
			<px:PXLayoutRule runat="server" ID="CstPXLayoutRule5" StartColumn="True" ></px:PXLayoutRule>
			<px:PXSelector CommitChanges="True" runat="server" ID="CstPXSelector3" DataField="ServiceID" ></px:PXSelector></Template></px:PXFormView>
	<px:PXGrid SyncPosition="True" AllowPaging="True" AdjustPageSize="Auto" ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Inquire" AllowAutoHide="false">
		<Levels>
			<px:PXGridLevel DataMember="WorkOrders">
			    <Columns>
				<px:PXGridColumn AllowCheckAll="True" Type="CheckBox" TextAlign="Center" DataField="Selected" Width="60" ></px:PXGridColumn>
				<px:PXGridColumn DataField="OrderNbr" Width="140" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Description" Width="220" ></px:PXGridColumn>
				<px:PXGridColumn DataField="ServiceID" Width="140" ></px:PXGridColumn>
				<px:PXGridColumn DataField="DeviceID" Width="140" ></px:PXGridColumn>
				<px:PXGridColumn DataField="Priority" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn CommitChanges="True" DataField="Assignee" Width="70" ></px:PXGridColumn>
				<px:PXGridColumn DataField="TimeWithoutAction" Width="100" ></px:PXGridColumn></Columns>
			
				<RowTemplate>
					<px:PXSelector runat="server" ID="CstPXSelector8" DataField="Assignee" AutoRefresh="True" /></RowTemplate></px:PXGridLevel>
		</Levels>
		<AutoSize Container="Window" Enabled="True" MinHeight="150" ></AutoSize>
		<ActionBar >
		</ActionBar>
	</px:PXGrid></asp:Content>
