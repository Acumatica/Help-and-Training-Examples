using System;
using Core.Login;
using Core.TestExecution;

namespace AIAssistedDiagnostics.ModernUI.Tests.Tests
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
           
            using (TestExecution.CreateTestStepGroup("RS301000 labor quantity validation test"))
            {
                Console.WriteLine("Starting RS301000 labor quantity validation test...");

                var rs301000Tests = new RS301000_LaborQuantityTests();
                rs301000Tests.LaborQuantity_WhenBelowConfigured_RestoresConfiguredQuantity();

            }
        }
    }
}