using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Gamecube.Enums;
using SA3D.Modeling.Mesh.Gamecube.Parameters;
using SA3D.Modeling.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Gamecube.Parameters
{
	/// <summary>
	/// Json converter for <see cref="GCAmbientColorParameter"/>
	/// </summary>
	public class GCAmbientColorParameterJsonConverter : ChildJsonObjectConverter<GCParameterType, GCAmbientColorParameter, IGCParameter>
	{
		private const string _ambientColor = nameof(GCAmbientColorParameter.AmbientColor);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<GCParameterType, IGCParameter> ParentConverter => GCParameterJsonConverter._globalGCParameterConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _ambientColor, new(PropertyTokenType.String, Color.ColorBlack ) }
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(GCParameterType key)
		{
			return key == GCParameterType.AmbientColor;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _ambientColor:
					return JsonSerializer.Deserialize<Color>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override GCAmbientColorParameter CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				AmbientColor = (Color)values[_ambientColor]!
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, GCAmbientColorParameter value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_ambientColor);
			JsonSerializer.Serialize(writer, value.AmbientColor, options);
		}
	}
}
