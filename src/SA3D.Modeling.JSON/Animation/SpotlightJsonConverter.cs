using SA3D.Modeling.Animation;
using SA3D.Modeling.JSON.JsonBase;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Animation
{
	/// <summary>
	/// Json Converter for <see cref="Spotlight"/>
	/// </summary>
	public class SpotlightJsonConverter : SimpleJsonObjectConverter<Spotlight>
	{
		private const string _near = nameof(Spotlight.near);
		private const string _far = nameof(Spotlight.far);
		private const string _insideAngle = nameof(Spotlight.insideAngle);
		private const string _outsideAngle = nameof(Spotlight.outsideAngle);


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _near, new(PropertyTokenType.Number, 0f) },
			{ _far, new(PropertyTokenType.Number, 0f) },
			{ _insideAngle, new(PropertyTokenType.Number, 0f) },
			{ _outsideAngle, new(PropertyTokenType.Number, 0f) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _near:
				case _far:
				case _insideAngle:
				case _outsideAngle:
					return reader.GetSingle();
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override Spotlight Create(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				near = (float)values[_near]!,
				far = (float)values[_far]!,
				insideAngle = (float)values[_insideAngle]!,
				outsideAngle = (float)values[_outsideAngle]!,
			};
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, Spotlight value, JsonSerializerOptions options)
		{
			writer.WriteNumber(_near, value.near);
			writer.WriteNumber(_far, value.far);
			writer.WriteNumber(_insideAngle, value.insideAngle);
			writer.WriteNumber(_outsideAngle, value.outsideAngle);
		}
	}
}
