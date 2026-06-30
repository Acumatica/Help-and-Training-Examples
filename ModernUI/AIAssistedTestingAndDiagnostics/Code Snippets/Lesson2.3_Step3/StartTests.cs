using System;
using AIAssistedDiagnostics.ModernUI.Tests.Tests;
using Core.Login;
using Core.TestExecution;

namespace GeneratedWrappers.SOLUTIONNAME
{
  internal class StartTests
  {
      public void Execute()
      {
          PxLogin.LoginToDestinationSite();

          using (TestExecution.CreateTestStepGroup("RS301000 smoke test"))
          {
              Console.WriteLine("Starting RS301000 smoke test...");

              var rs301000Tests = new RS301000_LaborQuantityTests();
              rs301000Tests.RepairWorkOrdersScreen_CanOpenPreparedWorkOrder();
          }


          // Run the test that verifies that the labor quantity is restored after setting it below the configured value.
          ///////////// The modified code
          using (TestExecution.CreateTestStepGroup("RS301000 labor quantity validation test"))
          {
              Console.WriteLine("Starting RS301000 labor quantity validation test...");

              var rs301000Tests = new RS301000_LaborQuantityTests();
              rs301000Tests.LaborQuantity_WhenBelowConfigured_RestoresConfiguredQuantity();

          }
          //////////// The end of the modified code
      }
  }
}