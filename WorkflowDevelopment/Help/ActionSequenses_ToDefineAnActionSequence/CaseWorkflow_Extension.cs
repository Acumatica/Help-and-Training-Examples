using PX.Data;
using PX.Data.WorkflowAPI;
using PX.Objects.CR;
using PX.Objects.CR.Workflows;
using static PX.Data.WorkflowAPI.BoundedTo<PX.Objects.CR.CRCaseMaint, PX.Objects.CR.CRCase>;

namespace Test_ActionSequence
{
    public class CaseWorkflow_Extension : PXGraphExtension<CaseWorkflow, CRCaseMaint>
    {
        public class Conditions : Condition.Pack
        {
          public Condition IsBillable =>
            GetOrCreate(b => b.FromBql<CRCase.isBillable.IsEqual<True>>());
        }
      
        public sealed override void Configure(PXScreenConfiguration config) =>
            Configure(config.GetScreenConfigurationContext<CRCaseMaint, CRCase>());

        protected static void Configure(WorkflowContext<CRCaseMaint, CRCase> context)
        {          
            var conditions = context.Conditions.GetPack<Conditions>();

            context.UpdateScreenConfigurationFor(screen =>
            {
                return screen
                  .WithActionSequences(sequences =>
                  {
                      sequences.Add(s => s
                        .AfterAction("Close")
                        .RunAction("release")
                        .StopOnError(true));
                      sequences.Add(s => s
                        .AfterAction("release")
                        .RunAction("viewInvoice")
                        .AppliesWhen(conditions.IsBillable)
                        .StopOnError(true));
                  });
            });
        }
    }
}