using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Buffer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Buffer
{
	/// <summary>
	/// Json converter for <see cref="BufferVertex"/>
	/// </summary>
	public class BufferVertexJsonConverter : SimpleJsonObjectConverter<BufferVertex>
	{
		private const string _position = nameof(BufferVertex.Position);
		private const string _normal = nameof(BufferVertex.Normal);
		private const string _index = nameof(BufferVertex.Index);
		private const string _weight = nameof(BufferVertex.Weight);

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _position, new(PropertyTokenType.String, Vector3.Zero) },
			{ _normal, new(PropertyTokenType.String, BufferMesh.DefaultNormal) },
			{ _index, new(PropertyTokenType.Number, (ushort)0u) },
			{ _weight, new(PropertyTokenType.Number, 0.0f) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			return propertyName switch
			{
				_position 
				or _normal => JsonSerializer.Deserialize<Vector3>(ref reader, options),
				_index => reader.GetUInt16(),
				_weight => reader.GetSingle(),
				_ => throw new InvalidPropertyException(),
			};
		}

		/// <inheritdoc/>
		protected override BufferVertex Create(ReadOnlyDictionary<string, object?> values)
		{
			return new(
				(Vector3)values[_position]!,
				(Vector3)values[_normal]!,
				(ushort)values[_index]!,
				(float)values[_weight]!
			);
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, BufferVertex value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_position);
			JsonSerializer.Serialize(writer, value.Position, options);

			if(value.Normal != BufferMesh.DefaultNormal)
			{
				writer.WritePropertyName(_normal);
				JsonSerializer.Serialize(writer, value.Normal, options);
			}

			if(value.Index != 0)
			{
				writer.WriteNumber(_index, value.Index);
			}

			if(value.Weight != 0)
			{
				writer.WriteNumber(_weight, value.Weight);
			}
		}
	}
}
