using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Chunk.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Chunk.Structs
{
	/// <summary>
	/// Json converter for <see cref="ChunkVolumeStrip"/>
	/// </summary>
	public class ChunkVolumeStripJsonConverter : SimpleJsonObjectConverter<ChunkVolumeStrip>
	{
		private const string _reversed = nameof(ChunkVolumeStrip.Reversed);
		private const string _indices = nameof(ChunkVolumeStrip.Indices);
		private const string _triangleAttributes = nameof(ChunkVolumeStrip.TriangleAttributes);


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _reversed, new(PropertyTokenType.Bool, false) },
			{ _indices, new(PropertyTokenType.Array, null) },
			{ _triangleAttributes, new(PropertyTokenType.Array, null) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _reversed:
					return reader.GetBoolean();
				case _indices:
					return JsonSerializer.Deserialize<ushort[]>(ref reader, options);
				case _triangleAttributes:
					return JsonSerializer.Deserialize<string[][]>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override ChunkVolumeStrip Create(ReadOnlyDictionary<string, object?> values)
		{
			ushort[] indices = (ushort[]?)values[_indices]
				?? throw new InvalidDataException("Chunk volume strip requires indices!");

			ChunkVolumeStrip result = new(indices, (bool)values[_reversed]!);

			if(values[_triangleAttributes] is string[][] attributes)
			{
				for(int i = 0; i < attributes.Length && i < result.TriangleAttributes.Length; i++)
				{
					string[] attributeArray = attributes[i];
					for(int j = 0; j < attributeArray.Length && j < 3; j++)
					{
						result.TriangleAttributes[i, j] = attributeArray[j].HexToUShort("Chunk volume strip attributes");
					}
				}
			}

			return result;
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, ChunkVolumeStrip value, JsonSerializerOptions options)
		{
			if(value.Reversed)
			{
				writer.WriteBoolean(_reversed, value.Reversed);
			}

			writer.WritePropertyName(_indices);
			JsonSerializer.Serialize(writer, value.Indices, options);

			bool hasAttributes = false;
			for(int i = 0; i < value.TriangleAttributes.Length; i++)
			{
				for(int j = 0; j < 3; j++)
				{
					if(value.TriangleAttributes[i, j] != 0)
					{
						hasAttributes = true;
						break;
					}
				}

				if(hasAttributes)
				{
					break;
				}
			}

			if(hasAttributes)
			{
				string[][] attributes = new string[value.TriangleAttributes.Length][];

				for(int i = 0; i < value.TriangleAttributes.Length; i++)
				{
					string[] attributeArray = new string[3];
					for(int j = 0; j < 3; j++)
					{
						attributeArray[j] = value.TriangleAttributes[i, j].ToString("X", CultureInfo.InvariantCulture);
					}
				}

				writer.WritePropertyName(_triangleAttributes);
				JsonSerializer.Serialize(writer, attributes, options);
			}
		}
	}
}
