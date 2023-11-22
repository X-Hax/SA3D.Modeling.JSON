using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Chunk.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Chunk.Structs
{
	/// <summary>
	/// Json converter for <see cref="ChunkVolumeTriangle"/>
	/// </summary>
	public class ChunkVolumeTriangleJsonConverter : SimpleJsonObjectConverter<ChunkVolumeTriangle>
	{
		private const string _indices = "Indices";
		private const string _attributes = "Attributes";

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _indices, new(PropertyTokenType.Array, null) },
			{ _attributes, new(PropertyTokenType.Array, null) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _indices:
					return JsonSerializer.Deserialize<ushort[]>(ref reader, options);
				case _attributes:
					string[] attributes = JsonSerializer.Deserialize<string[]>(ref reader, options)!;

					ushort[] result = new ushort[3];
					for(int i = 0; i < attributes.Length && i < 3; i++)
					{
						result[i] = attributes[i].HexToUShort("Chunk voluem triangle attributes");
					}

					return result;
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override ChunkVolumeTriangle Create(ReadOnlyDictionary<string, object?> values)
		{
			ushort[] indices = (ushort[]?)values[_indices]
				?? throw new InvalidDataException("Chunk volume triangle requires indices!");

			if(indices.Length < 3)
			{
				throw new InvalidDataException("Chunk volume triangle requires 3 indices!");
			}

			ChunkVolumeTriangle result = new(indices[0], indices[1], indices[2]);

			if(values[_attributes] is ushort[] attributes)
			{
				result.Attribute1 = attributes[0];
				result.Attribute2 = attributes[1];
				result.Attribute3 = attributes[2];
			}

			return result;
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, ChunkVolumeTriangle value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_indices);
			JsonSerializer.Serialize(writer, new ushort[] { value.Index1, value.Index2, value.Index3 }, options);

			if(value.Attribute1 != 0 || value.Attribute2 != 0 || value.Attribute3 != 0)
			{
				writer.WritePropertyName(_attributes);
				JsonSerializer.Serialize(writer, new ushort[] { value.Attribute1, value.Attribute2, value.Attribute3 }, options);
			}
		}
	}
}
