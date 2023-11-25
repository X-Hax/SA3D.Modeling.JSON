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
	/// Json converter for <see cref="ChunkVertex"/>
	/// </summary>
	public class ChunkVertexJsonConverter : SimpleJsonObjectConverter<ChunkVertex>
	{
		private const string _position = nameof(ChunkVertex.Position);
		private const string _normal = nameof(ChunkVertex.Normal);
		private const string _diffuse = nameof(ChunkVertex.Diffuse);
		private const string _specular = nameof(ChunkVertex.Specular);
		private const string _attributes = nameof(ChunkVertex.Attributes);


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _position, new(PropertyTokenType.String, ChunkVertex.DefaultValues.Position) },
			{ _normal, new(PropertyTokenType.String, ChunkVertex.DefaultValues.Normal) },
			{ _diffuse, new(PropertyTokenType.String, ChunkVertex.DefaultValues.Diffuse) },
			{ _specular, new(PropertyTokenType.String, ChunkVertex.DefaultValues.Specular) },
			{ _attributes, new(PropertyTokenType.String, ChunkVertex.DefaultValues.Attributes) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _position:
				case _normal:
					return JsonSerializer.Deserialize<Vector3>(ref reader, options);
				case _diffuse:
				case _specular:
					return JsonSerializer.Deserialize<Color>(ref reader, options);
				case _attributes:
					return reader.GetString()!.HexToUInt("Chunk Vertex attributes");
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override ChunkVertex Create(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				Position = (Vector3)values[_position]!,
				Normal = (Vector3)values[_normal]!,
				Diffuse = (Color)values[_diffuse]!,
				Specular = (Color)values[_specular]!,
				Attributes = (uint)values[_attributes]!,
			};
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, ChunkVertex value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_position);
			JsonSerializer.Serialize(writer, value.Position, options);

			if(value.Normal != ChunkVertex.DefaultValues.Normal)
			{
				writer.WritePropertyName(_normal);
				JsonSerializer.Serialize(writer, value.Normal, options);
			}

			if(value.Diffuse != ChunkVertex.DefaultValues.Diffuse)
			{
				writer.WritePropertyName(_diffuse);
				JsonSerializer.Serialize(writer, value.Diffuse, options);
			}

			if(value.Specular != ChunkVertex.DefaultValues.Specular)
			{
				writer.WritePropertyName(_specular);
				JsonSerializer.Serialize(writer, value.Specular, options);
			}

			if(value.Attributes != ChunkVertex.DefaultValues.Attributes)
			{
				writer.WriteString(_attributes, value.Attributes.ToString("X", CultureInfo.InvariantCulture));
			}
		}
	}
}
