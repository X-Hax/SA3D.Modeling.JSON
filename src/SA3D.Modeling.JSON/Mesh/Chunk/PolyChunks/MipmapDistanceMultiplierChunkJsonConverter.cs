using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Chunk;
using SA3D.Modeling.Mesh.Chunk.PolyChunks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Chunk.PolyChunks
{
	/// <summary>
	/// Json converter for <see cref="MipmapDistanceMultiplierChunk"/>
	/// </summary>
	public class MipmapDistanceMultiplierChunkJsonConverter : ChildJsonObjectConverter<PolyChunkType, MipmapDistanceMultiplierChunk, PolyChunk>
	{
		private const string _mipmapDistanceMultiplier = nameof(MipmapDistanceMultiplierChunk.MipmapDistanceMultiplier);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<PolyChunkType, PolyChunk> ParentConverter => PolyChunkJsonConverter._globalPolyChunkConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _mipmapDistanceMultiplier, new(PropertyTokenType.Number, 1f) }
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(PolyChunkType key)
		{
			return key == PolyChunkType.MipmapDistanceMultiplier;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _mipmapDistanceMultiplier:
					return reader.GetSingle();
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override MipmapDistanceMultiplierChunk CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				MipmapDistanceMultiplier = (float)values[_mipmapDistanceMultiplier]!
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, MipmapDistanceMultiplierChunk value, JsonSerializerOptions options)
		{
			writer.WriteNumber(_mipmapDistanceMultiplier, value.MipmapDistanceMultiplier);
		}
	}
}
