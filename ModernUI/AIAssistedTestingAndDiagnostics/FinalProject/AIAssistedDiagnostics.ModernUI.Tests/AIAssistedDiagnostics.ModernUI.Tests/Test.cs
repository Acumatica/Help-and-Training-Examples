using AIAssistedDiagnostics.ModernUI.Tests.Tests;
using ClassGenerator;
using Core.Login;
using Core.TestExecution;

namespace AIAssistedDiagnostics.ModernUI.Tests
{
    public class Test : Check
    {
        public override void Execute()
        {
            PxLogin.LoginToDestinationSite();

            StartTests startTests = new StartTests();
            startTests.Execute();
        }
    }
}
