using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Gamecube.Enums;
using SA3D.Modeling.Mesh.Gamecube.Parameters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Gamecube.Parameters
{
	/// <summary>
	/// Json converter for <see cref="GCTextureParameter"/>
	/// </summary>
	public class GCTextureParameterJsonConverter : ChildJsonObjectConverter<GCParameterType, GCTextureParameter, IGCParameter>
	{
		private const string _textureID = nameof(GCTextureParameter.TextureID);
		private const string _tiling = nameof(GCTextureParameter.Tiling);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<GCParameterType, IGCParameter> ParentConverter => GCParameterJsonConverter._globalGCParameterConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _textureID, new(PropertyTokenType.Number, (ushort)0u) },
			{ _tiling, new(PropertyTokenType.String | PropertyTokenType.Number, default(GCTileMode)) },
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(GCParameterType key)
		{
			return key == GCParameterType.Texture;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _textureID:
					return reader.GetUInt16();
				case _tiling:
					return JsonSerializer.Deserialize<GCTileMode>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override GCTextureParameter CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				TextureID = (ushort)values[_textureID]!,
				Tiling = (GCTileMode)values[_tiling]!,
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, GCTextureParameter value, JsonSerializerOptions options)
		{
			writer.WriteNumber(_textureID, value.TextureID);

			writer.WritePropertyName(_tiling);
			JsonSerializer.Serialize(writer, value.Tiling, options);
		}
	}
}
