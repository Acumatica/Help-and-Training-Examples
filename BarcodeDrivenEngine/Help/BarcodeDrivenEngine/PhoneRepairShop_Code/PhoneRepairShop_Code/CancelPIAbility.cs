using System;
using PX.Data;
using PX.BarcodeProcessing;
using PX.Common;
using PX.Objects.IN;
using PX.Objects.IN.WMS;

namespace PhoneRepairShopWMS
{
    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class CancelPIAbility : INScanCount.ScanExtension
    {
        [PXOverride]
        public virtual ScanMode<INScanCount> DecorateScanMode(
        ScanMode<INScanCount> original,
        Func<ScanMode<INScanCount>, ScanMode<INScanCount>> base_DecorateScanMode)
        {
            var mode = base_DecorateScanMode(original);

            if (mode is INScanCount.CountMode countMode)
                PatchCountMode(countMode); 

            return mode;
        }

        protected virtual void PatchCountMode(INScanCount.CountMode countMode)
        {
            countMode
                .Intercept.CreateCommands.ByAppend(() =>
                {
                    return new[] { new CancelPICommand() };
                });
        }
        public sealed class CancelPICommand : INScanCount.ScanCommand
        {
            // The code to be scanned to execute the command
            public override string Code => "CANCEL*PI";
            // The name of the PXAction instance that is created for the command
            public override string ButtonName => "ScanCancelPI";
            // The display name of the button for the PXAction instance that is created for the command
            public override string DisplayName => Msg.DisplayName;
            // The Boolean value that indicates when the command can be executed
            protected override bool IsEnabled => Basis.DocumentIsEditable;

            // The logic that is executed for the command
            protected override bool Process() 
            {
                var countReview = PXGraph.CreateInstance<INPIReview>();
                countReview.PIHeader.Current = countReview.PIHeader.Search<INPIHeader.pIID>(Basis.Document.PIID);
                countReview.cancelPI.Press();

                // Clear the screen
                Basis.CurrentMode.Reset(fullReset: true);
                // Inform the user
                Basis.ReportInfo(Msg.Success); 

                return true; 
            }

            [PXLocalizable]
            public abstract class Msg
            {
                public const string DisplayName = "Cancel Count";
                public const string Success = "Physical inventory count has been canceled.";
            }
        }
    }
}
