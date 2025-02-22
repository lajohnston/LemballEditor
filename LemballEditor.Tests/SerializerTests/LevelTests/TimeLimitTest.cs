using LemballEditor.Models;

namespace LemballEditor.Tests.SerializerTests.LevelTests
{
    [TestClass]
    public class TimeLimitTest
    {
        public void Deserialize_ShouldSetTimeLimitToNull_WhenValueIs600()
        {
            ushort value = 600;
            using var stream = new MemoryStream(BitConverter.GetBytes(value));
            using var reader = new BinaryReader(stream);

            var level = new Level();


            //new TimeLimit().Deserialize(level, reader)
        }

    }
}
