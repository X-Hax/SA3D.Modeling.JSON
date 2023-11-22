using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Buffer;
using SA3D.Modeling.Mesh.Weighted;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Weighted
{
	/// <summary>
	/// Json converter for <see cref="WeightedVertex"/>.
	/// </summary>
	public class WeightedVertexJsonConverter : SimpleJsonObjectConverter<WeightedVertex>
	{
		private const string _position = nameof(WeightedVertex.Position);
		private const string _normal = nameof(WeightedVertex.Normal);
		private const string _weights = nameof(WeightedVertex.Weights);

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _position, new(PropertyTokenType.String, Vector3.Zero) },
			{ _normal, new(PropertyTokenType.String, BufferMesh.DefaultNormal) },
			{ _weights, new(PropertyTokenType.Array, null, true) }
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _position:
				case _normal:
					return JsonSerializer.Deserialize<Vector3>(ref reader, options);
				case _weights:
					return JsonSerializer.Deserialize<float[]?>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override WeightedVertex Create(ReadOnlyDictionary<string, object?> values)
		{
			return new(
				(Vector3)values[_position]!,
				(Vector3)values[_normal]!,
				(float[]?)values[_weights]);
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, WeightedVertex value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_position);
			JsonSerializer.Serialize(writer, value.Position, options);

			if(value.Normal != BufferMesh.DefaultNormal)
			{
				writer.WritePropertyName(_normal);
				JsonSerializer.Serialize(writer, value.Normal, options);
			}

			if(value.Weights != null)
			{
				writer.WritePropertyName(_weights);
				JsonSerializer.Serialize(writer, value.Weights, options);
			}
		}
	}
}
