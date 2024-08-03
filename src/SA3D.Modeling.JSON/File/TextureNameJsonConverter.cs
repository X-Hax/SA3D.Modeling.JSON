using SA3D.Modeling.JSON.JsonBase;
using SA3D.Texturing.Texname;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Globalization;

namespace SA3D.Modeling.JSON.File
{
    /// <summary>
    /// Json converter for <see cref="TextureName"/>
    /// </summary>
    public class TextureNameJsonConverter : SimpleJsonObjectConverter<TextureName>
    {
        private const string _name = nameof(TextureName.Name);
        private const string _attributes = nameof(TextureName.Attributes);
        private const string _textureAddress = nameof(TextureName.TextureAddress);


        /// <inheritdoc/>
        public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
        {
            { _name, new(PropertyTokenType.String, null) },
            { _attributes, new(PropertyTokenType.String, 0u) },
            { _textureAddress, new(PropertyTokenType.String, 0u) },
        });

        /// <inheritdoc/>
        protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
        {
            switch(propertyName)
            {
                case _name:
                    return reader.GetString();
                case _attributes:
                    return reader.GetString()!.HexToUInt("TextureName Attributes");
                case _textureAddress:
                    return reader.GetString()!.HexToUInt("TextureName Texture Address");
                default:
                    throw new InvalidPropertyException();
            }
        }

        /// <inheritdoc/>
        protected override TextureName Create(ReadOnlyDictionary<string, object?> values)
        {
            string? name = (string?)values[_name];
            uint attributes = (uint)values[_attributes]!;
            uint textureAddress = (uint)values[_textureAddress]!;

            return new(name, attributes, textureAddress);
        }

        /// <inheritdoc/>
        protected override void WriteValues(Utf8JsonWriter writer, TextureName value, JsonSerializerOptions options)
        {
            writer.WriteString(_name, value.Name);

            if(value.Attributes != 0)
            {
                writer.WriteString(_attributes, value.Attributes.ToString("X", CultureInfo.InvariantCulture));
            }

            if(value.TextureAddress != 0)
            {
                writer.WriteString(_textureAddress, value.TextureAddress.ToString("X", CultureInfo.InvariantCulture));
            }
        }
    }
}
