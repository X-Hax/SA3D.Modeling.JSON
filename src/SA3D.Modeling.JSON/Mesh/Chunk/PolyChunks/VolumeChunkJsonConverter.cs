using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Chunk;
using SA3D.Modeling.Mesh.Chunk.PolyChunks;
using SA3D.Modeling.Mesh.Chunk.Structs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Chunk.PolyChunks
{
	/// <summary>
	/// Json converter for <see cref="VolumeChunk"/>
	/// </summary>
	public class VolumeChunkJsonConverter : ChildJsonObjectConverter<PolyChunkType, VolumeChunk, PolyChunk>
	{
		private const string _attributes = nameof(VolumeChunk.Attributes);
		private const string _polygons = nameof(VolumeChunk.Polygons);
		private const string _polygonAttributeCount = nameof(VolumeChunk.PolygonAttributeCount);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<PolyChunkType, PolyChunk> ParentConverter => PolyChunkJsonConverter._globalPolyChunkConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _attributes, new(PropertyTokenType.String, (byte)0) },
			{ _polygons, new(PropertyTokenType.Array, null) },
			{ _polygonAttributeCount, new(PropertyTokenType.Number, 0) },
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(PolyChunkType key)
		{
			return key is >= PolyChunkType.Volume_Polygon3 and <= PolyChunkType.Volume_Polygon4;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _attributes:
					return reader.GetString()!.HexToByte("Volume chunk attributes");
				case _polygons:
					PolyChunkType type = (PolyChunkType)values[PolyChunkJsonConverter._type]!;

					if(type == PolyChunkType.Volume_Polygon3)
					{
						return JsonSerializer.Deserialize<ChunkVolumeTriangle[]>(ref reader, options)!.Cast<IChunkVolumePolygon[]>().ToArray();
					}
					else if(type == PolyChunkType.Volume_Polygon4)
					{
						return JsonSerializer.Deserialize<ChunkVolumeQuad[]>(ref reader, options)!.Cast<IChunkVolumePolygon[]>().ToArray();
					}
					else if(type == PolyChunkType.Volume_Strip)
					{
						return JsonSerializer.Deserialize<ChunkVolumeStrip[]>(ref reader, options)!.Cast<IChunkVolumePolygon[]>().ToArray();
					}

					throw new InvalidOperationException("Cannot be reached; If reached, volume type somehow invalid.");

				case _polygonAttributeCount:
					return reader.GetInt32();
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override VolumeChunk CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			PolyChunkType type = (PolyChunkType)values[PolyChunkJsonConverter._type]!;
			int polygonAttributeCount = (int)values[_polygonAttributeCount]!;

			IChunkVolumePolygon[] polygons = (IChunkVolumePolygon[]?)values[_polygons]
				?? throw new InvalidDataException($"Volume chunk requires \"{_polygons}\" property!");

			return new(type, polygons, polygonAttributeCount)
			{
				Attributes = (byte)values[_attributes]!
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, VolumeChunk value, JsonSerializerOptions options)
		{
			if(value.Attributes != 0)
			{
				writer.WriteString(_attributes, value.Attributes.ToString("X", CultureInfo.InvariantCulture));
			}

			if(value.PolygonAttributeCount != 0)
			{
				writer.WriteNumber(_polygonAttributeCount, value.PolygonAttributeCount);
			}

			writer.WritePropertyName(_polygons);
			JsonSerializer.Serialize(writer, value.Polygons, options);
		}
	}
}
