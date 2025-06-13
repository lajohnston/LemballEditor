using System;
using System.IO;

namespace LemballEditor.Serializers
{
    public class SequenceSerializer<TModel> : ISerializer<TModel>
    {
        /// <summary>
        /// Ordered sequence of serializers
        /// </summary>
        private readonly ISerializer<TModel>[] serializers;

        public SequenceSerializer(ISerializer<TModel>[] serializers)
        {
            this.serializers = serializers;
        }

        /// <summary>
        /// Deserializes a model using the sequence of provided serializers.
        /// </summary>
        /// <param name="reader">BinaryReader instance from which to read the raw data</param>
        /// <param name="model">Model to which to apply the data</param>
        /// <returns>Resulting model</returns>
        public TModel Deserialize(BinaryReader reader, TModel model)
        {
            var accumulatedModel = model;

            foreach (var serializer in this.serializers)
            {
                accumulatedModel = serializer.Deserialize(reader, accumulatedModel);
            }

            return accumulatedModel;
        }

        /// <summary>
        /// Serializes a model using the sequence of provided serializers.
        /// </summary>
        /// <param name="model">The model to serialize</param>
        /// <param name="writer">The BinaryWriter to write the data to</param>
        public void Serialize(TModel model, BinaryWriter writer)
        {
            foreach (var serializer in this.serializers)
            {
                serializer.Serialize(model, writer);
            }
        }
    }
}
