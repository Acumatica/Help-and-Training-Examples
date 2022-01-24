public override void Configure(PXScreenConfiguration config)
{
    ...

    context.AddScreenConfigurationFor(screen =>
        screen
            .StateIdentifierIs<RSSVWorkOrder.status>()
            .AddDefaultFlow(flow =>
                flow
                .WithFlowStates(fss =>
                {
                    ...
                    fss.Add<States.pendingPayment>(flowState =>
                    {
                        return flowState
                            .WithFieldStates(...)
                            .WithActions(...)
                            .WithEventHandlers(handlers =>
                            {
                                handlers.Add(g => g.OnInvoiceGotPrepaid);
                            });
                    });
                    ...
                })
                .WithTransitions(transitions =>
                {
                    ...
                    transitions.AddGroupFrom<States.pendingPayment>(ts =>
                    {
                        ...
                        ts.Add(t => t.To<States.readyForAssignment>()
                            .IsTriggeredOn(g => g.OnInvoiceGotPrepaid));
                    });
                    ...
                })
            )
            .WithHandlers(handlers =>
            {
                ...
                handlers.Add(handler => handler
                    .WithTargetOf<ARRegister>()
                    .OfEntityEvent<RSSVWorkOrder.MyEvents>(e => e.InvoiceGotPrepaid)
                    .Is(g => g.OnInvoiceGotPrepaid)
                    .UsesPrimaryEntityGetter<
                        SelectFrom<RSSVWorkOrder>.
                        Where<RSSVWorkOrder.invoiceNbr
                        .IsEqual<ARRegister.refNbr.FromCurrent>>>());
            })
            .WithActions(...)
            .WithCategories(...)
            .WithForms(...)
            );
}
