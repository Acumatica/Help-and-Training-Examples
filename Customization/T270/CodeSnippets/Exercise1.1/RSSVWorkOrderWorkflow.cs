context.AddScreenConfigurationFor(screen => screen
    .StateIdentifierIs<RSSVWorkOrder.status>()
    .AddDefaultFlow(flow => flow
        .WithFlowStates(fss =>
        {
            fss.Add<States.onHold>(flowState =>
            {
                return flowState
                    .IsInitial()
                    .WithActions(actions =>
                    {
                        actions.Add(g => g.ReleaseFromHold, a => a
                        .IsDuplicatedInToolbar()
                        .WithConnotation(ActionConnotation.Success));
                    });
            });

            fss.Add<States.readyForAssignment>(flowState =>
            {
                return flowState
                    .WithFieldStates(states =>
                    {
                        ...
                    })
                    .WithActions(actions =>
                    {
                        actions.Add(g => g.PutOnHold, a => a.IsDuplicatedInToolbar());
                    });
            });

            fss.Add<States.pendingPayment>(flowState =>
            {
                return flowState
                    .WithFieldStates(states =>
                    {
                        ...
                    })
                    .WithActions(actions =>
                    {
                        actions.Add(g => g.PutOnHold, a => a.IsDuplicatedInToolbar());
                    });
            });
        })
        .WithTransitions(transitions =>
        {
            transitions.AddGroupFrom<States.onHold>(ts =>
            {
                ts.Add(t => t.To<States.readyForAssignment>()
                    .IsTriggeredOn(g => g.ReleaseFromHold)
                    .When(conditions.DoesNotRequirePrepayment)
                    );
                ts.Add(t => t.To<States.pendingPayment>()
                    .IsTriggeredOn(g => g.ReleaseFromHold)
                    .When(conditions.RequiresPrepayment));
            });
            transitions.AddGroupFrom<States.readyForAssignment>(ts =>
            {
                ts.Add(t => t.To<States.onHold>().IsTriggeredOn(g => g.PutOnHold));
            });
            transitions.AddGroupFrom<States.pendingPayment>(ts =>
            {
                ts.Add(t => t.To<States.onHold>().IsTriggeredOn(g => g.PutOnHold));
            });
        }))
    .WithCategories(categories =>
    {
        categories.Add(processingCategory);
    })
    .WithActions(actions =>
    {
        actions.Add(g => g.ReleaseFromHold, c => c
                .WithCategory(processingCategory));
        actions.Add(g => g.PutOnHold, c => c
                .WithCategory(processingCategory));
    })
);