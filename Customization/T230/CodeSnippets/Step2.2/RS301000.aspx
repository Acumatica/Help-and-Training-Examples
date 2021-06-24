<px:PXDSCallbackCommand Visible="false" CommitChanges="true" 
  Name="UpdateItemPrices" ></px:PXDSCallbackCommand>

...
<ActionBar>
	<CustomItems>
		<px:PXToolBarButton Text="UpdateItemPrices">
			<AutoCallBack Command="UpdateItemPrices" Target="ds">
				<Behavior CommitChanges="True" ></Behavior>
			</AutoCallBack>
		</px:PXToolBarButton>
	</CustomItems>
</ActionBar>