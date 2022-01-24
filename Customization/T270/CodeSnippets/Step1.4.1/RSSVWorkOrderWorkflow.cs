public override void Configure(PXScreenConfiguration config)
{
    var context = config.GetScreenConfigurationContext<RSSVWorkOrderEntry, RSSVWorkOrder>();

    var formAssign = context.Forms.Create("FormAssign", form =>
        form.Prompt("Select Assignee").WithFields(fields =>
        {
            fields.Add("Assignee", field => field
                .WithSchemaOf<RSSVWorkOrder.assignee>()
                .IsRequired()
                .Prompt("Assignee"));
        }));

    context.AddScreenConfigurationFor(screen => screen
        ...
        .WithForms(forms => forms.Add(formAssign))
        );
}