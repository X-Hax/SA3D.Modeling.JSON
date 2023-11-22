using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Buffer;
using SA3D.Modeling.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Buffer
{
	/// <summary>
	/// Json converter for <see cref="BufferCorner"/>.
	/// </summary>
	public class BufferCornerJsonConverter : SimpleJsonObjectConverter<BufferCorner>
	{
		private const string _vertexIndex = nameof(BufferCorner.VertexIndex);
		private const string _color = nameof(BufferCorner.Color);
		private const string _texcoord = nameof(BufferCorner.Texcoord);

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _vertexIndex, new(PropertyTokenType.Number, 0) },
			{ _color, new(PropertyTokenType.String, BufferMesh.DefaultColor) },
			{ _texcoord, new(PropertyTokenType.String, Vector2.Zero) }
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			return propertyName switch
			{
				_vertexIndex => reader.GetUInt16(),
				_color => JsonSerializer.Deserialize<Color>(ref reader, options),
				_texcoord => JsonSerializer.Deserialize<Vector2>(ref reader, options),
				_ => throw new InvalidPropertyException(),
			};
		}

		/// <inheritdoc/>
		protected override BufferCorner Create(ReadOnlyDictionary<string, object?> values)
		{
			return new BufferCorner(
				(ushort)values[_vertexIndex]!,
				(Color)values[_color]!,
				(Vector2)values[_texcoord]!
			);
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, BufferCorner value, JsonSerializerOptions options)
		{
			writer.WriteNumber(_vertexIndex, value.VertexIndex);

			if(value.Color != BufferMesh.DefaultColor)
			{
				writer.WritePropertyName(_color);
				JsonSerializer.Serialize(writer, value.Color, options);
			}

			if(value.Texcoord != Vector2.Zero)
			{
				writer.WritePropertyName(_texcoord);
				JsonSerializer.Serialize(writer, value.Texcoord, options);
			}
		}
	}
}
