using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh;
using SA3D.Modeling.Mesh.Gamecube.Enums;
using SA3D.Modeling.Mesh.Gamecube.Parameters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Gamecube.Parameters
{
	/// <summary>
	/// Json converter for <see cref="GCBlendAlphaParameter"/>
	/// </summary>
	public class GCBlendAlphaParameterJsonConverter : ChildJsonObjectConverter<GCParameterType, GCBlendAlphaParameter, IGCParameter>
	{
		private const string _sourceAlpha = nameof(GCBlendAlphaParameter.SourceAlpha);
		private const string _destinationAlpha = nameof(GCBlendAlphaParameter.DestinationAlpha);
		private const string _useAlpha = nameof(GCBlendAlphaParameter.UseAlpha);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<GCParameterType, IGCParameter> ParentConverter => GCParameterJsonConverter._globalGCParameterConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _sourceAlpha, new(PropertyTokenType.String, GCBlendAlphaParameter.DefaultBlendParameter.SourceAlpha) },
			{ _destinationAlpha, new(PropertyTokenType.String, GCBlendAlphaParameter.DefaultBlendParameter.DestinationAlpha) },
			{ _useAlpha, new(PropertyTokenType.Bool, GCBlendAlphaParameter.DefaultBlendParameter.UseAlpha) },
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(GCParameterType key)
		{
			return key == GCParameterType.BlendAlpha;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _sourceAlpha:
				case _destinationAlpha:
					return JsonSerializer.Deserialize<BlendMode>(ref reader, options);
				case _useAlpha:
					return reader.GetBoolean();
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override GCBlendAlphaParameter CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				SourceAlpha = (BlendMode)values[_sourceAlpha]!,
				DestinationAlpha = (BlendMode)values[_destinationAlpha]!,
				UseAlpha = (bool)values[_useAlpha]!,

			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, GCBlendAlphaParameter value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_sourceAlpha);
			JsonSerializer.Serialize(writer, value.SourceAlpha, options);

			writer.WritePropertyName(_destinationAlpha);
			JsonSerializer.Serialize(writer, value.DestinationAlpha, options);

			writer.WriteBoolean(_useAlpha, value.UseAlpha);
		}
	}
}
