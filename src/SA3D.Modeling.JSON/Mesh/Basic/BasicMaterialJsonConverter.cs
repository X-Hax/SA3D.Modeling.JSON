using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh;
using SA3D.Modeling.Mesh.Basic;
using SA3D.Modeling.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Basic
{
	/// <summary>
	/// Json converter for <see cref="BasicMaterial"/>
	/// </summary>
	public class BasicMaterialJsonConverter : SimpleJsonObjectConverter<BasicMaterial>
	{
		private const string _diffuseColor = nameof(BasicMaterial.DiffuseColor);
		private const string _specularColor = nameof(BasicMaterial.SpecularColor);
		private const string _specularExponent = nameof(BasicMaterial.SpecularExponent);
		private const string _textureID = nameof(BasicMaterial.TextureID);
		private const string _userAttributes = nameof(BasicMaterial.UserAttributes);
		private const string _pickStatus = nameof(BasicMaterial.PickStatus);
		private const string _mipmapDistanceMultiplier = nameof(BasicMaterial.MipmapDistanceMultiplier);
		private const string _superSample = nameof(BasicMaterial.SuperSample);
		private const string _filterMode = nameof(BasicMaterial.FilterMode);
		private const string _clampV = nameof(BasicMaterial.ClampV);
		private const string _clampU = nameof(BasicMaterial.ClampU);
		private const string _mirrorV = nameof(BasicMaterial.MirrorV);
		private const string _mirrorU = nameof(BasicMaterial.MirrorU);
		private const string _ignoreSpecular = nameof(BasicMaterial.IgnoreSpecular);
		private const string _useAlpha = nameof(BasicMaterial.UseAlpha);
		private const string _useTexture = nameof(BasicMaterial.UseTexture);
		private const string _environmentMap = nameof(BasicMaterial.EnvironmentMap);
		private const string _doubleSided = nameof(BasicMaterial.DoubleSided);
		private const string _flatShading = nameof(BasicMaterial.FlatShading);
		private const string _ignoreLighting = nameof(BasicMaterial.IgnoreLighting);
		private const string _destinationAlpha = nameof(BasicMaterial.DestinationAlpha);
		private const string _sourceAlpha = nameof(BasicMaterial.SourceAlpha);

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _diffuseColor, new(PropertyTokenType.String, BasicMaterial.DefaultValues.DiffuseColor ) },
			{ _specularColor, new(PropertyTokenType.String, BasicMaterial.DefaultValues.SpecularColor) },
			{ _specularExponent, new(PropertyTokenType.Number, BasicMaterial.DefaultValues.SpecularExponent) },
			{ _textureID, new(PropertyTokenType.Number, BasicMaterial.DefaultValues.TextureID) },
			{ _userAttributes, new(PropertyTokenType.Number, BasicMaterial.DefaultValues.UserAttributes) },
			{ _pickStatus, new(PropertyTokenType.Bool, BasicMaterial.DefaultValues.PickStatus) },
			{ _mipmapDistanceMultiplier, new(PropertyTokenType.Number, BasicMaterial.DefaultValues.MipmapDistanceMultiplier) },
			{ _superSample, new(PropertyTokenType.Bool, BasicMaterial.DefaultValues.SuperSample) },
			{ _filterMode, new(PropertyTokenType.String, BasicMaterial.DefaultValues.FilterMode) },
			{ _clampV, new(PropertyTokenType.Bool, BasicMaterial.DefaultValues.ClampV) },
			{ _clampU, new(PropertyTokenType.Bool, BasicMaterial.DefaultValues.ClampU) },
			{ _mirrorV, new(PropertyTokenType.Bool, BasicMaterial.DefaultValues.MirrorV) },
			{ _mirrorU, new(PropertyTokenType.Bool, BasicMaterial.DefaultValues.MirrorU) },
			{ _ignoreSpecular, new(PropertyTokenType.Bool, BasicMaterial.DefaultValues.IgnoreSpecular) },
			{ _useAlpha, new(PropertyTokenType.Bool, BasicMaterial.DefaultValues.UseAlpha) },
			{ _useTexture, new(PropertyTokenType.Bool, BasicMaterial.DefaultValues.UseTexture) },
			{ _environmentMap, new(PropertyTokenType.Bool, BasicMaterial.DefaultValues.EnvironmentMap) },
			{ _doubleSided, new(PropertyTokenType.Bool, BasicMaterial.DefaultValues.DoubleSided) },
			{ _flatShading, new(PropertyTokenType.Bool, BasicMaterial.DefaultValues.FlatShading) },
			{ _ignoreLighting, new(PropertyTokenType.Bool, BasicMaterial.DefaultValues.IgnoreLighting) },
			{ _destinationAlpha, new(PropertyTokenType.String, BasicMaterial.DefaultValues.DestinationAlpha) },
			{ _sourceAlpha, new(PropertyTokenType.String, BasicMaterial.DefaultValues.SourceAlpha) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			return propertyName switch
			{
				_diffuseColor
				or _specularColor => JsonSerializer.Deserialize<Color>(ref reader, options),

				_textureID => reader.GetUInt32(),
				_userAttributes => reader.GetByte(),

				_specularExponent
				or _mipmapDistanceMultiplier => reader.GetSingle(),

				_filterMode => JsonSerializer.Deserialize<FilterMode>(ref reader, options),

				_pickStatus
				or _superSample
				or _clampV
				or _clampU
				or _mirrorV
				or _mirrorU
				or _ignoreSpecular
				or _useAlpha
				or _useTexture
				or _environmentMap
				or _doubleSided
				or _flatShading
				or _ignoreLighting => reader.GetBoolean(),

				_destinationAlpha
				or _sourceAlpha => JsonSerializer.Deserialize<BlendMode>(ref reader, options),
				_ => throw new InvalidPropertyException(),
			};
		}

		/// <inheritdoc/>
		protected override BasicMaterial Create(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				DiffuseColor = (Color)values[_diffuseColor]!,
				SpecularColor = (Color)values[_specularColor]!,
				SpecularExponent = (float)values[_specularExponent]!,
				TextureID = (uint)values[_textureID]!,
				UserAttributes = (byte)values[_userAttributes]!,
				PickStatus = (bool)values[_pickStatus]!,
				MipmapDistanceMultiplier = (float)values[_mipmapDistanceMultiplier]!,
				SuperSample = (bool)values[_superSample]!,
				FilterMode = (FilterMode)values[_filterMode]!,
				ClampV = (bool)values[_clampV]!,
				ClampU = (bool)values[_clampU]!,
				MirrorV = (bool)values[_mirrorV]!,
				MirrorU = (bool)values[_mirrorU]!,
				IgnoreSpecular = (bool)values[_ignoreSpecular]!,
				UseAlpha = (bool)values[_useAlpha]!,
				UseTexture = (bool)values[_useTexture]!,
				EnvironmentMap = (bool)values[_environmentMap]!,
				DoubleSided = (bool)values[_doubleSided]!,
				FlatShading = (bool)values[_flatShading]!,
				IgnoreLighting = (bool)values[_ignoreLighting]!,
				DestinationAlpha = (BlendMode)values[_destinationAlpha]!,
				SourceAlpha = (BlendMode)values[_sourceAlpha]!,
			};
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, BasicMaterial value, JsonSerializerOptions options)
		{
			void serialize<T>(string name, T value, T def) where T : notnull
			{
				if(!value.Equals(def))
				{
					writer.WritePropertyName(name);
					JsonSerializer.Serialize<T>(writer, value, options);
				}
			}

			void writeBoolean(string name, bool value, bool def)
			{
				if(value != def)
				{
					writer.WriteBoolean(name, value);
				}
			}

			serialize(_diffuseColor, value.DiffuseColor, BasicMaterial.DefaultValues.DiffuseColor);
			serialize(_specularColor, value.SpecularColor, BasicMaterial.DefaultValues.SpecularColor);

			if(value.SpecularExponent != BasicMaterial.DefaultValues.SpecularExponent)
			{
				writer.WriteNumber(_specularExponent, value.SpecularExponent);
			}

			if(value.TextureID != BasicMaterial.DefaultValues.TextureID)
			{
				writer.WriteNumber(_textureID, value.TextureID);
			}

			if(value.UserAttributes != BasicMaterial.DefaultValues.UserAttributes)
			{
				writer.WriteNumber(_userAttributes, value.UserAttributes);
			}

			writeBoolean(_pickStatus, value.PickStatus, BasicMaterial.DefaultValues.PickStatus);

			if(value.MipmapDistanceMultiplier != BasicMaterial.DefaultValues.MipmapDistanceMultiplier)
			{
				writer.WriteNumber(_mipmapDistanceMultiplier, value.MipmapDistanceMultiplier);
			}

			writeBoolean(_superSample, value.SuperSample, BasicMaterial.DefaultValues.SuperSample);

			serialize(_filterMode, value.FilterMode, BasicMaterial.DefaultValues.FilterMode);

			writeBoolean(_clampV, value.ClampV, BasicMaterial.DefaultValues.ClampV);
			writeBoolean(_clampU, value.ClampU, BasicMaterial.DefaultValues.ClampU);
			writeBoolean(_mirrorV, value.MirrorV, BasicMaterial.DefaultValues.MirrorV);
			writeBoolean(_mirrorU, value.MirrorU, BasicMaterial.DefaultValues.MirrorU);
			writeBoolean(_ignoreSpecular, value.IgnoreSpecular, BasicMaterial.DefaultValues.IgnoreSpecular);
			writeBoolean(_useAlpha, value.UseAlpha, BasicMaterial.DefaultValues.UseAlpha);
			writeBoolean(_useTexture, value.UseTexture, BasicMaterial.DefaultValues.UseTexture);
			writeBoolean(_environmentMap, value.EnvironmentMap, BasicMaterial.DefaultValues.EnvironmentMap);
			writeBoolean(_doubleSided, value.DoubleSided, BasicMaterial.DefaultValues.DoubleSided);
			writeBoolean(_flatShading, value.FlatShading, BasicMaterial.DefaultValues.FlatShading);
			writeBoolean(_ignoreLighting, value.IgnoreLighting, BasicMaterial.DefaultValues.IgnoreLighting);

			serialize(_destinationAlpha, value.DestinationAlpha, BasicMaterial.DefaultValues.DestinationAlpha);
			serialize(_sourceAlpha, value.SourceAlpha, BasicMaterial.DefaultValues.SourceAlpha);
		}
	}
}
