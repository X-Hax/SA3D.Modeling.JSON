using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Chunk;
using SA3D.Modeling.Mesh.Chunk.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Chunk
{
	/// <summary>
	/// Json converter for <see cref="VertexChunk"/>
	/// </summary>
	public class VertexChunkJsonConverter : SimpleJsonObjectConverter<VertexChunk>
	{
		private const string _type = nameof(VertexChunk.Type);
		private const string _attributes = nameof(VertexChunk.Attributes);
		private const string _indexOffset = nameof(VertexChunk.IndexOffset);
		private const string _vertices = nameof(VertexChunk.Vertices);

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _type, new(PropertyTokenType.String, null) },
			{ _attributes, new(PropertyTokenType.String, (byte)0) },
			{ _indexOffset, new(PropertyTokenType.Number, (ushort)0u) },
			{ _vertices, new(PropertyTokenType.Array, null) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _type:
					return JsonSerializer.Deserialize<VertexChunkType>(ref reader, options);
				case _attributes:
					return reader.GetString()!.HexToByte("Vertex chunk attributes");
				case _indexOffset:
					return reader.GetUInt16();
				case _vertices:
					return JsonSerializer.Deserialize<ChunkVertex[]>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override VertexChunk Create(ReadOnlyDictionary<string, object?> values)
		{
			VertexChunkType type = (VertexChunkType?)values[_type]
				?? throw new InvalidDataException($"Vertex chunk requires \"{_type}\" property.");

			ChunkVertex[] vertices = (ChunkVertex[]?)values[_vertices]
				?? throw new InvalidDataException($"Vertex chunk requires \"{_vertices}\" property.");

			return new(type, (byte)values[_attributes]!, (ushort)values[_indexOffset]!, vertices);
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, VertexChunk value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_type);
			JsonSerializer.Serialize(writer, value.Type, options);

			if(value.Attributes != 0)
			{
				writer.WriteString(_attributes, value.Attributes.ToString("X", CultureInfo.InvariantCulture));
			}

			if(value.IndexOffset != 0)
			{
				writer.WriteNumber(_indexOffset, value.IndexOffset);
			}

			writer.WritePropertyName(_vertices);
			JsonSerializer.Serialize(writer, value.Vertices, options);
		}
	}
}
