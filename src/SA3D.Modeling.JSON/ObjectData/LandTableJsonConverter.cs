using SA3D.Common.Lookup;
using SA3D.Modeling.Animation;
using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.ObjectData;
using SA3D.Modeling.ObjectData.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.ObjectData
{
	/// <summary>
	/// Json converter for <see cref="LandTable"/>
	/// </summary>
	public class LandTableJsonConverter : SimpleJsonObjectConverter<LandTable>
	{
		private const string _label = nameof(LandTable.Label);
		private const string _format = nameof(LandTable.Format);
		private const string _drawDistance = nameof(LandTable.DrawDistance);
		private const string _textureFileName = nameof(LandTable.TextureFileName);
		private const string _texListPtr = nameof(LandTable.TexListPtr);
		private const string _attributes = nameof(LandTable.Attributes);
		private const string _geometry = nameof(LandTable.Geometry);
		private const string _geometryAnimations = nameof(LandTable.GeometryAnimations);


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _label, new(PropertyTokenType.String, string.Empty) },
			{ _format, new(PropertyTokenType.String, null) },
			{ _drawDistance, new(PropertyTokenType.Number, 0f) },
			{ _textureFileName, new(PropertyTokenType.String, null, true) },
			{ _texListPtr, new(PropertyTokenType.String, 0) },
			{ _attributes, new(PropertyTokenType.String, default(LandtableAttributes)) },
			{ _geometry, new(PropertyTokenType.Object | PropertyTokenType.String, null) },
			{ _geometryAnimations, new(PropertyTokenType.Object | PropertyTokenType.String, null) },
		});


		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _label:
					return reader.GetString();
				case _format:
					return JsonSerializer.Deserialize<ModelFormat>(ref reader, options);
				case _drawDistance:
					return reader.GetSingle();
				case _textureFileName:
					return reader.GetString();
				case _texListPtr:
					return reader.GetString()!.HexToUInt("Texture list pointer");
				case _attributes:
					return JsonSerializer.Deserialize<LandtableAttributes>(ref reader, options);
				case _geometry:
					return JsonSerializer.Deserialize<LabeledArray<LandEntry>>(ref reader, options);
				case _geometryAnimations:
					return JsonSerializer.Deserialize<LabeledArray<LandEntryMotion>>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override LandTable Create(ReadOnlyDictionary<string, object?> values)
		{
			LabeledArray<LandEntry> geometry = (LabeledArray<LandEntry>?)values[_geometry]
				?? throw new InvalidDataException($"Landtable requires \"{_geometry}\" property");

			ModelFormat format = (ModelFormat?)values[_format]
				?? throw new InvalidDataException($"Landtable requires \"{_format}\" property");

			LandTable result = new(geometry, format)
			{
				Label = (string)values[_label]!,
				Attributes = (LandtableAttributes)values[_attributes]!,
				DrawDistance = (float)values[_drawDistance]!,
				TextureFileName = (string?)values[_textureFileName],
				TexListPtr = (uint)values[_texListPtr]!,
			};

			if(values[_geometryAnimations] is LabeledArray<LandEntryMotion> anims)
			{
				result.GeometryAnimations = anims;
			}

			return result;
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, LandTable value, JsonSerializerOptions options)
		{
			writer.WriteString(_label, value.Label);

			writer.WritePropertyName(_format);
			JsonSerializer.Serialize(writer, value.Format, options);

			writer.WriteNumber(_drawDistance, value.DrawDistance);

			if(value.TextureFileName != null)
			{
				writer.WriteString(_textureFileName, value.TextureFileName);
			}

			if(value.TexListPtr != 0)
			{
				writer.WriteString(_texListPtr, value.TexListPtr.ToString("X", NumberFormatInfo.InvariantInfo));
			}

			if(value.Attributes != default)
			{
				writer.WritePropertyName(_attributes);
				JsonSerializer.Serialize(writer, value.Format, options);
			}

			writer.WritePropertyName(_geometry);
			JsonSerializer.Serialize(writer, value.Geometry, options);

			if(value.GeometryAnimations.Length > 0)
			{
				writer.WritePropertyName(_geometryAnimations);
				JsonSerializer.Serialize(writer, value.GeometryAnimations, options);
			}
		}
	}
}
