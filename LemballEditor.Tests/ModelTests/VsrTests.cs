using FluentAssertions;
using LemballEditor.Models;

namespace LemballEditor.Tests.ModelTests
{
    [TestClass]
    public class VsrTests
    {
        [TestMethod]
        public void ShouldSetTheDirectoryPointersToZeroByDefault()
        {
            var vsr = ServiceFactory.CreateVsr();

            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Fun).Should().Be(0);
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Tricky).Should().Be(0);
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Taxing).Should().Be(0);
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Mayhem).Should().Be(0);
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Network).Should().Be(0);
        }

        [TestMethod]
        public void ShouldStoreTheDirectoryPointers()
        {
            var vsr = ServiceFactory.CreateVsr();

            vsr.SetLevelDirectoryPointer(LevelGroupName.Fun, 1);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Tricky, 2);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Taxing, 3);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Mayhem, 4);
            vsr.SetLevelDirectoryPointer(LevelGroupName.Network, 5);

            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Fun).Should().Be(1);
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Tricky).Should().Be(2);
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Taxing).Should().Be(3);
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Mayhem).Should().Be(4);
            _ = vsr.GetLevelDirectoryPointer(LevelGroupName.Network).Should().Be(5);
        }

        [TestMethod]
        public void ShouldStoreTheAssetData()
        {
            var vsr = ServiceFactory.CreateVsr();
            byte[] data = [1, 2, 3, 4];
            vsr.AssetData = data;

            _ = vsr.AssetData.Should().BeEquivalentTo(data);
        }

        [TestMethod]
        public void ShouldStoreTheFunDirectoryData()
        {
            byte[] data = [1, 2, 3, 4];

            var vsr = ServiceFactory.CreateVsr();
            vsr.SetLevelDirectoryData(LevelGroupName.Fun, data);

            _ = vsr.GetLevelDirectoryData(LevelGroupName.Fun).Should().BeEquivalentTo(data);
        }

        [TestMethod]
        public void ShouldStoreTheTrickyDirectoryData()
        {
            byte[] data = [1, 2, 3, 4];

            var vsr = ServiceFactory.CreateVsr();
            vsr.SetLevelDirectoryData(LevelGroupName.Tricky, data);

            _ = vsr.GetLevelDirectoryData(LevelGroupName.Tricky).Should().BeEquivalentTo(data);
        }

        [TestMethod]
        public void ShouldStoreTheTaxingDirectoryData()
        {
            byte[] data = [1, 2, 3, 4];

            var vsr = ServiceFactory.CreateVsr();
            vsr.SetLevelDirectoryData(LevelGroupName.Taxing, data);

            _ = vsr.GetLevelDirectoryData(LevelGroupName.Taxing).Should().BeEquivalentTo(data);
        }

        [TestMethod]
        public void ShouldStoreTheMayhemDirectoryData()
        {
            byte[] data = [1, 2, 3, 4];

            var vsr = ServiceFactory.CreateVsr();
            vsr.SetLevelDirectoryData(LevelGroupName.Mayhem, data);

            _ = vsr.GetLevelDirectoryData(LevelGroupName.Mayhem).Should().BeEquivalentTo(data);
        }

        [TestMethod]
        public void ShouldStoreTheNetworkDirectoryData()
        {
            byte[] data = [1, 2, 3, 4];

            var vsr = ServiceFactory.CreateVsr();
            vsr.SetLevelDirectoryData(LevelGroupName.Network, data);

            _ = vsr.GetLevelDirectoryData(LevelGroupName.Network).Should().BeEquivalentTo(data);
        }

        [TestMethod]
        public void ShouldReturnTheFunAddressBasedOnTheAssetDataSize()
        {
            var vsr = new Vsr
            {
                AssetData = new byte[5]
            };

            _ = vsr.FunAddress.Should().Be(5);
        }

        [TestMethod]
        public void ShouldReturnANullFunAddressIfTheAssetDataIsNotSet()
        {
            var vsr = new Vsr();

            _ = vsr.FunAddress.Should().Be(null);
        }
    }
}
