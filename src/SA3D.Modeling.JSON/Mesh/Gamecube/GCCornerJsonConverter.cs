using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Gamecube;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Gamecube
{
	/// <summary>
	/// Json converter for <see cref="GCCorner"/>
	/// </summary>
	public class GCCornerJsonConverter : SimpleJsonObjectConverter<GCCorner>
	{
		private const string _positionMatrixIDIndex = nameof(GCCorner.PositionMatrixIDIndex);
		private const string _positionIndex = nameof(GCCorner.PositionIndex);
		private const string _normalIndex = nameof(GCCorner.NormalIndex);
		private const string _color0Index = nameof(GCCorner.Color0Index);
		private const string _color1Index = nameof(GCCorner.Color1Index);
		private const string _texCoord0Index = nameof(GCCorner.TexCoord0Index);
		private const string _texCoord1Index = nameof(GCCorner.TexCoord1Index);
		private const string _texCoord2Index = nameof(GCCorner.TexCoord2Index);
		private const string _texCoord3Index = nameof(GCCorner.TexCoord3Index);
		private const string _texCoord4Index = nameof(GCCorner.TexCoord4Index);
		private const string _texCoord5Index = nameof(GCCorner.TexCoord5Index);
		private const string _texCoord6Index = nameof(GCCorner.TexCoord6Index);
		private const string _texCoord7Index = nameof(GCCorner.TexCoord7Index);


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _positionMatrixIDIndex, new(PropertyTokenType.Number, (ushort)0u) },
			{ _positionIndex, new(PropertyTokenType.Number, (ushort)0u) },
			{ _normalIndex, new(PropertyTokenType.Number, (ushort)0u) },
			{ _color0Index, new(PropertyTokenType.Number, (ushort)0u) },
			{ _color1Index, new(PropertyTokenType.Number, (ushort)0u) },
			{ _texCoord0Index, new(PropertyTokenType.Number, (ushort)0u) },
			{ _texCoord1Index, new(PropertyTokenType.Number, (ushort)0u) },
			{ _texCoord2Index, new(PropertyTokenType.Number, (ushort)0u) },
			{ _texCoord3Index, new(PropertyTokenType.Number, (ushort)0u) },
			{ _texCoord4Index, new(PropertyTokenType.Number, (ushort)0u) },
			{ _texCoord5Index, new(PropertyTokenType.Number, (ushort)0u) },
			{ _texCoord6Index, new(PropertyTokenType.Number, (ushort)0u) },
			{ _texCoord7Index, new(PropertyTokenType.Number, (ushort)0u) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _positionMatrixIDIndex:
				case _positionIndex:
				case _normalIndex:
				case _color0Index:
				case _color1Index:
				case _texCoord0Index:
				case _texCoord1Index:
				case _texCoord2Index:
				case _texCoord3Index:
				case _texCoord4Index:
				case _texCoord5Index:
				case _texCoord6Index:
				case _texCoord7Index:
					return reader.GetUInt16();
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override GCCorner Create(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				PositionMatrixIDIndex = (ushort)values[_positionMatrixIDIndex]!,
				PositionIndex = (ushort)values[_positionIndex]!,
				NormalIndex = (ushort)values[_normalIndex]!,
				Color0Index = (ushort)values[_color0Index]!,
				Color1Index = (ushort)values[_color1Index]!,
				TexCoord0Index = (ushort)values[_texCoord0Index]!,
				TexCoord1Index = (ushort)values[_texCoord1Index]!,
				TexCoord2Index = (ushort)values[_texCoord2Index]!,
				TexCoord3Index = (ushort)values[_texCoord3Index]!,
				TexCoord4Index = (ushort)values[_texCoord4Index]!,
				TexCoord5Index = (ushort)values[_texCoord5Index]!,
				TexCoord6Index = (ushort)values[_texCoord6Index]!,
				TexCoord7Index = (ushort)values[_texCoord7Index]!,
			};
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, GCCorner value, JsonSerializerOptions options)
		{
			void writeNumber(string name, ushort number)
			{
				if(number != 0)
				{
					writer.WriteNumber(name, number);
				}
			}

			writeNumber(_positionMatrixIDIndex, value.PositionMatrixIDIndex);
			writeNumber(_positionIndex, value.PositionIndex);
			writeNumber(_normalIndex, value.NormalIndex);
			writeNumber(_color0Index, value.Color0Index);
			writeNumber(_color1Index, value.Color1Index);
			writeNumber(_texCoord0Index, value.TexCoord0Index);
			writeNumber(_texCoord1Index, value.TexCoord1Index);
			writeNumber(_texCoord2Index, value.TexCoord2Index);
			writeNumber(_texCoord3Index, value.TexCoord3Index);
			writeNumber(_texCoord4Index, value.TexCoord4Index);
			writeNumber(_texCoord5Index, value.TexCoord5Index);
			writeNumber(_texCoord6Index, value.TexCoord6Index);
			writeNumber(_texCoord7Index, value.TexCoord7Index);
		}
	}
}
