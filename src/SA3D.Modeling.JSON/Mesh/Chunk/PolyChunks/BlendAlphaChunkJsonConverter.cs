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
	/// Json converter for <see cref="BlendAlphaChunk"/>
	/// </summary>
	public class BlendAlphaChunkJsonConverter : ChildJsonObjectConverter<PolyChunkType, BlendAlphaChunk, PolyChunk>
	{
		private const string _sourceAlpha = nameof(BlendAlphaChunk.SourceAlpha);
		private const string _destinationAlpha = nameof(BlendAlphaChunk.DestinationAlpha);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<PolyChunkType, PolyChunk> ParentConverter => PolyChunkJsonConverter._globalPolyChunkConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _sourceAlpha, new(PropertyTokenType.String, BlendMode.Zero) },
			{ _destinationAlpha, new(PropertyTokenType.String, BlendMode.Zero) }
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(PolyChunkType key)
		{
			return key == PolyChunkType.BlendAlpha;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _sourceAlpha:
				case _destinationAlpha:
					return JsonSerializer.Deserialize<BlendMode>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override BlendAlphaChunk CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				SourceAlpha = (BlendMode)values[_sourceAlpha]!,
				DestinationAlpha = (BlendMode)values[_destinationAlpha]!,
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, BlendAlphaChunk value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_sourceAlpha);
			JsonSerializer.Serialize(writer, value.SourceAlpha, options);

			writer.WritePropertyName(_destinationAlpha);
			JsonSerializer.Serialize(writer, value.DestinationAlpha, options);
		}
	}
}
