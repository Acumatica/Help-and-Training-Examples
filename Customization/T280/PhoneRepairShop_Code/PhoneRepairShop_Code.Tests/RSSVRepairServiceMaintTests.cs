using Xunit;
using PX.Data;
using PX.Tests.Unit;
using PhoneRepairShop;

namespace PhoneRepairShop_Code.Tests
{
    public class RSSVRepairServiceMaintTests : TestBase
    {
        [Fact]
        public void PreliminaryCheckAndWalkInServiceFlags_AreOpposite()
        {
            var graph = PXGraph.CreateInstance<RSSVRepairServiceMaint>();

            RSSVRepairService repairService =
                graph.Caches[typeof(RSSVRepairService)].
                Insert(new RSSVRepairService
                {
                    ServiceCD = "Service1",
                    Description = "Service 1",
                    WalkInService = true,
                    PreliminaryCheck = false
                }) as RSSVRepairService;

            repairService.WalkInService = false;
            graph.Caches[typeof(RSSVRepairService)].Update(repairService);
            Assert.True(repairService.PreliminaryCheck);

            repairService.WalkInService = true;
            graph.Caches[typeof(RSSVRepairService)].Update(repairService);
            Assert.False(repairService.PreliminaryCheck);

            repairService.PreliminaryCheck = false;
            graph.Caches[typeof(RSSVRepairService)].Update(repairService);
            Assert.True(repairService.WalkInService);

            repairService.PreliminaryCheck = true;
            graph.Caches[typeof(RSSVRepairService)].Update(repairService);
            Assert.False(repairService.WalkInService);
        }
    }
}
