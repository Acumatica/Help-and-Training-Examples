using System;
using Core;
using AIAssistedDiagnostics.ModernUI.Tests.Helpers;
using AIAssistedDiagnostics.ModernUI.Tests.Wrappers.Custom;

namespace AIAssistedDiagnostics.ModernUI.Tests.Tests
{
    public sealed class RS301000_LaborQuantityTests : Wrapper
    {
        public void RepairWorkOrdersScreen_CanOpenPreparedWorkOrder()
        {
            // Arrange
            var screen = new RepairWorkOrdersScreen();

            // Act
            screen.Open();
            screen.OpenWorkOrder(TestData.PreparedWorkOrderNbr);

            decimal actualQuantity = screen.GetFirstLaborQuantity();

            // Assert
            AssertDecimalEqual(
                TestData.ConfiguredLaborQuantity,
                actualQuantity,
                "The prepared repair work order should open with the expected configured labor quantity.");
        }

        //TODO: Implement the test for verifying that labor quantity is restored after setting it below the configured value.

        private static void AssertDecimalEqual(
            decimal expected,
            decimal actual,
            string message)
        {
            if (expected != actual)
            {
                throw new InvalidOperationException(
                    $"{message}{Environment.NewLine}" +
                    $"Expected: {expected}{Environment.NewLine}" +
                    $"Actual:   {actual}");
            }
        }
    }
}