context.AddScreenConfigurationFor(screen => screen
    .StateIdentifierIs<RSSVWorkOrder.status>()
    .AddDefaultFlow(flow => flow
        .WithFlowStates(fss =>
        {
            ...

            fss.Add<States.assigned>(flowState =>
            {
                return flowState
                    ...
                    .WithActions(actions =>
                    {
                        actions.Add(g => g.Complete, a => a
                            .IsDuplicatedInToolbar()
                            .WithConnotation(ActionConnotation.Success));
                    });
            });

            fss.Add<States.completed>(flowState =>
            {
                return flowState
                    .WithFieldStates(states =>
                    {
                        states.AddField<RSSVWorkOrder.customerID>(state => 
                            state.IsDisabled());
                        states.AddField<RSSVWorkOrder.serviceID>(state => 
                            state.IsDisabled());
                        states.AddField<RSSVWorkOrder.deviceID>(state => 
                            state.IsDisabled());
                    });
            });
        })
        .WithTransitions(transitions =>
        {
            ...
            transitions.AddGroupFrom<States.assigned>(ts =>
            {
                ts.Add(t => t.To<States.completed>().IsTriggeredOn(g => 
                    g.Complete));
            });
        }))
    ...
    .WithActions(actions =>
    {
        actions.Add(g => g.ReleaseFromHold, c => c
                .WithCategory(processingCategory));
        actions.Add(g => g.PutOnHold, c => c
                .WithCategory(processingCategory));
        actions.Add(g => g.Assign, c => c
            .WithCategory(processingCategory)
            .WithForm(formAssign)
            .WithFieldAssignments(fields => {
                fields.Add<RSSVWorkOrder.assignee>(f => 
                    f.SetFromFormField(formAssign, "Assignee"));
            }));
        actions.Add(g => g.Complete, c => c
            .WithCategory(processingCategory)
            .WithFieldAssignments(fas => fas.Add<RSSVWorkOrder.dateCompleted>(f =>
            f.SetFromToday())));
    })
    .WithForms(forms => forms.Add(formAssign)));