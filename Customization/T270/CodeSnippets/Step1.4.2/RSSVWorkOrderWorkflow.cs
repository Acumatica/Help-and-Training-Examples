.WithActions(actions =>
{
    ...
    actions.Add(g => g.Assign, c => c
        .WithCategory(processingCategory)
        .WithForm(formAssign)
        .WithFieldAssignments(fields => {
            fields.Add<RSSVWorkOrder.assignee>(f => 
                f.SetFromFormField(formAssign, "Assignee"));
        }));
})