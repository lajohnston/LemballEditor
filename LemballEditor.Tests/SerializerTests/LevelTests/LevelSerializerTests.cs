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
            var result = new Level();
            _ = stream.Seek(0, SeekOrigin.Begin);
            new Serializers.Level.LevelSerializer().Deserialize(result, reader);

            return result;
        }

        [TestMethod]
        public void ShouldGetAndSetUnknownA()
        {
            var level = new Level
            {
                UnknownA = 10
            };

            var result = serializeAndDeserialize(level);
            _ = result.UnknownA.Should().Be(10);
        }

        [TestMethod]
        public void ShouldGetAndSetTheNumberOfLemmings()
        {
            var level = new Level
            {
                NumberOfLemmings = 2
            };

            var result = serializeAndDeserialize(level);
            _ = result.NumberOfLemmings.Should().Be(2);
        }

        [TestMethod]
        public void ShouldGetAndSetGrassTheme()
        {
            var level = new Level
            {
                Theme = LevelTheme.Grass
            };

            var result = serializeAndDeserialize(level);
            _ = result.Theme.Should().Be(LevelTheme.Grass);
        }

        [TestMethod]
        public void ShouldGetAndSetLegoTheme()
        {
            var level = new Level()
            {
                Theme = LevelTheme.Lego
            };

            var result = serializeAndDeserialize(level);
            _ = result.Theme.Should().Be(LevelTheme.Lego);
        }

        [TestMethod]
        public void ShouldGetAndSetSnowTheme()
        {
            var level = new Level()
            {
                Theme = LevelTheme.Snow
            };

            var result = serializeAndDeserialize(level);
            _ = result.Theme.Should().Be(LevelTheme.Snow);
        }

        [TestMethod]
        public void ShouldGetAndSetSpaceTheme()
        {
            var level = new Level()
            {
                Theme = LevelTheme.Space
            };

            var result = serializeAndDeserialize(level);
            _ = result.Theme.Should().Be(LevelTheme.Space);
        }

        [TestMethod]
        public void ShouldGetAndSetTheTimeLimit()
        {
            var level = new Level()
            {
                TimeLimitInSeconds = 123
            };

            var result = serializeAndDeserialize(level);
            _ = result.TimeLimitInSeconds.Should().Be(123);
        }

        [TestMethod]
        public void ShouldGetAndSetAnInfiniteTimeLimit_WhenThereIsNoTimeLimit()
        {
            var level = new Level()
            {
                TimeLimitInSeconds = null
            };

            var result = serializeAndDeserialize(level);
            _ = result.TimeLimitInSeconds.Should().BeNull();
        }

        [TestMethod]
        public void ShouldGetAndSetTheFlagsRequiredIndicator()
        {
            var level = new Level()
            {
                FlagsRequired = 3
            };

            var result = serializeAndDeserialize(level);
            _ = result.FlagsRequired.Should().Be(3);
        }
    }
}