using LemballEditor.Models;
using System;
using System.IO;

namespace LemballEditor.Serializers.Level
{
    /// <summary>
    /// Serializes and deserializes the UnknownA ushort
    /// A value of zero crashes the game, indicating it is read for something, but changing it to anything
    /// above 1 doesn't seem to have any effect on the level.
    ///
    /// The value is usually 9 or 10 with the exception of Taxing Level_15 (6) and Mayhem Level_02 (7).
    ///
    /// </summary>
    public class UnknownA : ILevelSerializer
    {
        public void Deserialize(ILevel level, BinaryReader reader)
        {
            var value = reader.ReadUInt16();
            try
            {
                level.UnknownA = value;
            }
            catch (ArgumentException error)
            {
                throw new InvalidDataException(error.Message);
            }
        }

        public void Serialize(ILevel level, BinaryWriter writer)
        {
            writer?.Write(level.UnknownA);
        }
    }
}
