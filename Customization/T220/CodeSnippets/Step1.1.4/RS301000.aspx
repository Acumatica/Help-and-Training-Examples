<RowTemplate>
	<px:PXLayoutRule runat="server" ID="CstPXLayoutRule21" StartRow="True" >
	  </px:PXLayoutRule>
	<px:PXLayoutRule ControlSize="M" LabelsWidth="SM" runat="server" 
	  ID="CstPXLayoutRule26" StartGroup="True" GroupCaption="Repair Item" >
	  </px:PXLayoutRule>
	<px:PXDropDown runat="server" ID="CstPXDropDown25" DataField="RepairItemType" >
	  </px:PXDropDown>
	<px:PXSegmentMask runat="server" ID="CstPXSegmentMask23" DataField="InventoryID" >
	  </px:PXSegmentMask>
	<px:PXTextEdit runat="server" ID="CstPXTextEdit24" DataField="InventoryID_description" >
	  </px:PXTextEdit>
	<px:PXLayoutRule LabelsWidth="S" StartColumn="True" GroupCaption="Price Info" 
	  runat="server" ID="CstPXLayoutRule27" StartGroup="True" ></px:PXLayoutRule>
	<px:PXNumberEdit runat="server" ID="CstPXNumberEdit22" DataField="BasePrice" >
	  </px:PXNumberEdit>
</RowTemplate>