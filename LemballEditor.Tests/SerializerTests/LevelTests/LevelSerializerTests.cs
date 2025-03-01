using FluentAssertions;
using LemballEditor.Models;

namespace LemballEditor.Tests.SerializerTests.LevelTests
{
    [TestClass]
    public sealed class LevelSerializerTests
    {
        private ILevel serializeAndDeserialize(ILevel level)
        {
            using var stream = new MemoryStream();
            using var reader = new BinaryReader(stream);

            // Serialize data
            using var writer = new BinaryWriter(stream);
            new Serializers.Level.LevelSerializer().Serialize(level, writer);

            // Deserialize the data
            var result = ModelFactory.LevelFactory(1, 1);
            _ = stream.Seek(0, SeekOrigin.Begin);
            _ = new Serializers.Level.LevelSerializer().Deserialize(reader, result);

            return result;
        }

        [TestMethod]
        public void ShouldGetAndSetUnknownA()
        {
            var level = ModelFactory.LevelFactory(1, 1);
            level.UnknownA = 10;

            var result = serializeAndDeserialize(level);
            _ = result.UnknownA.Should().Be(10);
        }

        [TestMethod]
        public void ShouldGetAndSetUnknownB()
        {
            var level = ModelFactory.LevelFactory(1, 1);
            level.UnknownB = 33425;

            var result = serializeAndDeserialize(level);
            _ = result.UnknownB.Should().Be(33425);
        }

        [TestMethod]
        public void ShouldGetAndSetTheNumberOfLemmings()
        {
            var level = ModelFactory.LevelFactory(1, 1);
            level.NumberOfLemmings = 2;

            var result = serializeAndDeserialize(level);
            _ = result.NumberOfLemmings.Should().Be(2);
        }

        [TestMethod]
        public void ShouldGetAndSetGrassTheme()
        {
            var level = ModelFactory.LevelFactory(1, 1);
            level.Theme = LevelTheme.Grass;

            var result = serializeAndDeserialize(level);
            _ = result.Theme.Should().Be(LevelTheme.Grass);
        }

        [TestMethod]
        public void ShouldGetAndSetLegoTheme()
        {
            var level = ModelFactory.LevelFactory(1, 1);
            level.Theme = LevelTheme.Lego;

            var result = serializeAndDeserialize(level);
            _ = result.Theme.Should().Be(LevelTheme.Lego);
        }

        [TestMethod]
        public void ShouldGetAndSetSnowTheme()
        {
            var level = ModelFactory.LevelFactory(1, 1);
            level.Theme = LevelTheme.Snow;

            var result = serializeAndDeserialize(level);
            _ = result.Theme.Should().Be(LevelTheme.Snow);
        }

        [TestMethod]
        public void ShouldGetAndSetSpaceTheme()
        {
            var level = ModelFactory.LevelFactory(1, 1);
            level.Theme = LevelTheme.Space;

            var result = serializeAndDeserialize(level);
            _ = result.Theme.Should().Be(LevelTheme.Space);
        }

        [TestMethod]
        public void ShouldGetAndSetTheTimeLimit()
        {
            var level = ModelFactory.LevelFactory(1, 1);
            level.TimeLimitInSeconds = 123;

            var result = serializeAndDeserialize(level);
            _ = result.TimeLimitInSeconds.Should().Be(123);
        }

        [TestMethod]
        public void ShouldGetAndSetAnInfiniteTimeLimit_WhenThereIsNoTimeLimit()
        {
            var level = ModelFactory.LevelFactory(1, 1);
            level.TimeLimitInSeconds = null;

            var result = serializeAndDeserialize(level);
            _ = result.TimeLimitInSeconds.Should().BeNull();
        }

        [TestMethod]
        public void ShouldGetAndSetTheFlagsRequiredIndicator()
        {
            var level = ModelFactory.LevelFactory(1, 1);
            level.FlagsRequired = 3;

            var result = serializeAndDeserialize(level);
            _ = result.FlagsRequired.Should().Be(3);
        }

        [TestMethod]
        public void ShouldGetAndSetTheLevelMap()
        {
            var level = ModelFactory.LevelFactory(1, 1);
            var map = level.Map;

            var resultMap = serializeAndDeserialize(level).Map;

            _ = resultMap.XTiles.Should().Be(map.XTiles);
            _ = resultMap.YTiles.Should().Be(map.YTiles);
        }
    }
}