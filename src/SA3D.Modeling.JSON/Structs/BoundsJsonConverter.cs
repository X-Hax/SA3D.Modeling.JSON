using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Structs
{
	/// <summary>
	/// Json converter for <see cref="Bounds"/>.
	/// </summary>
	public class BoundsJsonConverter : SimpleJsonObjectConverter<Bounds>
	{
		private const string _position = nameof(Bounds.Position);
		private const string _radius = nameof(Bounds.Radius);

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _position, new(PropertyTokenType.String, Vector3.Zero) },
			{ _radius, new(PropertyTokenType.Number, 0.0f) }
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			return propertyName switch
			{
				_position => JsonSerializer.Deserialize<Vector3>(ref reader, options),
				_radius => reader.GetSingle(),
				_ => throw new InvalidPropertyException(),
			};
		}

		/// <inheritdoc/>
		protected override Bounds Create(ReadOnlyDictionary<string, object?> values)
		{
			return new(
				(Vector3)values[_position]!,
				(float)values[_radius]!);
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, Bounds value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_position);
			JsonSerializer.Serialize(writer, value.Position, options);
			writer.WriteNumber(_radius, value.Radius);
		}
	}
}
