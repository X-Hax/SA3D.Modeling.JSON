using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Chunk.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Chunk.Structs
{
	/// <summary>
	/// Json converter for <see cref="ChunkVolumeQuad"/>
	/// </summary>
	public class ChunkVolumeQuadJsonConverter : SimpleJsonObjectConverter<ChunkVolumeQuad>
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
						result[i] = attributes[i].HexToUShort("Chunk volume quad attributes");
					}

					return result;
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override ChunkVolumeQuad Create(ReadOnlyDictionary<string, object?> values)
		{
			ushort[] indices = (ushort[]?)values[_indices]
				?? throw new InvalidDataException("Chunk volume quad requires indices!");

			if(indices.Length < 4)
			{
				throw new InvalidDataException("Chunk volume quad requires 4 indices!");
			}

			ChunkVolumeQuad result = new(indices[0], indices[1], indices[2], indices[3]);

			if(values[_attributes] is ushort[] attributes)
			{
				result.Attribute1 = attributes[0];
				result.Attribute2 = attributes[1];
				result.Attribute3 = attributes[2];
			}

			return result;
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, ChunkVolumeQuad value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_indices);
			JsonSerializer.Serialize(writer, new ushort[] { value.Index1, value.Index2, value.Index3, value.Index4 }, options);

			if(value.Attribute1 != 0 || value.Attribute2 != 0 || value.Attribute3 != 0)
			{
				writer.WritePropertyName(_attributes);
				JsonSerializer.Serialize(writer, new ushort[] { value.Attribute1, value.Attribute2, value.Attribute3 }, options);
			}
		}
	}
}
