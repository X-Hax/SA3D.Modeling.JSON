using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Chunk;
using SA3D.Modeling.Mesh.Chunk.PolyChunks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Chunk.PolyChunks
{
	/// <summary>
	/// Json converter for <see cref="MaterialBumpChunk"/>
	/// </summary>
	public class MaterialBumpChunkJsonConverter : ChildJsonObjectConverter<PolyChunkType, MaterialBumpChunk, PolyChunk>
	{
		private const string _dx = nameof(MaterialBumpChunk.DX);
		private const string _dy = nameof(MaterialBumpChunk.DY);
		private const string _dz = nameof(MaterialBumpChunk.DZ);
		private const string _ux = nameof(MaterialBumpChunk.UX);
		private const string _uy = nameof(MaterialBumpChunk.UY);
		private const string _uz = nameof(MaterialBumpChunk.UZ);

		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<PolyChunkType, PolyChunk> ParentConverter => PolyChunkJsonConverter._globalPolyChunkConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _dx, new(PropertyTokenType.Number, (ushort)0u) },
			{ _dy, new(PropertyTokenType.Number, (ushort)0u) },
			{ _dz, new(PropertyTokenType.Number, (ushort)0u) },
			{ _ux, new(PropertyTokenType.Number, (ushort)0u) },
			{ _uy, new(PropertyTokenType.Number, (ushort)0u) },
			{ _uz, new(PropertyTokenType.Number, (ushort)0u) },
		});

		/// <inheritdoc/>
		protected override bool CheckTypeMatches(PolyChunkType key)
		{
			return key == PolyChunkType.Material_Bump;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _dx:
				case _dy:
				case _dz:
				case _ux:
				case _uy:
				case _uz:
					return reader.GetUInt16();
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override MaterialBumpChunk CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				DX = (ushort)values[_dx]!,
				DY = (ushort)values[_dy]!,
				DZ = (ushort)values[_dz]!,
				UX = (ushort)values[_ux]!,
				UY = (ushort)values[_uy]!,
				UZ = (ushort)values[_uz]!,
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, MaterialBumpChunk value, JsonSerializerOptions options)
		{
			writer.WriteNumber(_dx, value.DX);
			writer.WriteNumber(_dy, value.DY);
			writer.WriteNumber(_dz, value.DZ);
			writer.WriteNumber(_ux, value.UX);
			writer.WriteNumber(_uy, value.UY);
			writer.WriteNumber(_uz, value.UZ);
		}
	}
}
