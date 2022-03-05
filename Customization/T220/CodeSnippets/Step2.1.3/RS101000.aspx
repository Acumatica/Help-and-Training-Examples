<px:PXFormView ID="form" runat="server" DataSourceID="ds" DataMember="Setup" Width="100%" AllowAutoHide="false">
	<Template>
		<px:PXLayoutRule ControlSize="SM" LabelsWidth="SM" 
		  ID="PXLayoutRule1" runat="server" StartRow="True">
		  </px:PXLayoutRule>
		<px:PXSelector AllowEdit="True" runat="server" ID="CstPXSelector2" DataField="NumberingID" ></px:PXSelector>
		<px:PXSegmentMask runat="server" ID="CstPXSegmentMask4" DataField="WalkInCustomerID" />
		<px:PXSelector runat="server" ID="CstPXSelector1" DataField="DefaultEmployee" />
		<px:PXNumberEdit runat="server" ID="CstPXNumberEdit3" DataField="PrepaymentPercent" /></Template>
	<AutoSize Container="Window" Enabled="True" MinHeight="200" ></AutoSize>
</px:PXFormView>