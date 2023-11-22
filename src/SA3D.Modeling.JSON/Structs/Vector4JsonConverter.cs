using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SA3D.Modeling.JSON.Structs
{
	/// <summary>
	/// Json converter for <see cref="Vector4"/>.
	/// </summary>
	public class Vector4JsonConverter : JsonConverter<Vector4>
	{
		/// <inheritdoc/>
		public override Vector4 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if(reader.TokenType != JsonTokenType.String)
			{
				throw new JsonException("Expected a string for Vector4!");
			}

			string[] values = reader.GetString()!.Split(' ');
			return new(
				float.Parse(values[0]),
				float.Parse(values[1]),
				float.Parse(values[2]),
				float.Parse(values[3]));
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, Vector4 value, JsonSerializerOptions options)
		{
			writer.WriteStringValue($"{value.X:F4} {value.Y:F4} {value.Z:F4} {value.W:F4}");
		}
	}
}
