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
        /// The data to assert/stream
        /// </summary>
        private readonly byte[] data;

        /// <summary>
        /// The exception message if the deserialized data doesn't match the constant
        /// </summary>
        private readonly string errorMessage;

        /// <summary>
        /// Creates an instance of the Constant serializer
        /// </summary>
        /// <param name="data">The constant data to assert and write</param>
        /// <param name="errorMessage">The exception message if the deserialized data doesn't match the constant</param>
        public Constant(byte[] data, string errorMessage = "Invalid value")
        {
            this.data = data;
            this.errorMessage = errorMessage;
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
                throw new InvalidDataException(errorMessage);
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
