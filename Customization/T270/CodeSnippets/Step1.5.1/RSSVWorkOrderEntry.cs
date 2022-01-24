public PXAction<RSSVWorkOrder> Complete;
[PXButton]
[PXUIField(DisplayName = "Complete", Enabled = false)]
protected virtual IEnumerable complete(PXAdapter adapter) => adapter.Get();