using System;
using PX.Data;
using PX.BarcodeProcessing;
using PX.Common;
using PX.Objects.IN;
using PX.Objects.IN.WMS;

namespace PhoneRepairShopWMS
{
    public abstract class WMSNoteAppender<TDocument, TScanBasis, TScanGraph> : 
        PXGraphExtension<TScanBasis, TScanGraph>
    where TDocument : class, PXBqlTable, IBqlTable, new()
    where TScanBasis : BarcodeDrivenStateMachine<TScanBasis, TScanGraph>
    where TScanGraph : PXGraph, new() 
    {
        protected TScanBasis Basis => Base1;

        public const string NotePrefix = "note:"; 

        [PXOverride]
        public virtual bool? ProcessCustomScan(string barcode, 
            Func<string, bool?> base_ProcessCustomScan)
        {
            if (barcode.StartsWith(NotePrefix))
            {
                string value = barcode.Substring(NotePrefix.Length).Trim();

                if (!string.IsNullOrEmpty(value))
                {
                    PXCache<TDocument> docCache = Basis.Graph.Caches<TDocument>();
                    // Use an abstract function to get the entity.
                    var document = GetNoteOwnerEntity(); 

                    if (document != null && docCache.Fields.Contains("NoteID"))
                    {
                        string existingValue = PXNoteAttribute.GetNote(docCache, document);
                        string newLine = 
                            $"[{Basis.Graph.Accessinfo.UserName}@{DateTime.Now.ToShortDateString()}]: {value}";

                        PXNoteAttribute.SetNote(docCache, document,
                            string.IsNullOrEmpty(existingValue)
                                ? newLine
                                : existingValue + Environment.NewLine + newLine);

                        Basis.SaveChanges();
                        Basis.ReportInfo(Msg.Success); 

                        return true; 
                    }
                    else
                    {
                        Basis.ReportError(Msg.Fail);

                        return false; 
                    }
                }
            }

            return base_ProcessCustomScan(barcode);
        }

        protected abstract TDocument GetNoteOwnerEntity(); 

        [PXLocalizable]
        public abstract class Msg 
        {
            public const string Success = "Your note was successfully added.";
            public const string Fail = "The system was not able to add your note.";
        }
    }

    // Acuminator disable once PX1016 ExtensionDoesNotDeclareIsActiveMethod extension should be constantly active
    public class ItemNoteAttacher : WMSNoteAppender<InventoryItem, 
        InventoryItemLookup, InventoryItemLookup.Host>
    {
        protected override InventoryItem GetNoteOwnerEntity() => 
            Basis.InventoryItem.Select(); // decide how to get the underlying entity
    }
}
