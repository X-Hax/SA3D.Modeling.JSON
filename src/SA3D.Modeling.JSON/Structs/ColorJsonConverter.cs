using SA3D.Modeling.Structs;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SA3D.Modeling.JSON.Structs
{
	/// <summary>
	/// Json converter for <see cref="Color"/>.
	/// </summary>
	public class ColorJsonConverter : JsonConverter<Color>
	{
		/// <inheritdoc/>
		public override Color Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if(reader.TokenType != JsonTokenType.String)
			{
				throw new JsonException("Expected a string for Color!");
			}

			Color result = default;
			result.Hex = reader.GetString()!;
			return result;
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
		{
			writer.WriteStringValue(value.Hex);
		}
	}
}
