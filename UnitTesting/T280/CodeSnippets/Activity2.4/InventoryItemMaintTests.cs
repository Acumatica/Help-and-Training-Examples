using Xunit;
using PX.Data;
using PX.Tests.Unit;
using PX.Objects.IN;
using PhoneRepairShop;

namespace PhoneRepairShop_Code.Tests
{
    public class InventoryItemMaintTests : TestBase
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void RepairItemTypeEnabled_WhenRepairItemSelected
            (bool enabled)
        {
            var graph = PXGraph.CreateInstance<InventoryItemMaint>();

            InventoryItem item =
                (InventoryItem)graph.Caches[typeof(InventoryItem)].Insert(
                    new InventoryItem
                    {
                        InventoryCD = "Item1",
                        Descr = "Item 1"
                    });

            InventoryItemExt itemExt = item.GetExtension<InventoryItemExt>();

            itemExt.UsrRepairItem = enabled;
            graph.Caches[typeof(InventoryItem)].Update(item);
            PXFieldState fieldState =
                ((PXFieldState)graph.Caches[typeof(InventoryItem)].GetStateExt<
                InventoryItemExt.usrRepairItemType>(item));
            Assert.True(enabled == fieldState.Enabled);
        }
    }
}
