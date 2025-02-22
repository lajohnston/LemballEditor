using LemballEditor.Models;
using System;
using System.IO;

namespace LemballEditor.Serializers.Level
{
    /// <summary>
    /// A value storing the number of flags required in the level selector. The value doesn't
    /// seem to be used by the level itself
    /// </summary>
    public class FlagsRequiredIndicator : ILevelSerializer
    {
        private static readonly ushort NO_FLAGS_REQUIRED = 7;

        public void Deserialize(ILevel level, BinaryReader reader)
        {
            var value = reader.ReadUInt16();

            if (value > byte.MaxValue)
            {
                throw new InvalidDataException($"FlagsRequiredIndicator out of byte range. {value} given");
            }

            try
            {
                level.FlagsRequired = (byte)(value == NO_FLAGS_REQUIRED ? 0 : value);
            }
            catch (ArgumentException error)
            {
                throw new InvalidDataException(error.Message);
            }
        }

        public void Serialize(ILevel level, BinaryWriter writer)
        {
            var value = level.FlagsRequired == 0 ? NO_FLAGS_REQUIRED : level.FlagsRequired;

            writer.Write(value);
        }
    }
}
