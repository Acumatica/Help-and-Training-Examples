<px:PXDSCallbackCommand Visible="false" CommitChanges="true" 
  Name="UpdateLaborPrices" ></px:PXDSCallbackCommand>

...
<ActionBar>
	<CustomItems>
		<px:PXToolBarButton Text="UpdateLaborPrices">
			<AutoCallBack Command="UpdateLaborPrices" Target="ds">
				<Behavior CommitChanges="True" ></Behavior>
			</AutoCallBack>
		</px:PXToolBarButton>
	</CustomItems>
</ActionBar>