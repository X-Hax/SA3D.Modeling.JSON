using SA3D.Modeling.Mesh.Basic.Polygon;
using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO;

namespace SA3D.Modeling.JSON.Mesh.Basic.Polygon
{
	/// <summary>
	/// Json converter for <see cref="BasicQuad"/>
	/// </summary>
	public class BasicQuadJsonConverter : JsonConverter<BasicQuad>
	{
		/// <inheritdoc/>
		public override BasicQuad Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			if(reader.TokenType != JsonTokenType.StartArray)
			{
				throw new InvalidDataException("Expected an array for BasicQuad!");
			}

			ushort[] indices = JsonSerializer.Deserialize<ushort[]>(ref reader, options)!;

			if(indices.Length < 4)
			{
				throw new InvalidDataException("BasicQuad has too few indices! At least 4 needed!");
			}

			return new(indices[0], indices[1], indices[2], indices[3]);
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, BasicQuad value, JsonSerializerOptions options)
		{
			writer.WriteStartArray();
			writer.WriteNumberValue(value.Index1);
			writer.WriteNumberValue(value.Index2);
			writer.WriteNumberValue(value.Index3);
			writer.WriteNumberValue(value.Index4);
			writer.WriteEndArray();
		}
	}
}
