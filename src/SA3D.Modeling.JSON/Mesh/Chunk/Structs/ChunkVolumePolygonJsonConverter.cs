using System;
using System.Text.Json.Serialization;
using System.Text.Json;
using SA3D.Modeling.Mesh.Chunk.Structs;

namespace SA3D.Modeling.JSON.Mesh.Chunk.Structs
{
	/// <summary>
	/// Writes basic polygons.
	/// </summary>
	public class ChunkVolumePolygonJsonConverter : JsonConverter<IChunkVolumePolygon>
	{
		/// <inheritdoc/>
		public override IChunkVolumePolygon? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		{
			throw new NotSupportedException("Cannot read interfaces!");
		}

		/// <inheritdoc/>
		public override void Write(Utf8JsonWriter writer, IChunkVolumePolygon value, JsonSerializerOptions options)
		{
			switch(value)
			{
				case ChunkVolumeStrip stripPolygon:
					JsonSerializer.Serialize(writer, stripPolygon, options);
					break;
				case ChunkVolumeTriangle triangle:
					JsonSerializer.Serialize(writer, triangle, options);
					break;
				case ChunkVolumeQuad quad:
					JsonSerializer.Serialize(writer, quad, options);
					break;
				default:
					throw new NotSupportedException($"Unknown IChunkVolumePolygon type: {value.GetType()}");
			}
		}
	}
}
