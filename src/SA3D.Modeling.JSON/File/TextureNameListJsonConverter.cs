using SA3D.Common.Lookup;
using SA3D.Modeling.JSON.JsonBase;
using SA3D.Texturing.Texname;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.File
{
    /// <summary>
    /// Json converter for <see cref="TextureNameList"/>
    /// </summary>
    public class TextureNameListJsonConverter : SimpleJsonObjectConverter<TextureNameList>
    {
        private const string _label = nameof(TextureNameList.Label);
        private const string _textureNames = nameof(TextureNameList.TextureNames);


        /// <inheritdoc/>
        public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
        {
            { _label, new(PropertyTokenType.String, string.Empty) },
            { _textureNames, new(PropertyTokenType.Object | PropertyTokenType.Array, null) },
        });

        /// <inheritdoc/>
        protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
        {
            switch(propertyName)
            {
                case _label:
                    return reader.GetString();
                case _textureNames:
                    return JsonSerializer.Deserialize<LabeledArray<TextureName>>(ref reader, options);
                default:
                    throw new InvalidPropertyException();
            }
        }

        /// <inheritdoc/>
        protected override TextureNameList Create(ReadOnlyDictionary<string, object?> values)
        {
            string label = (string)values[_label]!;
            LabeledArray<TextureName> textureNames = new(0);

            if(values[_textureNames] is LabeledArray<TextureName> readTextureNames)
            {
                textureNames = readTextureNames;
            }

            return new(label, textureNames);
        }

        /// <inheritdoc/>
        protected override void WriteValues(Utf8JsonWriter writer, TextureNameList value, JsonSerializerOptions options)
        {
            writer.WriteString(_label, value.Label);

            writer.WritePropertyName(_textureNames);
            JsonSerializer.Serialize(writer, value.TextureNames, options);
        }
    }
}
