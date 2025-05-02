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

        [TestMethod]
        public void ShouldGetAndSetTheFixedLevelCountForEachLevelGroup()
        {
            var vsr = new Vsr();
            vsr.SetFixedLevelCount(LevelGroupName.Fun, 1);
            vsr.SetFixedLevelCount(LevelGroupName.Tricky, 2);
            vsr.SetFixedLevelCount(LevelGroupName.Taxing, 3);
            vsr.SetFixedLevelCount(LevelGroupName.Mayhem, 4);
            vsr.SetFixedLevelCount(LevelGroupName.Network, 5);

            vsr.GetFixedLevelCount(LevelGroupName.Fun).Should().Be(1);
            vsr.GetFixedLevelCount(LevelGroupName.Tricky).Should().Be(2);
            vsr.GetFixedLevelCount(LevelGroupName.Taxing).Should().Be(3);
            vsr.GetFixedLevelCount(LevelGroupName.Mayhem).Should().Be(4);
            vsr.GetFixedLevelCount(LevelGroupName.Network).Should().Be(5);
        }

        [TestMethod]
        public void ShouldGetAndSetTheIdOfTheFirstLevel()
        {
            var vsr = new Vsr();
            vsr.FirstLevelFileId = 1234;
            _ = vsr.FirstLevelFileId.Should().Be(1234);
        }
    }
}
