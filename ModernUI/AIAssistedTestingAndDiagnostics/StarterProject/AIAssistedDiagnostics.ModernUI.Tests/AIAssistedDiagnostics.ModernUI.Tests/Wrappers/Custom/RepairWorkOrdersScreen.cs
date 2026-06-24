using System;
using System.Globalization;
using System.Threading;
using GeneratedWrappers.AIAssistedDiagnostics.ModernUI.Tests.Wrappers.Generated;

namespace AIAssistedDiagnostics.ModernUI.Tests.Wrappers.Custom
{
    /// <summary>
    /// Friendly helper over the generated Modern UI wrapper for RS301000.
    /// This class inherits from the generated RS301000 wrapper because the generated
    /// work order containers and labor grid are protected members.
    /// </summary>
    public sealed class RepairWorkOrdersScreen : RS301000
    {
        public void Open()
        {
            OpenScreen();

            // Temporary wait for diagnostics. If this solves the next issue,
            // replace it later with the project’s normal Test SDK wait pattern.
            Thread.Sleep(3000);
        }

        public void OpenWorkOrder(string workOrderNbr)
        {
            if (string.IsNullOrWhiteSpace(workOrderNbr))
            {
                throw new ArgumentException(
                    "The work order number must be provided.",
                    nameof(workOrderNbr));
            }

            WorkOrders_fsColumnAOrder.OrderNbr.Type(workOrderNbr);

            // Temporary wait to allow the selector/screen data to load.
            Thread.Sleep(2000);
        }

        public decimal GetFirstLaborQuantity()
        {
            string value = Labor_gridLabor.Row.Quantity.GetValue();
            Console.WriteLine($"Raw labor quantity value: '{value}'");

            if (decimal.TryParse(
                    value,
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out decimal parsed))
            {
                return parsed;
            }

            throw new FormatException(
                $"The labor quantity value '{value}' could not be converted to decimal.");
        }

        public void SetFirstLaborQuantity(decimal quantity)
        {
            Labor_gridLabor.Row.Quantity.Type(
                quantity.ToString(CultureInfo.InvariantCulture));
        }

        public void TriggerValidation()
        {
            Save();

            Thread.Sleep(2000);

            Labor_gridLabor.Refresh();

            Thread.Sleep(2000);
        }
    }
}