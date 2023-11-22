using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Chunk;
using SA3D.Modeling.Mesh.Chunk.PolyChunks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Chunk.PolyChunks
{
	/// <summary>
	/// Json converter for <see cref="SpecularExponentChunk"/>
	/// </summary>
	public class SpecularExponentChunkJsonConverter : ChildJsonObjectConverter<PolyChunkType, SpecularExponentChunk, PolyChunk>
	{
		private const string _specularExponent = nameof(SpecularExponentChunk.SpecularExponent);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<PolyChunkType, PolyChunk> ParentConverter => PolyChunkJsonConverter._globalPolyChunkConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _specularExponent, new(PropertyTokenType.Number, (byte)0) }
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(PolyChunkType key)
		{
			return key == PolyChunkType.SpecularExponent;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _specularExponent:
					return reader.GetByte();
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override SpecularExponentChunk CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				SpecularExponent = (byte)values[_specularExponent]!
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, SpecularExponentChunk value, JsonSerializerOptions options)
		{
			writer.WriteNumber(_specularExponent, value.SpecularExponent);
		}
	}
}
