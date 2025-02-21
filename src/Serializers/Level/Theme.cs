using LemballEditor.Models;
using System.IO;

namespace LemballEditor.Serializers.Level
{
    public class Theme : ILevelSerializer
    {
        public void Deserialize(ILevel level, BinaryReader reader)
        {
            var value = reader.ReadUInt16();

            switch (value)
            {
                case 0:
                    level.Theme = LevelTheme.Grass;
                    break;
                case 1:
                    level.Theme = LevelTheme.Lego;
                    break;
                case 2:
                    level.Theme = LevelTheme.Snow;
                    break;
                case 3:
                    level.Theme = LevelTheme.Space;
                    break;
                default:
                    throw new InvalidDataException($"Theme value should be between 0-3, {value} given");
            }
        }

        public void Serialize(ILevel level, BinaryWriter writer)
        {
            switch (level.Theme)
            {
                case LevelTheme.Grass:
                    writer?.Write((ushort)0);
                    break;
                case LevelTheme.Lego:
                    writer?.Write((ushort)1);
                    break;
                case LevelTheme.Snow:
                    writer?.Write((ushort)2);
                    break;
                case LevelTheme.Space:
                    writer?.Write((ushort)3);
                    break;
            }
        }
    }
}
