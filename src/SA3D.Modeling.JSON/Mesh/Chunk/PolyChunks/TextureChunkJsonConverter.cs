using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh;
using SA3D.Modeling.Mesh.Chunk;
using SA3D.Modeling.Mesh.Chunk.PolyChunks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Chunk.PolyChunks
{
	/// <summary>
	/// Json converter for <see cref="TextureChunk"/>
	/// </summary>
	public class TextureChunkJsonConverter : ChildJsonObjectConverter<PolyChunkType, TextureChunk, PolyChunk>
	{
		private const string _mipmapDistanceMultiplier = nameof(TextureChunk.MipmapDistanceMultiplier);
		private const string _clampV = nameof(TextureChunk.ClampV);
		private const string _clampU = nameof(TextureChunk.ClampU);
		private const string _mirrorV = nameof(TextureChunk.MirrorV);
		private const string _mirrorU = nameof(TextureChunk.MirrorU);
		private const string _textureID = nameof(TextureChunk.TextureID);
		private const string _superSample = nameof(TextureChunk.SuperSample);
		private const string _filterMode = nameof(TextureChunk.FilterMode);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<PolyChunkType, PolyChunk> ParentConverter => PolyChunkJsonConverter._globalPolyChunkConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _mipmapDistanceMultiplier, new(PropertyTokenType.Number, 1f) },
			{ _clampV, new(PropertyTokenType.Bool, false) },
			{ _clampU, new(PropertyTokenType.Bool, false) },
			{ _mirrorV, new(PropertyTokenType.Bool, false) },
			{ _mirrorU, new(PropertyTokenType.Bool, false) },
			{ _textureID, new(PropertyTokenType.Number, (ushort)0) },
			{ _superSample, new(PropertyTokenType.Bool, false) },
			{ _filterMode, new(PropertyTokenType.String, FilterMode.Bilinear) },

		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(PolyChunkType key)
		{
			return key is PolyChunkType.TextureID or PolyChunkType.TextureID2;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _mipmapDistanceMultiplier:
					return reader.GetSingle();
				case _clampV:
				case _clampU:
				case _mirrorV:
				case _mirrorU:
				case _superSample:
					return reader.GetBoolean();
				case _textureID:
					return reader.GetUInt16();
				case _filterMode:
					return JsonSerializer.Deserialize<FilterMode>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override TextureChunk CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			return new(values[PolyChunkJsonConverter._type] is PolyChunkType.TextureID2)
			{
				MipmapDistanceMultiplier = (float)values[_mipmapDistanceMultiplier]!,
				ClampV = (bool)values[_clampV]!,
				ClampU = (bool)values[_clampU]!,
				MirrorV = (bool)values[_mirrorV]!,
				MirrorU = (bool)values[_mirrorU]!,
				TextureID = (ushort)values[_textureID]!,
				SuperSample = (bool)values[_superSample]!,
				FilterMode = (FilterMode)values[_filterMode]!,
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, TextureChunk value, JsonSerializerOptions options)
		{
			void writeBoolean(string name, bool value)
			{
				if(value)
				{
					writer.WriteBoolean(name, value);
				}
			}

			if(value.MipmapDistanceMultiplier != 1f)
			{
				writer.WriteNumber(_mipmapDistanceMultiplier, value.MipmapDistanceMultiplier);
			}

			writeBoolean(_clampV, value.ClampV);
			writeBoolean(_clampU, value.ClampU);
			writeBoolean(_mirrorV, value.MirrorV);
			writeBoolean(_mirrorU, value.MirrorU);
			writer.WriteNumber(_textureID, value.TextureID);
			writeBoolean(_superSample, value.SuperSample);

			writer.WritePropertyName(_filterMode);
			JsonSerializer.Serialize(writer, value.FilterMode, options);
		}
	}
}
