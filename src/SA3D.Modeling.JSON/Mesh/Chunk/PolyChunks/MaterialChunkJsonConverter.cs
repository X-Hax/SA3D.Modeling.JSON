using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh;
using SA3D.Modeling.Mesh.Chunk;
using SA3D.Modeling.Mesh.Chunk.PolyChunks;
using SA3D.Modeling.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Chunk.PolyChunks
{
	/// <summary>
	/// Json converter for <see cref="MaterialChunk"/>
	/// </summary>
	public class MaterialChunkJsonConverter : ChildJsonObjectConverter<PolyChunkType, MaterialChunk, PolyChunk>
	{
		private const string _sourceAlpha = nameof(MaterialChunk.SourceAlpha);
		private const string _destinationAlpha = nameof(MaterialChunk.DestinationAlpha);
		private const string _diffuse = nameof(MaterialChunk.Diffuse);
		private const string _ambient = nameof(MaterialChunk.Ambient);
		private const string _specular = nameof(MaterialChunk.Specular);
		private const string _specularExponent = nameof(MaterialChunk.SpecularExponent);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<PolyChunkType, PolyChunk> ParentConverter => PolyChunkJsonConverter._globalPolyChunkConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _sourceAlpha, new(PropertyTokenType.String, BlendMode.Zero) },
			{ _destinationAlpha, new(PropertyTokenType.String, BlendMode.Zero) },
			{ _diffuse, new(PropertyTokenType.String, null, true) },
			{ _ambient, new(PropertyTokenType.String, null, true) },
			{ _specular, new(PropertyTokenType.String, null, true) },
			{ _specularExponent, new(PropertyTokenType.Number, (byte)0) },
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(PolyChunkType key)
		{
			return key is not PolyChunkType.Material_Bump
				and >= PolyChunkType.Material_Diffuse
				and <= PolyChunkType.Material_DiffuseAmbientSpecular2;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _sourceAlpha:
				case _destinationAlpha:
					return JsonSerializer.Deserialize<BlendMode>(ref reader, options);
				case _diffuse:
				case _ambient:
				case _specular:
					return JsonSerializer.Deserialize<Color?>(ref reader, options);
				case _specularExponent:
					return reader.GetByte();
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override MaterialChunk CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			PolyChunkType type = (PolyChunkType)values[PolyChunkJsonConverter._type]!;

			bool second = type
				is PolyChunkType.Material_Diffuse2
				or PolyChunkType.Material_Ambient2
				or PolyChunkType.Material_DiffuseAmbient2
				or PolyChunkType.Material_Specular2
				or PolyChunkType.Material_DiffuseSpecular2
				or PolyChunkType.Material_AmbientSpecular2
				or PolyChunkType.Material_DiffuseAmbientSpecular2;

			return new()
			{
				SourceAlpha = (BlendMode)values[_sourceAlpha]!,
				DestinationAlpha = (BlendMode)values[_destinationAlpha]!,
				Diffuse = (Color?)values[_diffuse]!,
				Ambient = (Color?)values[_ambient]!,
				Specular = (Color?)values[_specular]!,
				SpecularExponent = (byte)values[_specularExponent]!,
				Second = second,
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, MaterialChunk value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_sourceAlpha);
			JsonSerializer.Serialize(writer, value.SourceAlpha, options);

			writer.WritePropertyName(_destinationAlpha);
			JsonSerializer.Serialize(writer, value.DestinationAlpha, options);

			if(value.Diffuse != null)
			{
				writer.WritePropertyName(_diffuse);
				JsonSerializer.Serialize(writer, value.Diffuse, options);
			}

			if(value.Ambient != null)
			{
				writer.WritePropertyName(_ambient);
				JsonSerializer.Serialize(writer, value.Ambient, options);
			}

			if(value.Specular != null)
			{
				writer.WritePropertyName(_specular);
				JsonSerializer.Serialize(writer, value.Specular, options);
			}

			if(value.SpecularExponent != 0)
			{
				writer.WriteNumber(_specularExponent, value.SpecularExponent);
			}
		}
	}
}
