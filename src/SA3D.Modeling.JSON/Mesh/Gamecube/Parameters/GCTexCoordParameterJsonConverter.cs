using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Gamecube.Enums;
using SA3D.Modeling.Mesh.Gamecube.Parameters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Gamecube.Parameters
{
	/// <summary>
	/// Json converter for <see cref="GCTexCoordParameter"/>
	/// </summary>
	public class GCTexCoordParameterJsonConverter : ChildJsonObjectConverter<GCParameterType, GCTexCoordParameter, IGCParameter>
	{
		private const string _texCoordID = nameof(GCTexCoordParameter.TexCoordID);
		private const string _texCoordType = nameof(GCTexCoordParameter.TexCoordType);
		private const string _texCoordSource = nameof(GCTexCoordParameter.TexCoordSource);
		private const string _matrixID = nameof(GCTexCoordParameter.MatrixID);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<GCParameterType, IGCParameter> ParentConverter => GCParameterJsonConverter._globalGCParameterConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _texCoordID, new(PropertyTokenType.String, GCTexCoordParameter.DefaultValues.TexCoordID) },
			{ _texCoordType, new(PropertyTokenType.String, GCTexCoordParameter.DefaultValues.TexCoordType) },
			{ _texCoordSource, new(PropertyTokenType.String, GCTexCoordParameter.DefaultValues.TexCoordSource) },
			{ _matrixID, new(PropertyTokenType.String, GCTexCoordParameter.DefaultValues.MatrixID) },
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(GCParameterType key)
		{
			return key == GCParameterType.Texcoord;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _texCoordID:
					return JsonSerializer.Deserialize<GCTexCoordID>(ref reader, options);
				case _texCoordType:
					return JsonSerializer.Deserialize<GCTexCoordType>(ref reader, options);
				case _texCoordSource:
					return JsonSerializer.Deserialize<GCTexCoordSource>(ref reader, options);
				case _matrixID:
					return JsonSerializer.Deserialize<GCTexCoordMatrix>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override GCTexCoordParameter CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				TexCoordID = (GCTexCoordID)values[_texCoordID]!,
				TexCoordType = (GCTexCoordType)values[_texCoordType]!,
				TexCoordSource = (GCTexCoordSource)values[_texCoordSource]!,
				MatrixID = (GCTexCoordMatrix)values[_matrixID]!,
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, GCTexCoordParameter value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_texCoordID);
			JsonSerializer.Serialize(writer, value.TexCoordID, options);

			writer.WritePropertyName(_texCoordType);
			JsonSerializer.Serialize(writer, value.TexCoordType, options);

			writer.WritePropertyName(_texCoordSource);
			JsonSerializer.Serialize(writer, value.TexCoordSource, options);

			writer.WritePropertyName(_matrixID);
			JsonSerializer.Serialize(writer, value.MatrixID, options);

		}
	}
}
