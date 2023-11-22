using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh;
using SA3D.Modeling.Mesh.Buffer;
using SA3D.Modeling.Mesh.Gamecube.Enums;
using SA3D.Modeling.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Buffer
{
	/// <summary>
	/// Json converter for <see cref="BufferMaterial"/>
	/// </summary>
	public class BufferMaterialJsonConverter : SimpleJsonObjectConverter<BufferMaterial>
	{
		private const string _diffuse = nameof(BufferMaterial.Diffuse);
		private const string _specular = nameof(BufferMaterial.Specular);
		private const string _specularExponent = nameof(BufferMaterial.SpecularExponent);
		private const string _ambient = nameof(BufferMaterial.Ambient);
		private const string _textureIndex = nameof(BufferMaterial.TextureIndex);
		private const string _textureFiltering = nameof(BufferMaterial.TextureFiltering);
		private const string _mipmapDistanceMultiplier = nameof(BufferMaterial.MipmapDistanceMultiplier);
		private const string _sourceBlendMode = nameof(BufferMaterial.SourceBlendMode);
		private const string _destinationBlendmode = nameof(BufferMaterial.DestinationBlendmode);
		private const string _useTexture = nameof(BufferMaterial.UseTexture);
		private const string _anisotropicFiltering = nameof(BufferMaterial.AnisotropicFiltering);
		private const string _clampU = nameof(BufferMaterial.ClampU);
		private const string _clampV = nameof(BufferMaterial.ClampV);
		private const string _mirrorU = nameof(BufferMaterial.MirrorU);
		private const string _mirrorV = nameof(BufferMaterial.MirrorV);
		private const string _normalMapping = nameof(BufferMaterial.NormalMapping);
		private const string _noLighting = nameof(BufferMaterial.NoLighting);
		private const string _noAmbient = nameof(BufferMaterial.NoAmbient);
		private const string _noSpecular = nameof(BufferMaterial.NoSpecular);
		private const string _flat = nameof(BufferMaterial.Flat);
		private const string _useAlpha = nameof(BufferMaterial.UseAlpha);
		private const string _backfaceCulling = nameof(BufferMaterial.BackfaceCulling);
		private const string _gcShadowStencil = nameof(BufferMaterial.GCShadowStencil);
		private const string _gcTexCoordID = nameof(BufferMaterial.GCTexCoordID);
		private const string _gcTexCoordType = nameof(BufferMaterial.GCTexCoordType);
		private const string _gcTexCoordSource = nameof(BufferMaterial.GCTexCoordSource);
		private const string _gcMatrixID = nameof(BufferMaterial.GCMatrixID);

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _diffuse, new(PropertyTokenType.String, BufferMaterial.DefaultValues.Diffuse) },
			{ _specular, new(PropertyTokenType.String, BufferMaterial.DefaultValues.Specular) },
			{ _specularExponent, new(PropertyTokenType.Number, BufferMaterial.DefaultValues.SpecularExponent) },
			{ _ambient, new(PropertyTokenType.String, BufferMaterial.DefaultValues.Ambient) },
			{ _textureIndex, new(PropertyTokenType.Number, BufferMaterial.DefaultValues.TextureIndex) },
			{ _textureFiltering, new(PropertyTokenType.String, BufferMaterial.DefaultValues.TextureFiltering) },
			{ _mipmapDistanceMultiplier, new(PropertyTokenType.Number, BufferMaterial.DefaultValues.MipmapDistanceMultiplier) },
			{ _sourceBlendMode, new(PropertyTokenType.String, BufferMaterial.DefaultValues.SourceBlendMode) },
			{ _destinationBlendmode, new(PropertyTokenType.String, BufferMaterial.DefaultValues.DestinationBlendmode) },

			{ _useTexture, new(PropertyTokenType.Bool, BufferMaterial.DefaultValues.UseTexture) },
			{ _anisotropicFiltering, new(PropertyTokenType.Bool, BufferMaterial.DefaultValues.AnisotropicFiltering) },
			{ _clampU, new(PropertyTokenType.Bool, BufferMaterial.DefaultValues.ClampU) },
			{ _clampV, new(PropertyTokenType.Bool, BufferMaterial.DefaultValues.ClampV) },
			{ _mirrorU, new(PropertyTokenType.Bool, BufferMaterial.DefaultValues.MirrorU) },
			{ _mirrorV, new(PropertyTokenType.Bool, BufferMaterial.DefaultValues.MirrorV) },
			{ _normalMapping, new(PropertyTokenType.Bool, BufferMaterial.DefaultValues.NormalMapping) },
			{ _noLighting, new(PropertyTokenType.Bool, BufferMaterial.DefaultValues.NoLighting) },
			{ _noAmbient, new(PropertyTokenType.Bool, BufferMaterial.DefaultValues.NoAmbient) },
			{ _noSpecular, new(PropertyTokenType.Bool, BufferMaterial.DefaultValues.NoSpecular) },
			{ _flat, new(PropertyTokenType.Bool, BufferMaterial.DefaultValues.Flat) },
			{ _useAlpha, new(PropertyTokenType.Bool, BufferMaterial.DefaultValues.UseAlpha) },
			{ _backfaceCulling, new(PropertyTokenType.Bool, BufferMaterial.DefaultValues.BackfaceCulling) },

			{ _gcShadowStencil, new(PropertyTokenType.Number, BufferMaterial.DefaultValues.GCShadowStencil) },
			{ _gcTexCoordID, new(PropertyTokenType.String, BufferMaterial.DefaultValues.GCTexCoordID) },
			{ _gcTexCoordType, new(PropertyTokenType.String, BufferMaterial.DefaultValues.GCTexCoordType) },
			{ _gcTexCoordSource, new(PropertyTokenType.String, BufferMaterial.DefaultValues.GCTexCoordSource) },
			{ _gcMatrixID, new(PropertyTokenType.String, BufferMaterial.DefaultValues.GCMatrixID) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			return propertyName switch
			{
				_diffuse
				or _specular
				or _ambient => JsonSerializer.Deserialize<Color>(ref reader, options),

				_specularExponent
				or _mipmapDistanceMultiplier => reader.GetSingle(),

				_textureIndex => reader.GetUInt32(),

				_textureFiltering => JsonSerializer.Deserialize<FilterMode>(ref reader, options),

				_sourceBlendMode
				or _destinationBlendmode => JsonSerializer.Deserialize<BlendMode>(ref reader, options),

				_useTexture
				or _anisotropicFiltering
				or _clampU
				or _clampV
				or _mirrorU
				or _mirrorV
				or _normalMapping
				or _noLighting
				or _noAmbient
				or _noSpecular
				or _flat
				or _useAlpha
				or _backfaceCulling => reader.GetBoolean(),

				_gcShadowStencil => reader.GetByte(),
				_gcTexCoordID => JsonSerializer.Deserialize<GCTexCoordID>(ref reader, options),
				_gcTexCoordType => JsonSerializer.Deserialize<GCTexCoordType>(ref reader, options),
				_gcTexCoordSource => JsonSerializer.Deserialize<GCTexCoordSource>(ref reader, options),
				_gcMatrixID => JsonSerializer.Deserialize<GCTexCoordMatrix>(ref reader, options),

				_ => throw new InvalidPropertyException(),
			};
		}

		/// <inheritdoc/>
		protected override BufferMaterial Create(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				Diffuse = (Color)values[_diffuse]!,
				Specular = (Color)values[_specular]!,
				SpecularExponent = (float)values[_specularExponent]!,
				Ambient = (Color)values[_ambient]!,
				TextureIndex = (uint)values[_textureIndex]!,
				TextureFiltering = (FilterMode)values[_textureFiltering]!,
				MipmapDistanceMultiplier = (float)values[_mipmapDistanceMultiplier]!,
				SourceBlendMode = (BlendMode)values[_sourceBlendMode]!,
				DestinationBlendmode = (BlendMode)values[_destinationBlendmode]!,
				UseTexture = (bool)values[_useTexture]!,
				AnisotropicFiltering = (bool)values[_anisotropicFiltering]!,
				ClampU = (bool)values[_clampU]!,
				ClampV = (bool)values[_clampV]!,
				MirrorU = (bool)values[_mirrorU]!,
				MirrorV = (bool)values[_mirrorV]!,
				NormalMapping = (bool)values[_normalMapping]!,
				NoLighting = (bool)values[_noLighting]!,
				NoAmbient = (bool)values[_noAmbient]!,
				NoSpecular = (bool)values[_noSpecular]!,
				Flat = (bool)values[_flat]!,
				UseAlpha = (bool)values[_useAlpha]!,
				BackfaceCulling = (bool)values[_backfaceCulling]!,
				GCShadowStencil = (byte)values[_gcShadowStencil]!,
				GCTexCoordID = (GCTexCoordID)values[_gcTexCoordID]!,
				GCTexCoordType = (GCTexCoordType)values[_gcTexCoordType]!,
				GCTexCoordSource = (GCTexCoordSource)values[_gcTexCoordSource]!,
				GCMatrixID = (GCTexCoordMatrix)values[_gcMatrixID]!,
			};
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, BufferMaterial value, JsonSerializerOptions options)
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

			serialize(_diffuse, value.Diffuse, BufferMaterial.DefaultValues.Diffuse);
			serialize(_specular, value.Specular, BufferMaterial.DefaultValues.Specular);

			if(value.SpecularExponent != BufferMaterial.DefaultValues.SpecularExponent)
			{
				writer.WriteNumber(_specularExponent, value.SpecularExponent);
			}

			serialize(_ambient, value.Ambient, BufferMaterial.DefaultValues.Ambient);

			if(value.TextureIndex != BufferMaterial.DefaultValues.TextureIndex)
			{
				writer.WriteNumber(_textureIndex, value.TextureIndex);
			}

			serialize(_textureFiltering, value.TextureFiltering, BufferMaterial.DefaultValues.TextureFiltering);

			if(value.MipmapDistanceMultiplier != BufferMaterial.DefaultValues.MipmapDistanceMultiplier)
			{
				writer.WriteNumber(_mipmapDistanceMultiplier, value.MipmapDistanceMultiplier);
			}

			serialize(_sourceBlendMode, value.SourceBlendMode, BufferMaterial.DefaultValues.SourceBlendMode);
			serialize(_destinationBlendmode, value.DestinationBlendmode, BufferMaterial.DefaultValues.DestinationBlendmode);

			writeBoolean(_useTexture, value.UseTexture, BufferMaterial.DefaultValues.UseTexture);
			writeBoolean(_anisotropicFiltering, value.AnisotropicFiltering, BufferMaterial.DefaultValues.AnisotropicFiltering);
			writeBoolean(_clampU, value.ClampU, BufferMaterial.DefaultValues.ClampU);
			writeBoolean(_clampV, value.ClampV, BufferMaterial.DefaultValues.ClampV);
			writeBoolean(_mirrorU, value.MirrorU, BufferMaterial.DefaultValues.MirrorU);
			writeBoolean(_mirrorV, value.MirrorV, BufferMaterial.DefaultValues.MirrorV);
			writeBoolean(_normalMapping, value.NormalMapping, BufferMaterial.DefaultValues.NormalMapping);
			writeBoolean(_noLighting, value.NoLighting, BufferMaterial.DefaultValues.NoLighting);
			writeBoolean(_noAmbient, value.NoAmbient, BufferMaterial.DefaultValues.NoAmbient);
			writeBoolean(_noSpecular, value.NoSpecular, BufferMaterial.DefaultValues.NoSpecular);
			writeBoolean(_flat, value.Flat, BufferMaterial.DefaultValues.Flat);
			writeBoolean(_useAlpha, value.UseAlpha, BufferMaterial.DefaultValues.UseAlpha);
			writeBoolean(_backfaceCulling, value.BackfaceCulling, BufferMaterial.DefaultValues.BackfaceCulling);

			if(value.GCShadowStencil != BufferMaterial.DefaultValues.GCShadowStencil)
			{
				writer.WriteNumber(_gcShadowStencil, value.GCShadowStencil);
			}

			serialize(_gcTexCoordID, value.GCTexCoordID, BufferMaterial.DefaultValues.GCTexCoordID);
			serialize(_gcTexCoordType, value.GCTexCoordType, BufferMaterial.DefaultValues.GCTexCoordType);
			serialize(_gcTexCoordSource, value.GCTexCoordSource, BufferMaterial.DefaultValues.GCTexCoordSource);
			serialize(_gcMatrixID, value.GCMatrixID, BufferMaterial.DefaultValues.GCMatrixID);
		}
	}
}
