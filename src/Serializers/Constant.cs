using System.IO;
using System.Linq;

namespace LemballEditor.Serializers
{
    /// <summary>
    /// Serializes/Deserializes a stream of constant bytes to and from a stream
    /// </summary>
    public class Constant<TResult> : ISerializer<TResult>
    {
        /// <summary>
        /// The data to assert/stream
        /// </summary>
        private readonly byte[] data;

        /// <summary>
        /// A description of the data for use in error messaging
        /// </summary>
        private readonly string dataDescription;

        /// <summary>
        /// Creates an instance of the Constant serializer
        /// </summary>
        /// <param name="data">The constant data to assert and write</param>
        /// <param name="dataDescription">A brief description of the data</param>
        public Constant(byte[] data, string dataDescription)
        {
            this.data = data;
            this.dataDescription = dataDescription;
        }

        /// <summary>
        /// Asserts the constant data can be read from the current position of the given reader
        /// </summary>
        /// <param name="reader">The reader to read from</param>
        /// <exception cref="InvalidDataException">If the bytes don't match</exception>
        public TResult Deserialize(BinaryReader reader, TResult result)
        {
            var headerBytes = reader.ReadBytes(this.data.Length);

            if (!headerBytes.SequenceEqual(this.data))
            {
                var invalidPosition = reader.BaseStream.Position - this.data.Length;
                throw new InvalidDataException($"Unexpected {this.dataDescription} at position {invalidPosition}");
            }

            return result;
        }

        /// <summary>
        /// Write the constant data to the current position of the writer
        /// </summary>
        /// <param name="writer">The writer for writing to the stream</param>
        public void Serialize(TResult result, BinaryWriter writer)
        {
            writer.Write(this.data);
        }
    }
}
