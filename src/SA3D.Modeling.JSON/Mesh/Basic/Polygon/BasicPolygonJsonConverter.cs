using SA3D.Modeling.Mesh.Basic.Polygon;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SA3D.Modeling.JSON.Mesh.Basic.Polygon
{
	/// <summary>
	/// Writes basic polygons.
	/// </summary>
	public class BasicPolygonJsonConverter : JsonConverter<IBasicPolygon>
	{
		/// <inheritdoc/>
		public override IBasicPolygon? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			throw new NotSupportedException("Cannot read interfaces!");
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, IBasicPolygon value, JsonSerializerOptions options)
		{
			switch(value)
			{
				case BasicMultiPolygon multiPolygon:
					JsonSerializer.Serialize(writer, multiPolygon, options);
					break;
				case BasicTriangle triangle:
					JsonSerializer.Serialize(writer, triangle, options);
					break;
				case BasicQuad quad:
					JsonSerializer.Serialize(writer, quad, options);
					break;
				default:
					throw new NotSupportedException($"Unknown IBasicPolygon type: {value.GetType()}");
			}
		}
	}
}
