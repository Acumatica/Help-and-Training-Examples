public PXAction<RSSVWorkOrder> PutOnHold;
[PXButton, PXUIField(DisplayName = "Hold",
  MapEnableRights = PXCacheRights.Select,
  MapViewRights = PXCacheRights.Select)]
protected virtual IEnumerable putOnHold(PXAdapter adapter) => adapter.Get();