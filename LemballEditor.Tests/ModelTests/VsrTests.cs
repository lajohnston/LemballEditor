using FluentAssertions;
using LemballEditor.Models;

namespace LemballEditor.Tests.ModelTests
{
    [TestClass]
    public class VsrTests
    {
        [TestMethod]
        public void ShouldStoreTheDirectoryPointers()
        {
            var vsr = ServiceFactory.CreateVsr();

            vsr.FunDirectoryPointer = 1;
            vsr.TrickyDirectoryPointer = 2;
            vsr.TaxingDirectoryPointer = 3;
            vsr.MayhemDirectoryPointer = 4;
            vsr.NetworkDirectoryPointer = 5;

            _ = vsr.FunDirectoryPointer.Should().Be(1);
            _ = vsr.TrickyDirectoryPointer.Should().Be(2);
            _ = vsr.TaxingDirectoryPointer.Should().Be(3);
            _ = vsr.MayhemDirectoryPointer.Should().Be(4);
            _ = vsr.NetworkDirectoryPointer.Should().Be(5);
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
        public void ShouldStoreALevelPack()
        {
            var levelPack = new LevelPack();
            var vsr = new Vsr { LevelPack = levelPack };

            _ = vsr.LevelPack.Should().Be(levelPack);
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
