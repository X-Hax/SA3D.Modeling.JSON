using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Chunk.Structs;
using SA3D.Modeling.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Numerics;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Chunk.Structs
{
	/// <summary>
	/// Json converter for <see cref="ChunkCorner"/>
	/// </summary>
	public class ChunkCornerJsonConverter : SimpleJsonObjectConverter<ChunkCorner>
	{
		private const string _index = nameof(ChunkCorner.Index);
		private const string _texcoord = nameof(ChunkCorner.Texcoord);
		private const string _texcoord2 = nameof(ChunkCorner.Texcoord2);
		private const string _normal = nameof(ChunkCorner.Normal);
		private const string _color = nameof(ChunkCorner.Color);
		private const string _attributes1 = nameof(ChunkCorner.Attributes1);
		private const string _attributes2 = nameof(ChunkCorner.Attributes2);
		private const string _attributes3 = nameof(ChunkCorner.Attributes3);


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>() {
			{ _index, new(PropertyTokenType.Number, ChunkCorner.DefaultValues.Index) },
			{ _texcoord, new(PropertyTokenType.String, ChunkCorner.DefaultValues.Texcoord) },
			{ _texcoord2, new(PropertyTokenType.String, ChunkCorner.DefaultValues.Texcoord2) },
			{ _normal, new(PropertyTokenType.String, ChunkCorner.DefaultValues.Normal ) },
			{ _color, new(PropertyTokenType.String, ChunkCorner.DefaultValues.Color) },
			{ _attributes1, new(PropertyTokenType.String, ChunkCorner.DefaultValues.Attributes1) },
			{ _attributes2, new(PropertyTokenType.String, ChunkCorner.DefaultValues.Attributes2) },
			{ _attributes3, new(PropertyTokenType.String, ChunkCorner.DefaultValues.Attributes3) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _index:
					return reader.GetUInt16();
				case _texcoord:
				case _texcoord2:
					return JsonSerializer.Deserialize<Vector2>(ref reader, options);
				case _normal:
					return JsonSerializer.Deserialize<Vector3>(ref reader, options);
				case _color:
					return JsonSerializer.Deserialize<Color>(ref reader, options);
				case _attributes1:
				case _attributes2:
				case _attributes3:
					return reader.GetString()!.HexToUShort("Chunk Corner attributes");
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override ChunkCorner Create(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				Index = (ushort)values[_index]!,
				Texcoord = (Vector2)values[_texcoord]!,
				Texcoord2 = (Vector2)values[_texcoord2]!,
				Normal = (Vector3)values[_normal]!,
				Color = (Color)values[_color]!,
				Attributes1 = (ushort)values[_attributes1]!,
				Attributes2 = (ushort)values[_attributes2]!,
				Attributes3 = (ushort)values[_attributes3]!,
			};
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, ChunkCorner value, JsonSerializerOptions options)
		{
			writer.WriteNumber(_index, value.Index);

			if(value.Texcoord != ChunkCorner.DefaultValues.Texcoord)
			{
				writer.WritePropertyName(_texcoord);
				JsonSerializer.Serialize(writer, value.Texcoord, options);
			}

			if(value.Texcoord2 != ChunkCorner.DefaultValues.Texcoord2)
			{
				writer.WritePropertyName(_texcoord2);
				JsonSerializer.Serialize(writer, value.Texcoord2, options);
			}

			if(value.Normal != ChunkCorner.DefaultValues.Normal)
			{
				writer.WritePropertyName(_normal);
				JsonSerializer.Serialize(writer, value.Normal, options);
			}

			if(value.Color != ChunkCorner.DefaultValues.Color)
			{
				writer.WritePropertyName(_color);
				JsonSerializer.Serialize(writer, value.Color, options);
			}

			if(value.Attributes1 != ChunkCorner.DefaultValues.Attributes1)
			{
				writer.WriteString(_attributes1, value.Attributes1.ToString("X", CultureInfo.InvariantCulture));
			}

			if(value.Attributes2 != ChunkCorner.DefaultValues.Attributes2)
			{
				writer.WriteString(_attributes2, value.Attributes2.ToString("X", CultureInfo.InvariantCulture));
			}

			if(value.Attributes3 != ChunkCorner.DefaultValues.Attributes3)
			{
				writer.WriteString(_attributes3, value.Attributes3.ToString("X", CultureInfo.InvariantCulture));
			}
		}
	}
}
