public PXAction<RSSVWorkOrder> Assign;
[PXButton]
[PXUIField(DisplayName = "Assign", Enabled = false)]
protected virtual IEnumerable assign(PXAdapter adapter) => adapter.Get();