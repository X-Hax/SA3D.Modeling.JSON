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
	/// Json converter for <see cref="GCSpecularColorParameter"/>
	/// </summary>
	public class GCSpecularColorParameterJsonConverter : ChildJsonObjectConverter<GCParameterType, GCSpecularColorParameter, IGCParameter>
	{
		private const string _specularColor = nameof(GCSpecularColorParameter.SpecularColor);

		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<GCParameterType, IGCParameter> ParentConverter => GCParameterJsonConverter._globalGCParameterConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _specularColor, new(PropertyTokenType.String, Color.ColorBlack ) }
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(GCParameterType key)
		{
			return key == GCParameterType.SpecularColor;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _specularColor:
					return JsonSerializer.Deserialize<Color>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override GCSpecularColorParameter CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				SpecularColor = (Color)values[_specularColor]!
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, GCSpecularColorParameter value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_specularColor);
			JsonSerializer.Serialize(writer, value.SpecularColor, options);
		}
	}
}
