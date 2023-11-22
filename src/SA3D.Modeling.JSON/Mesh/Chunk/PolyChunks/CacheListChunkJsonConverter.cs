using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Chunk;
using SA3D.Modeling.Mesh.Chunk.PolyChunks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Chunk.PolyChunks
{
	/// <summary>
	/// Json converter for <see cref="CacheListChunk"/>
	/// </summary>
	public class CacheListChunkJsonConverter : ChildJsonObjectConverter<PolyChunkType, CacheListChunk, PolyChunk>
	{
		private const string _list = nameof(CacheListChunk.List);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<PolyChunkType, PolyChunk> ParentConverter => PolyChunkJsonConverter._globalPolyChunkConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _list, new(PropertyTokenType.Number, (byte)0) }
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(PolyChunkType key)
		{
			return key == PolyChunkType.CacheList;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _list:
					return reader.GetByte();
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override CacheListChunk CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				List = (byte)values[_list]!
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, CacheListChunk value, JsonSerializerOptions options)
		{
			writer.WriteNumber(_list, value.List);
		}
	}
}
