using PhoneRepairShop;
using PX.Data;
using PX.Tests.Unit;
using PX.Objects.IN;
using System.Linq;
using Xunit;

namespace PhoneRepairShop_Code.Tests
{
    public class RSSVWorkOrderEntryTests : TestBase
    {
        [Fact]
        public void TestRepairWorkOrdersForm()
        {
            Setup<RSSVWorkOrderEntry>(new RSSVSetup());
            var graph = PXGraph.CreateInstance<RSSVWorkOrderEntry>();

            RSSVDevice device = (RSSVDevice)graph.Caches[typeof(RSSVDevice)].
                Insert(new RSSVDevice { DeviceCD = "Device1" });

            RSSVRepairService repairService =
                (RSSVRepairService)graph.Caches[typeof(RSSVRepairService)].
                Insert(new RSSVRepairService
                {
                    ServiceCD = "Service1"
                });
            RSSVRepairService repairService2 =
                (RSSVRepairService)graph.Caches[typeof(RSSVRepairService)].
                Insert(new RSSVRepairService
                {
                    ServiceCD = "Service2"
                });

            graph.Caches[typeof(RSSVRepairPrice)].Insert(new RSSVRepairPrice
            {
                DeviceID = device.DeviceID,
                ServiceID = repairService.ServiceID
            });

            InventoryItem battery1 =
                (InventoryItem)graph.Caches[typeof(InventoryItem)].Insert(new
                InventoryItem
                {
                    InventoryCD = "Battery1"
                });
            InventoryItemExt batteryExt1 =
                battery1.GetExtension<InventoryItemExt>();
            batteryExt1.UsrRepairItem = true;
            batteryExt1.UsrRepairItemType = RepairItemTypeConstants.Battery;
            graph.Caches[typeof(InventoryItem)].Update(battery1);

            InventoryItem backCover1 =
                (InventoryItem)graph.Caches[typeof(InventoryItem)].Insert(new
             InventoryItem
                {
                    InventoryCD = "BackCover1"
                });
            InventoryItemExt backCoverExt1 =
             backCover1.GetExtension<InventoryItemExt>();
            backCoverExt1.UsrRepairItem = true;
            backCoverExt1.UsrRepairItemType =
                RepairItemTypeConstants.BackCover;
            graph.Caches[typeof(InventoryItem)].Update(backCover1);

            InventoryItem work1 =
                (InventoryItem)graph.Caches[typeof(InventoryItem)].Insert(new
             InventoryItem
                {
                    InventoryCD = "Work1",
                    StkItem = false
                });

            RSSVRepairItem repairItemBackCover1 =
                (RSSVRepairItem)graph.Caches[typeof(RSSVRepairItem)].Insert(
                    new RSSVRepairItem
                    {
                        DeviceID = device.DeviceID,
                        ServiceID = repairService.ServiceID
                    });
            repairItemBackCover1.InventoryID = backCover1.InventoryID;
            repairItemBackCover1.Required = true;
            repairItemBackCover1.BasePrice = 10;
            repairItemBackCover1.IsDefault = true;
            repairItemBackCover1.RepairItemType =
                backCoverExt1.UsrRepairItemType;
            graph.Caches[typeof(RSSVRepairItem)].Update(repairItemBackCover1);

            RSSVRepairItem repairItemBattery1 =
                (RSSVRepairItem)graph.Caches[typeof(RSSVRepairItem)].Insert(
                    new RSSVRepairItem
                    {
                        DeviceID = device.DeviceID,
                        ServiceID = repairService.ServiceID
                    });
            repairItemBattery1.InventoryID = battery1.InventoryID;
            repairItemBattery1.Required = true;
            repairItemBattery1.BasePrice = 20;
            repairItemBattery1.IsDefault = true;
            repairItemBattery1.RepairItemType = batteryExt1.UsrRepairItemType;
            graph.Caches[typeof(RSSVRepairItem)].Update(repairItemBattery1);

            RSSVLabor labor = (RSSVLabor)graph.Caches[typeof(RSSVLabor)].
                Insert(new RSSVLabor
                {
                    InventoryID = work1.InventoryID,
                    DeviceID = device.DeviceID,
                    ServiceID = repairService.ServiceID
                });
            labor.DefaultPrice = 2;
            labor.Quantity = 3;
            graph.Caches[typeof(RSSVLabor)].Update(labor);

            RSSVWorkOrder workOrder = (RSSVWorkOrder)graph.
                Caches[typeof(RSSVWorkOrder)].Insert(new RSSVWorkOrder());
            workOrder.DeviceID = device.DeviceID;
            workOrder.ServiceID = repairService.ServiceID;
            graph.Caches[typeof(RSSVWorkOrder)].Update(workOrder);

            Assert.Equal(2, graph.RepairItems.Select().Count);
            Assert.Single(graph.Labor.Select());

            workOrder.ServiceID = repairService2.ServiceID;
            graph.Caches[typeof(RSSVWorkOrder)].Update(workOrder);
            Assert.Equal(2, graph.RepairItems.Select().Count);
            Assert.Single(graph.Labor.Select());

            workOrder.ServiceID = repairService.ServiceID;
            graph.Caches[typeof(RSSVWorkOrder)].Update(workOrder);

            RSSVWorkOrderLabor woLabor = graph.Labor.SelectSingle();
            Assert.Equal(3, woLabor.Quantity);
            woLabor.Quantity = -1;
            graph.Caches[typeof(RSSVWorkOrderLabor)].Update(woLabor);
            PXFieldState fieldState =
                (PXFieldState)graph.Caches[typeof(RSSVWorkOrderLabor)].
                GetStateExt<RSSVWorkOrderLabor.quantity>(woLabor);
            Assert.Equal(PhoneRepairShop.Messages.QuantityCannotBeNegative,
                fieldState.Error);
            Assert.Equal(PXErrorLevel.Error, fieldState.ErrorLevel);
            graph.Labor.Cache.Clear();

            woLabor.Quantity = 1;
            graph.Caches[typeof(RSSVWorkOrderLabor)].Update(woLabor);
            woLabor = (RSSVWorkOrderLabor)graph.
                Caches[typeof(RSSVWorkOrderLabor)].Locate(woLabor);
            fieldState = (PXFieldState)graph.
                Caches[typeof(RSSVWorkOrderLabor)].
                GetStateExt<RSSVWorkOrderLabor.quantity>(woLabor);
            Assert.Equal(PhoneRepairShop.Messages.QuantityToSmall,
                fieldState.Error);
            Assert.Equal(PXErrorLevel.Warning, fieldState.ErrorLevel);
            Assert.Equal(3, woLabor.Quantity);

            repairService2.PreliminaryCheck = true;
            graph.Caches[typeof(RSSVRepairService)].Update(repairService2);
            workOrder.ServiceID = repairService2.ServiceID;
            workOrder.Priority = WorkOrderPriorityConstants.Low;
            graph.Caches[typeof(RSSVWorkOrder)].Update(workOrder);
            workOrder = (RSSVWorkOrder)graph.Caches[typeof(RSSVWorkOrder)].
                Locate(workOrder);
            fieldState = (PXFieldState)graph.Caches[typeof(RSSVWorkOrder)].
                GetStateExt<RSSVWorkOrder.priority>(workOrder);
            Assert.Equal(PhoneRepairShop.Messages.PriorityTooLow, fieldState.Error);
            Assert.Equal(PXErrorLevel.Error, fieldState.ErrorLevel);
        }
    }
}
