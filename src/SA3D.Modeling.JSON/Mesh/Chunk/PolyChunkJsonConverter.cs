using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.JSON.Mesh.Chunk.PolyChunks;
using SA3D.Modeling.Mesh.Chunk;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Chunk
{
	/// <summary>
	/// Json parent converter for polychunks
	/// </summary>
	public class PolyChunkJsonConverter : ParentJsonObjectConverter<PolyChunkType, PolyChunk>
	{
		internal static readonly PolyChunkJsonConverter _globalPolyChunkConverter = new();

		internal const string _type = nameof(PolyChunk.Type);

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _type, new(PropertyTokenType.String, null) }
		});

		/// <inheritdoc/>
		protected internal override string KeyPropertyName => _type;

		/// <inheritdoc/>
		protected override PolyChunk CreateBase(ReadOnlyDictionary<string, object?> values)
		{
			throw new NotSupportedException();
		}

		/// <inheritdoc/>
		protected override Dictionary<PolyChunkType, IChildJsonConverter<PolyChunk>> CreateConverters()
		{
			TextureChunkJsonConverter textureConverter = new();
			MaterialChunkJsonConverter materialConverter = new();
			StripChunkJsonConverter stripConverter = new();
			VolumeChunkJsonConverter volumeConverter = new();

			return new()
			{
				{ PolyChunkType.BlendAlpha, new BlendAlphaChunkJsonConverter()},
				{ PolyChunkType.MipmapDistanceMultiplier, new MipmapDistanceMultiplierChunkJsonConverter() },
				{ PolyChunkType.SpecularExponent, new SpecularExponentChunkJsonConverter() },
				{ PolyChunkType.CacheList, new CacheListChunkJsonConverter() },
				{ PolyChunkType.DrawList, new DrawListChunkJsonConverter() },
				{ PolyChunkType.TextureID, textureConverter },
				{ PolyChunkType.TextureID2, textureConverter },
				{ PolyChunkType.Material_Diffuse, materialConverter },
				{ PolyChunkType.Material_Ambient, materialConverter },
				{ PolyChunkType.Material_DiffuseAmbient, materialConverter },
				{ PolyChunkType.Material_Specular, materialConverter },
				{ PolyChunkType.Material_DiffuseSpecular, materialConverter },
				{ PolyChunkType.Material_AmbientSpecular, materialConverter },
				{ PolyChunkType.Material_DiffuseAmbientSpecular, materialConverter },
				{ PolyChunkType.Material_Bump, new MaterialBumpChunkJsonConverter() },
				{ PolyChunkType.Material_Diffuse2, materialConverter },
				{ PolyChunkType.Material_Ambient2, materialConverter },
				{ PolyChunkType.Material_DiffuseAmbient2, materialConverter },
				{ PolyChunkType.Material_Specular2, materialConverter },
				{ PolyChunkType.Material_DiffuseSpecular2, materialConverter },
				{ PolyChunkType.Material_AmbientSpecular2, materialConverter },
				{ PolyChunkType.Material_DiffuseAmbientSpecular2, materialConverter },
				{ PolyChunkType.Volume_Polygon3, volumeConverter },
				{ PolyChunkType.Volume_Polygon4, volumeConverter },
				{ PolyChunkType.Volume_Strip, volumeConverter },
				{ PolyChunkType.Strip_Blank, stripConverter },
				{ PolyChunkType.Strip_Tex, stripConverter },
				{ PolyChunkType.Strip_HDTex, stripConverter },
				{ PolyChunkType.Strip_Normal, stripConverter },
				{ PolyChunkType.Strip_TexNormal, stripConverter },
				{ PolyChunkType.Strip_HDTexNormal, stripConverter },
				{ PolyChunkType.Strip_Color, stripConverter },
				{ PolyChunkType.Strip_TexColor, stripConverter },
				{ PolyChunkType.Strip_HDTexColor, stripConverter },
				{ PolyChunkType.Strip_BlankDouble, stripConverter },
				{ PolyChunkType.Strip_TexDouble, stripConverter },
				{ PolyChunkType.Strip_HDTexDouble, stripConverter },
			};
		}

		/// <inheritdoc/>
		protected override PolyChunkType GetKeyFromValue(PolyChunk value)
		{
			return value.Type;
		}

		/// <inheritdoc/>
		protected internal override object? ReadBaseValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _type:
					return JsonSerializer.Deserialize<PolyChunkType>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected internal override void WriteBaseValues(Utf8JsonWriter writer, PolyChunk value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_type);
			JsonSerializer.Serialize(writer, value.Type, options);
		}
	}
}
