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

        public void LaborQuantity_WhenBelowConfigured_RestoresConfiguredQuantity()
        {
            // Arrange
            var screen = new RepairWorkOrdersScreen();

            screen.Open();
            screen.OpenWorkOrder(TestData.PreparedWorkOrderNbr);

            // Act
            screen.SetFirstLaborQuantity(TestData.BelowConfiguredLaborQuantity);
            screen.TriggerValidation();

            decimal actualQuantity = screen.GetFirstLaborQuantity();

            // Assert
            AssertDecimalEqual(
                TestData.ExpectedRestoredLaborQuantity,
                actualQuantity,
                "After entering a below-configured labor quantity, the UI should restore the configured quantity.");
        }


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