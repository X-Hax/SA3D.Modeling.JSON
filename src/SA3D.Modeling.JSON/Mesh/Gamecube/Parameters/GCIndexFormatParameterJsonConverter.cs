using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Gamecube.Enums;
using SA3D.Modeling.Mesh.Gamecube.Parameters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Gamecube.Parameters
{
	/// <summary>
	/// Json converter for <see cref="GCIndexFormatParameter"/>
	/// </summary>
	public class GCIndexFormatParameterJsonConverter : ChildJsonObjectConverter<GCParameterType, GCIndexFormatParameter, IGCParameter>
	{
		private const string _indexFormat = nameof(GCIndexFormatParameter.IndexFormat);

		
		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<GCParameterType, IGCParameter> ParentConverter => GCParameterJsonConverter._globalGCParameterConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _indexFormat, new(PropertyTokenType.String | PropertyTokenType.Number, default(GCIndexFormat)) }
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(GCParameterType key)
		{
			return key == GCParameterType.IndexFormat;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _indexFormat:
					return JsonSerializer.Deserialize<GCIndexFormat>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override GCIndexFormatParameter CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				IndexFormat = (GCIndexFormat)values[_indexFormat]!
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, GCIndexFormatParameter value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_indexFormat);
			JsonSerializer.Serialize(writer, value.IndexFormat, options);
		}
	}
}
