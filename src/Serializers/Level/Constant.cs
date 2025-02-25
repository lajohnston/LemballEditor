using System.IO;
using System.Linq;

namespace LemballEditor.Serializers.Level
{
    /// <summary>
    /// Serializes/Deserializes a stream of constant bytes to and from a stream
    /// </summary>
    public class Constant<TResult> : ISerializer<TResult>
    {
        /// <summary>
        /// The data to stream
        /// </summary>
        private readonly byte[] data;

        /// <summary>
        /// Creates an instance of the Constant serializer
        /// </summary>
        /// <param name="data">The constant data to assert and write</param>
        public Constant(byte[] data)
        {
            this.data = data;
        }

        /// <summary>
        /// Asserts the constant data can be read from the current position of the given reader
        /// </summary>
        /// <param name="reader">The reader to read from</param>
        /// <exception cref="InvalidDataException">If the bytes don't match</exception>
        public TResult Deserialize(BinaryReader reader, TResult result)
        {
            var headerBytes = reader.ReadBytes(data.Length);

            if (!headerBytes.SequenceEqual(data))
            {
                throw new InvalidDataException("Invalid value");
            }

            return result;
        }

        /// <summary>
        /// Write the constant data to the current position of the writer
        /// </summary>
        /// <param name="writer">The writer for writing to the stream</param>
        public void Serialize(TResult result, BinaryWriter writer)
        {
            writer.Write(data);
        }
    }
}
