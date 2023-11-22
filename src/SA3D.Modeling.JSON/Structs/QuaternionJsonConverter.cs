using System;
using System.Numerics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SA3D.Modeling.JSON.Structs
{
	/// <summary>
	/// Json converter for <see cref="Quaternion"/>.
	/// </summary>
	public class QuaternionJsonConverter : JsonConverter<Quaternion>
	{
		/// <inheritdoc/>
		public override Quaternion Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if(reader.TokenType != JsonTokenType.String)
			{
				throw new JsonException("Expected a string for Quaternion!");
			}

			string[] values = reader.GetString()!.Split(' ');
			return new(
				float.Parse(values[0]),
				float.Parse(values[1]),
				float.Parse(values[2]),
				float.Parse(values[3]));
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, Quaternion value, JsonSerializerOptions options)
		{
			writer.WriteStringValue($"{value.X:F4} {value.Y:F4} {value.Z:F4} {value.W:F4}");
		}
	}
}
