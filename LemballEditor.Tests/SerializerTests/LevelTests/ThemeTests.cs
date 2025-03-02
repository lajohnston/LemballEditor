using FluentAssertions;
using LemballEditor.Models;
using LemballEditor.Serializers.Level;

namespace LemballEditor.Tests.SerializerTests.LevelTests
{
    [TestClass]
    public sealed class ThemeTests
    {
        [TestMethod]
        public void Deserialize_ShouldThrowAnExceptionIfTheThemeIsNotValid()
        {
            ushort invalidValue = 4;
            using var stream = new MemoryStream(BitConverter.GetBytes(invalidValue));
            using var reader = new BinaryReader(stream);

            var act = () => new Theme().Deserialize(reader, ServiceFactory.CreateLevel(1, 1));
            _ = act.Should().Throw<InvalidDataException>().WithMessage($"Theme value should be between 0-3, {invalidValue} given");
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheCorrectUShortValueForTheGrassTheme()
        {
            var level = ServiceFactory.CreateLevel(1, 1);
            level.Theme = LevelTheme.Grass;

            ushort expectedValue = 0;
            var serializer = new Theme();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheCorrectUShortValueForTheLegoTheme()
        {
            var level = ServiceFactory.CreateLevel(1, 1);
            level.Theme = LevelTheme.Lego;

            ushort expectedValue = 1;
            var serializer = new Theme();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheCorrectUShortValueForTheSnowTheme()
        {
            var level = ServiceFactory.CreateLevel(1, 1);
            level.Theme = LevelTheme.Snow;

            ushort expectedValue = 2;
            var serializer = new Theme();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }

        [TestMethod]
        public void Serialize_ShouldWriteTheCorrectUShortValueForTheSpaceTheme()
        {
            var level = ServiceFactory.CreateLevel(1, 1);
            level.Theme = LevelTheme.Space;

            ushort expectedValue = 3;
            var serializer = new Theme();

            TestHelper.AssertWrite(level, serializer, expectedValue);
        }
    }
}
