using System.IO;
using System.Text;

namespace LemballEditor.Serializers
{
    public class DataBlock<TModel> : ISerializer<TModel>
    {
        private readonly string header;
        private readonly ISerializer<TModel> bodySerializer;

        /// <summary>
        /// Serializes/deserializes a data block
        /// </summary>
        public DataBlock(string header, ISerializer<TModel> bodySerializer)
        {
            this.header = header;
            this.bodySerializer = bodySerializer;
        }

        /// <summary>
        /// Deserializes a data block from the reader, expecting a specific header
        /// </summary>
        public TModel Deserialize(BinaryReader reader, TModel model)
        {
            // Validate header
            var header = Encoding.ASCII.GetString(reader.ReadBytes(this.header.Length));
            if (header != this.header)
            {
                var position = reader.BaseStream.Position - 4;
                throw new InvalidDataException($"Expected {this.header} header at address {position}");
            }

            // Skip size
            _ = reader.ReadInt32();

            // Deserialize body
            var result = this.bodySerializer.Deserialize(reader, model);
            _ = reader.ReadBytes(this.GetPaddingSize(reader.BaseStream.Position)); // skip padding bytes

            return result;
        }

        /// <summary>
        /// Writes the header, data size, serialized body to the writer and alignment padding to the stream.
        /// </summary>
        public void Serialize(TModel model, BinaryWriter writer)
        {
            // Header
            var startPosition = writer.BaseStream.Position;
            writer.Write(Encoding.ASCII.GetBytes(this.header));

            // Size (placeholder)
            var sizePointer = writer.BaseStream.Position;
            writer.Write(0);

            // Serialize block
            this.bodySerializer.Serialize(model, writer);

            // Set size
            var size = writer.BaseStream.Position - startPosition;
            var endPosition = writer.BaseStream.Position;
            writer.BaseStream.Position = sizePointer;
            writer.Write(size);
            writer.BaseStream.Position = endPosition;

            // Add padding
            writer.Write(new byte[this.GetPaddingSize(writer.BaseStream.Position)]);
        }

        /// <summary>
        /// Get the padding size required to align the stream position to a multiple of 4 bytes.
        /// </summary>
        /// <param name="position">Current stream position</param>
        /// <returns>The number of padding bytes required</returns>
        private int GetPaddingSize(long position)
        {
            var remainder = position % 4;
            return remainder == 0 ? 0 : 4 - (int)remainder;
        }
    }
}
