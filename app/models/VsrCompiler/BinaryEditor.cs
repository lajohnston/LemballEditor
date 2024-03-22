using System;
using System.IO;
using System.IO.Compression;

namespace VsrCompiler
{
    public class BinaryEditor
    {
        /// <summary>
        /// The binary writer
        /// </summary>
        private readonly BinaryWriter binary;

        /// <summary>
        /// Create a new BinaryEditor with a specified memory stream
        /// </summary>
        /// <param name="outputStream"></param>
        public BinaryEditor(MemoryStream outputStream)
        {
            binary = new BinaryWriter(outputStream);
        }


        /// <summary>
        /// Create a new BinaryEditor
        /// </summary>
        /*
        public BinaryEditor()
            : this(0)
        {

        }
        */

        /*
        /// <summary>
        /// Create a new BinaryEditor with a specified starting size
        /// </summary>
        /// <param name="size"></param>
        public BinaryEditor(int size)
        {
            binary = new BinaryWriter(new MemoryStream());
        }
        */

        /// <summary>
        /// Append a byte array to the stream
        /// </summary>
        /// <param name="byteArray">The byte array to append</param>
        public void Append(byte[] byteArray)
        {
            binary.Write(byteArray, 0, byteArray.Length);
        }

        /// <summary>
        /// Appends a file to the binary
        /// </summary>
        /// <param name="path">The file path</param>
        /// <param name="startAddr">The start address to append</param>
        /// <param name="length">The length of data to append</param>
        public void AppendFile(string path, int startAddr, int length)
        {
            if (length > 0)
            {
                // Create array to hold level data
                byte[] fileData = new byte[length];

                // Load file data into array
                using (FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    _ = file.Read(fileData, Math.Max(startAddr, 0), fileData.Length);
                }

                // Append data to binary
                binary.Write(fileData, 0, fileData.Length);
            }
        }

        /// <summary>
        /// Appends a byte to the binary
        /// </summary>
        /// <param name="value">The byte value to append</param>
        public void Append(byte value)
        {
            binary.Write(value);
        }

        /// <summary>
        /// Appends a short to the binary
        /// </summary>
        /// <param name="value">The short value to append</param>
        public void Append(short value)
        {
            binary.Write(value);
        }

        /// <summary>
        /// Appends a 16-bit unsigned short value to the binary
        /// </summary>
        /// <param name="value">The unsigned short (16-bit) value to add</param>
        public void Append(ushort value)
        {
            binary.Write(value);
        }

        /// <summary>
        /// Appends a 32-bit signed integer to the binary
        /// </summary>
        /// <param name="value">The 32-bit signed integer to append</param>
        public void Append(int value)
        {
            binary.Write(value);
        }

        /// <summary>
        /// Appends a 32-bit unsigned integer to the binary
        /// </summary>
        /// <param name="value">The 32-bit unsigned integer to append</param>
        public void Append(uint value)
        {
            binary.Write(value);
        }

        /// <summary>
        /// Appends a 64-bit signed long value to the binary
        /// </summary>
        /// <param name="value">The 64-bit signed long value to append</param>
        public void Append(long value)
        {
            binary.Write(value);
        }

        /// <summary>
        /// Appends text to the binary
        /// </summary>
        /// <param name="text">The text to append</param>
        public void Append(string text)
        {
            binary.Write(text.ToCharArray());
        }

        /// <summary>
        /// Sets the value of a 4-byte address pointer
        /// </summary>
        /// <param name="pointerAddress">The address of the pointer</param>
        /// <param name="value">The value to set at the pointer</param>
        public void Set(uint pointerAddress, uint value)
        {
            // Go to address pointer
            _ = binary.BaseStream.Seek(pointerAddress, SeekOrigin.Begin);

            // Set 4-byte value at address pointer
            binary.Write(value);

            // Return to end of stream
            _ = binary.Seek(0, SeekOrigin.End);
        }

        /// <summary>
        /// Closes the binary
        /// </summary>
        public void Dispose()
        {
            binary.BaseStream.Dispose();
        }

        /// <summary>
        /// Returns the current size of the binary
        /// </summary>
        /// <returns>The size of the binary</returns>
        public uint getSize()
        {
            return (uint)binary.BaseStream.Position;
        }

        /// <summary>
        /// Saves the binary to a file
        /// </summary>
        /// <param name="path">The path to save</param>
        public void SaveToFile(string path)
        {
            using (FileStream file = new FileStream(path, FileMode.Create))
            {
                MemoryStream memStream = (MemoryStream)binary.BaseStream;
                memStream.WriteTo(file);
            }
        }

        /// <summary>
        /// Returns a the binary data as a byte array containing GZip compressed data
        /// </summary>
        /// <returns>The byte array containing GZip compressed data</returns>
        public byte[] GetCompressedData()
        {
            // Copy data into a byte array
            MemoryStream originalStream = (MemoryStream)binary.BaseStream;
            byte[] data = originalStream.ToArray();
            return Compress(data);
        }

        /// <summary>
        /// Compresses the data using GZip and stores the compressed data in the given byte array
        /// </summary>
        /// <param name="data">The data to compress</param>
        /// <returns>Compressed data in a byte array</returns>
        public static byte[] Compress(byte[] data)
        {
            // Prepare compression
            using (MemoryStream compressedStream = new MemoryStream())
            {
                GZipStream gz = new GZipStream(compressedStream, CompressionMode.Compress);

                // Compress bytes and write to memory stream
                gz.Write(data, 0, data.Length);

                // Writes any buffered data to the stream
                gz.Close();

                // Store compressed data in 'data' parameter array
                return compressedStream.ToArray();
            }
        }

        /// <summary>
        /// Decompress a byte array. Stores the decompressed data in the same array.
        /// </summary>
        /// <param name="data">The byte array to decompress</param>
        public static void Decompress(ref byte[] data, int uncompressedSize)
        {
            // Prepare compression stream
            using (MemoryStream stream = new MemoryStream(data))
            {
                using (GZipStream gz = new GZipStream(stream, CompressionMode.Decompress))
                {
                    // Create byte array to store decompressed data
                    byte[] decompressedData = new byte[uncompressedSize];

                    _ = gz.Read(decompressedData, 0, uncompressedSize);

                    gz.Close();

                    // Return decompressed data
                    data = decompressedData;
                }
            }
        }



        /// <summary>
        /// Returns the binary data as a byte array
        /// </summary>
        /// <returns></returns>
        public byte[] GetData()
        {
            MemoryStream memStream = (MemoryStream)binary.BaseStream;
            return memStream.ToArray();
        }

        /// <summary>
        /// Returns 
        /// </summary>
        /// <returns></returns>
        public MemoryStream getStream()
        {
            return (MemoryStream)binary.BaseStream;
        }

        /// <summary>
        /// Pads the binary to four bytes
        /// </summary>
        public void Pad()
        {
            // Number of bytes to pad
            uint padBytes = 4 - (getSize() % 4);

            // Pad bytes
            if (padBytes != 4)
            {
                for (int i = 0; i < padBytes; i++)
                {
                    binary.Write((byte)0);
                }
            }
        }
    }
}