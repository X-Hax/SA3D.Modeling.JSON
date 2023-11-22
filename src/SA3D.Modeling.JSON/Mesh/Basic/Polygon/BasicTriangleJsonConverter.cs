using SA3D.Modeling.Mesh.Basic.Polygon;
using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SA3D.Modeling.JSON.Mesh.Basic.Polygon
{
	/// <summary>
	/// Json converter for <see cref="BasicTriangle"/>
	/// </summary>
	public class BasicTriangleJsonConverter : JsonConverter<BasicTriangle>
	{
		/// <inheritdoc/>
		public override BasicTriangle Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if(reader.TokenType != JsonTokenType.StartArray)
			{
				throw new InvalidDataException("Expected an array for BasicTriangle!");
			}

			ushort[] indices = JsonSerializer.Deserialize<ushort[]>(ref reader, options)!;

			if(indices.Length < 3)
			{
				throw new InvalidDataException("BasicTriangle has too few indices! At least 3 needed!");
			}

			return new(indices[0], indices[1], indices[2]);
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, BasicTriangle value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			writer.WriteNumberValue(value.Index1);
			writer.WriteNumberValue(value.Index2);
			writer.WriteNumberValue(value.Index3);
			writer.WriteEndArray();
		}
	}
}
