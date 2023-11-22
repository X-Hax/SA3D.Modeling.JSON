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
	/// Json converter for <see cref="GCDiffuseColorParameter"/>
	/// </summary>
	public class GCDiffuseColorParameterJsonConverter : ChildJsonObjectConverter<GCParameterType, GCDiffuseColorParameter, IGCParameter>
	{
		private const string _diffuseColor = nameof(GCDiffuseColorParameter.DiffuseColor);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<GCParameterType, IGCParameter> ParentConverter => GCParameterJsonConverter._globalGCParameterConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _diffuseColor, new(PropertyTokenType.String, Color.ColorBlack ) }
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(GCParameterType key)
		{
			return key == GCParameterType.DiffuseColor;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _diffuseColor:
					return JsonSerializer.Deserialize<Color>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override GCDiffuseColorParameter CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				DiffuseColor = (Color)values[_diffuseColor]!
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, GCDiffuseColorParameter value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_diffuseColor);
			JsonSerializer.Serialize(writer, value.DiffuseColor, options);
		}
	}
}
