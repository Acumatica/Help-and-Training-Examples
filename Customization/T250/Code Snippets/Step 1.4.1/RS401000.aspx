...
<asp:Content ID="cont2" ContentPlaceHolderID="phF" Runat="Server">
  <px:PXFormView runat="server" ID="CstFormView1" DataSourceID="ds" 
      DataMember="Filter" Width="100%" >
    <Template>
      <px:PXLayoutRule LabelsWidth="XM" runat="server" ID="CstPXLayoutRule3" 
        StartColumn="True" ></px:PXLayoutRule>
      <px:PXSegmentMask CommitChanges="True" runat="server" ID="CstPXSegmentMask1" 
        DataField="CustomerID" ></px:PXSegmentMask>
      <px:PXSelector CommitChanges="True" runat="server" ID="CstPXSelector2" 
        DataField="ServiceID" ></px:PXSelector>
      <px:PXCheckBox CommitChanges="True" runat="server" 
        ID="CstPXCheckBoxGroupByStatus" DataField="GroupByStatus"/>
    </Template>
  </px:PXFormView>
</asp:Content>
...