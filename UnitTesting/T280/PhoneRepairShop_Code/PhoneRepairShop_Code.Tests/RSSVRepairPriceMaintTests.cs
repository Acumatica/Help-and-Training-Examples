using Xunit;
using PX.Data;
using PX.Objects.IN;
using PX.Tests.Unit;
using PhoneRepairShop;

namespace PhoneRepairShop_Code.Tests
{
    public class RSSVRepairPriceMaintTests : TestBase
    {
        [Fact]
        public void TestServicesAndPricesForm()
        {
            var graph = PXGraph.CreateInstance<RSSVRepairPriceMaint>();

            graph.Caches[typeof(RSSVDevice)].Insert(new RSSVDevice
            {
                DeviceCD = "Device1"
            });
            graph.Caches[typeof(RSSVRepairService)].Insert(new
             RSSVRepairService
            {
                ServiceCD = "Service1"
            });

            RSSVRepairPrice repairPrice =
                (RSSVRepairPrice)graph.Caches[typeof(RSSVRepairPrice)].
                Insert(new RSSVRepairPrice());

            InventoryItem battery1 = (InventoryItem)graph.
                Caches[typeof(InventoryItem)].Insert(new
                InventoryItem
                {
                    InventoryCD = "Battery1"
                });
            graph.Caches[typeof(InventoryItemCurySettings)].Insert(new
                InventoryItemCurySettings
            {
                InventoryID = battery1.InventoryID,
                CuryID = "USD"
            });
            InventoryItemExt batteryExt1 =
                battery1.GetExtension<InventoryItemExt>();
            batteryExt1.UsrRepairItem = true;
            batteryExt1.UsrRepairItemType = RepairItemTypeConstants.Battery;
            graph.Caches[typeof(InventoryItem)].Update(battery1);

            InventoryItem battery2 =
             (InventoryItem)graph.Caches[typeof(InventoryItem)].Insert(new
                InventoryItem
             {
                 InventoryCD = "Battery2"
             });
            graph.Caches[typeof(InventoryItemCurySettings)].Insert(new
                InventoryItemCurySettings
            {
                InventoryID = battery2.InventoryID,
                CuryID = "USD"
            });
            InventoryItemExt batteryExt2 =
                battery2.GetExtension<InventoryItemExt>();
            batteryExt2.UsrRepairItem = true;
            batteryExt2.UsrRepairItemType = RepairItemTypeConstants.Battery;
            graph.Caches[typeof(InventoryItem)].Update(battery2);

            InventoryItem backCover1 =
             (InventoryItem)graph.Caches[typeof(InventoryItem)].Insert(new
                InventoryItem
             {
                 InventoryCD = "BackCover1"
             });
            graph.Caches[typeof(InventoryItemCurySettings)].Insert(new
                InventoryItemCurySettings
            {
                InventoryID = backCover1.InventoryID,
                CuryID = "USD"
            });
            InventoryItemExt backCoverExt1 =
             backCover1.GetExtension<InventoryItemExt>();
            backCoverExt1.UsrRepairItem = true;
            backCoverExt1.UsrRepairItemType = RepairItemTypeConstants.BackCover;
            graph.Caches[typeof(InventoryItem)].Update(backCover1);

            InventoryItem work1 = (InventoryItem)graph.
                Caches[typeof(InventoryItem)].Insert(new InventoryItem
                {
                    InventoryCD = "Work1",
                    StkItem = false
                });

            // Configure the back cover repair item
            RSSVRepairItem repairItemBackCover1 =
                (RSSVRepairItem)graph.Caches[typeof(RSSVRepairItem)].Insert(
                    new RSSVRepairItem
                    {
                        InventoryID = backCover1.InventoryID,
                        Required = true,
                        BasePrice = 10,
                        IsDefault = true
                    });

            // Configure the first battery repair item
            RSSVRepairItem repairItemBattery1 =
                (RSSVRepairItem)graph.Caches[typeof(RSSVRepairItem)].Insert(
                    new RSSVRepairItem
                    {
                        InventoryID = battery1.InventoryID
                    });
            repairItemBattery1.Required = true;
            repairItemBattery1.BasePrice = 20;
            repairItemBattery1.IsDefault = true;
            graph.Caches[typeof(RSSVRepairItem)].Update(repairItemBattery1);

            // Configure the second battery repair item
            RSSVRepairItem repairItemBattery2 =
                (RSSVRepairItem)graph.Caches[typeof(RSSVRepairItem)].Insert(
                    new RSSVRepairItem
                    { InventoryID = battery2.InventoryID });
            repairItemBattery2.Required = false;
            repairItemBattery2.BasePrice = 30;
            repairItemBattery2.IsDefault = true;
            graph.Caches[typeof(RSSVRepairItem)].Update(repairItemBattery2);

            // 2nd battery is not required -> 1st battery is also not required
            Assert.False(repairItemBattery1.Required);
            // 2nd batt is used by default -> 1st batt is not used by default
            Assert.False(repairItemBattery1.IsDefault);
            // The back cover's Required and Default fields are not affected
            Assert.True(repairItemBackCover1.Required);
            Assert.True(repairItemBackCover1.IsDefault);

            RSSVLabor labor = (RSSVLabor)graph.Caches[typeof(RSSVLabor)].
                Insert(new RSSVLabor
                {
                    InventoryID = work1.InventoryID,
                    DefaultPrice = 2,
                    Quantity = 3
                });
            Assert.Equal(6, labor.ExtPrice);
            Assert.Equal(66, repairPrice.Price);
        }
    }
}
