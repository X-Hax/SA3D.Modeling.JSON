using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Chunk;
using SA3D.Modeling.Mesh.Chunk.PolyChunks;
using SA3D.Modeling.Mesh.Chunk.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Chunk.PolyChunks
{
	/// <summary>
	/// Json converter for <see cref="StripChunk"/>
	/// </summary>
	public class StripChunkJsonConverter : ChildJsonObjectConverter<PolyChunkType, StripChunk, PolyChunk>
	{
		private const string _ignoreLight = nameof(StripChunk.IgnoreLight);
		private const string _ignoreSpecular = nameof(StripChunk.IgnoreSpecular);
		private const string _ignoreAmbient = nameof(StripChunk.IgnoreAmbient);
		private const string _useAlpha = nameof(StripChunk.UseAlpha);
		private const string _doubleSide = nameof(StripChunk.DoubleSide);
		private const string _flatShading = nameof(StripChunk.FlatShading);
		private const string _environmentMapping = nameof(StripChunk.EnvironmentMapping);
		private const string _unknownAttribute = nameof(StripChunk.UnknownAttribute);
		private const string _strips = nameof(StripChunk.Strips );
		private const string _triangleAttributeCount = nameof(StripChunk.TriangleAttributeCount);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<PolyChunkType, PolyChunk> ParentConverter => PolyChunkJsonConverter._globalPolyChunkConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _ignoreLight, new(PropertyTokenType.Bool, false) },
			{ _ignoreSpecular, new(PropertyTokenType.Bool, false) },
			{ _ignoreAmbient, new(PropertyTokenType.Bool, false) },
			{ _useAlpha, new(PropertyTokenType.Bool, false) },
			{ _doubleSide, new(PropertyTokenType.Bool, false) },
			{ _flatShading, new(PropertyTokenType.Bool, false) },
			{ _environmentMapping, new(PropertyTokenType.Bool, false) },
			{ _unknownAttribute, new(PropertyTokenType.Bool, false) },
			{ _strips, new(PropertyTokenType.Array, null) },
			{ _triangleAttributeCount, new(PropertyTokenType.Number, 0) },

		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(PolyChunkType key)
		{
			return key is >= PolyChunkType.Strip_Blank and <= PolyChunkType.Strip_HDTexDouble;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _ignoreLight:
				case _ignoreSpecular:
				case _ignoreAmbient:
				case _useAlpha:
				case _doubleSide:
				case _flatShading:
				case _environmentMapping:
				case _unknownAttribute:
					return reader.GetBoolean();
				case _strips:
					return JsonSerializer.Deserialize<ChunkStrip[]>(ref reader, options);
				case _triangleAttributeCount:
					return reader.GetInt32();
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override StripChunk CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			PolyChunkType type = (PolyChunkType)values[PolyChunkJsonConverter._type]!;
			int triangleAttributeCount = (int)values[_triangleAttributeCount]!;

			ChunkStrip[] strips = (ChunkStrip[]?)values[_strips]
				?? throw new InvalidDataException($"Strip chunk requires \"{_strips}\" property!");

			return new(type, strips, triangleAttributeCount)
			{
				IgnoreLight = (bool)values[_ignoreLight]!,
				IgnoreSpecular = (bool)values[_ignoreSpecular]!,
				IgnoreAmbient = (bool)values[_ignoreAmbient]!,
				UseAlpha = (bool)values[_useAlpha]!,
				DoubleSide = (bool)values[_doubleSide]!,
				FlatShading = (bool)values[_flatShading]!,
				EnvironmentMapping = (bool)values[_environmentMapping]!,
				UnknownAttribute = (bool)values[_unknownAttribute]!,
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, StripChunk value, JsonSerializerOptions options)
		{
			void writeBoolean(string name, bool value)
			{
				if(value)
				{
					writer.WriteBoolean(name, value);
				}
			}

			writeBoolean(_ignoreLight, value.IgnoreLight);
			writeBoolean(_ignoreSpecular, value.IgnoreSpecular);
			writeBoolean(_ignoreAmbient, value.IgnoreAmbient);
			writeBoolean(_useAlpha, value.UseAlpha);
			writeBoolean(_doubleSide, value.DoubleSide);
			writeBoolean(_flatShading, value.FlatShading);
			writeBoolean(_environmentMapping, value.EnvironmentMapping);
			writeBoolean(_unknownAttribute, value.UnknownAttribute);

			if(value.TriangleAttributeCount != 0)
			{
				writer.WriteNumber(_triangleAttributeCount, value.TriangleAttributeCount);
			}

			writer.WritePropertyName(_strips);
			JsonSerializer.Serialize(writer, value.Strips, options);
		}
	}
}
