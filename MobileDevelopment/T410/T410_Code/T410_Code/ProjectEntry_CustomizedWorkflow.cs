using PX.Data;
using PX.Data.WorkflowAPI;
using PX.Objects.PM;

namespace T410_Code
{
    public class ProjectEntry_CustomizedWorkflow : PXGraphExtension<
        ProjectEntry_Workflow,
        ProjectEntry>
    {
        public override void Configure(PXScreenConfiguration config)
        {
            Configure(config.GetScreenConfigurationContext<ProjectEntry, 
                                                           PMProject>());
        }

        protected virtual void Configure(WorkflowContext<ProjectEntry, 
                                         PMProject> context)
        {
            context.UpdateScreenConfigurationFor(screen => screen
                .WithActions(actions =>
                {
                    actions.Update(
                      g => g.lockBudget,
                      a => a.IsExposedToMobile(true));
                    actions.Update(
                      g => g.unlockBudget,
                      a => a.IsExposedToMobile(true));
                })
            );
        }
    }
}
