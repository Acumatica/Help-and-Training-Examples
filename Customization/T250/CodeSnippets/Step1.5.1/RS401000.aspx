...
<asp:Content ID="cont3" ContentPlaceHolderID="phG" Runat="Server">
  <px:PXGrid ID="grid" runat="server" DataSourceID="ds" Width="100%" Height="150px" SkinID="Details" AllowAutoHide="false">
    <Levels>
      <px:PXGridLevel DataMember="DetailsView">
          <Columns>
              <px:PXGridColumn DataField="OrderType" Width="70" />
              <px:PXGridColumn DataField="OrderNbr" Width="72" />
              <px:PXGridColumn DataField="Status" Width="140" />
              <px:PXGridColumn DataField="InvoiceNbr" Width="72" CommitChanges="True" />
              <px:PXGridColumn DataField="PercentPaid" Width="72" />
              <px:PXGridColumn DataField="ARInvoice__DueDate" Width="72" />
              <px:PXGridColumn DataField="ARInvoice__CuryDocBal" Width="100" />
          </Columns>
          <RowTemplate>
            <px:PXSelector ID="edInvoiceNbr" runat="server"
                DataField="InvoiceNbr" Enabled="False" AllowEdit="True" />
          </RowTemplate>
      </px:PXGridLevel>
    </Levels>
    <AutoSize Container="Window" Enabled="True" MinHeight="150" />
    <ActionBar >
    </ActionBar>
  </px:PXGrid>
</asp:Content>
