public class RSSVWorkOrderWorkflow : PX.Data.PXGraphExtension<RSSVWorkOrderEntry>
{
    ...

    #region Conditions
    public class Conditions : Condition.Pack
    {
        ...
        public Condition DisableCreateInvoice => GetOrCreate(b => b.FromBql<
            Where<RSSVWorkOrder.invoiceNbr.IsNotNull>>());
    }
    #endregion

    public override void Configure(PXScreenConfiguration config)
    {
        ...

        context.AddScreenConfigurationFor(screen => screen
            .StateIdentifierIs<RSSVWorkOrder.status>()
            .AddDefaultFlow(flow => flow
                .WithFlowStates(fss =>
                {
                    ...

                    fss.Add<States.pendingPayment>(flowState =>
                    {
                        return flowState
                            ...
                            .WithActions(actions =>
                            {
                                actions.Add(g => g.PutOnHold, a => a.IsDuplicatedInToolbar());
                                actions.Add(g => g.CreateInvoiceAction, a => a
                                    .IsDuplicatedInToolbar()
                                    .WithConnotation(ActionConnotation.Success));
                            });
                    });

                    ...

                    fss.Add<States.completed>(flowState =>
                    {
                        return flowState
                            ...
                            .WithActions(actions =>
                            {
                                actions.Add(g => g.CreateInvoiceAction, a => a
                                .IsDuplicatedInToolbar()
                                .WithConnotation(ActionConnotation.Success));
                            });
                    });
                })
                ...
            ...
            .WithActions(actions =>
            {
                ...
                actions.Add(g => g.CreateInvoiceAction, c => c
                    .WithCategory(processingCategory)
                    .IsDisabledWhen(conditions.DisableCreateInvoice));
            })
            ...
    }
}