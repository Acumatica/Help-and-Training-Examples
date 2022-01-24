.WithFlowStates(fss =>
{
    ...

    fss.Add<States.readyForAssignment>(flowState =>
    {
        return flowState
            .WithFieldStates(states =>
            {
                states.AddField<RSSVWorkOrder.customerID>(state => state.IsDisabled());
                states.AddField<RSSVWorkOrder.serviceID>(state => state.IsDisabled());
                states.AddField<RSSVWorkOrder.deviceID>(state => state.IsDisabled());
            })
            .WithActions(actions =>
            {
                actions.Add(g => g.PutOnHold, a => a.IsDuplicatedInToolbar());
                actions.Add(g => g.Assign, a => a
                        .IsDuplicatedInToolbar()
                        .WithConnotation(ActionConnotation.Success));
            });
    });

    ...

    fss.Add<States.assigned>(flowState =>
    {
        return flowState
            .WithFieldStates(states =>
            {
                states.AddField<RSSVWorkOrder.customerID>(state => state.IsDisabled());
                states.AddField<RSSVWorkOrder.serviceID>(state => state.IsDisabled());
                states.AddField<RSSVWorkOrder.deviceID>(state => state.IsDisabled());
            });
    });
})
.WithTransitions(transitions =>
{
    ...
    transitions.AddGroupFrom<States.readyForAssignment>(ts =>
    {
        ts.Add(t => t.To<States.onHold>().IsTriggeredOn(g => g.PutOnHold));
        ts.Add(t => t.To<States.assigned>().IsTriggeredOn(g => g.Assign));
    });
    ...
}))