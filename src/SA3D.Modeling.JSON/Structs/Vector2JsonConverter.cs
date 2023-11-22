using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SA3D.Modeling.JSON.Structs
{
	/// <summary>
	/// Json converter for <see cref="Vector2"/>.
	/// </summary>
	public class Vector2JsonConverter : JsonConverter<Vector2>
	{
		/// <inheritdoc/>
		public override Vector2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if(reader.TokenType != JsonTokenType.String)
			{
				throw new JsonException("Expected a string for Vector2!");
			}

			string[] values = reader.GetString()!.Split(' ');
			return new(
				float.Parse(values[0]),
				float.Parse(values[1]));
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, Vector2 value, JsonSerializerOptions options)
		{
			writer.WriteStringValue($"{value.X:F4} {value.Y:F4}");
		}
	}
}
