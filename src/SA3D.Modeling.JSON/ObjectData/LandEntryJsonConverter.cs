using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.ObjectData;
using SA3D.Modeling.ObjectData.Enums;
using SA3D.Modeling.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.ObjectData
{
	/// <summary>
	/// Json converter for <see cref="LandEntry"/>
	/// </summary>
	public class LandEntryJsonConverter : SimpleJsonObjectConverter<LandEntry>
	{
		private const string _model = nameof(LandEntry.Model);
		private const string _modelBounds = nameof(LandEntry.ModelBounds);
		private const string _surfaceAttributes = nameof(LandEntry.SurfaceAttributes);
		private const string _blockBit = nameof(LandEntry.BlockBit);
		private const string _unknown = nameof(LandEntry.Unknown);

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _model, new(PropertyTokenType.Object | PropertyTokenType.String, null) },
			{ _modelBounds, new(PropertyTokenType.Object, default(Bounds)) },
			{ _surfaceAttributes, new(PropertyTokenType.String | PropertyTokenType.Number, default(SurfaceAttributes)) },
			{ _blockBit, new(PropertyTokenType.String, 0u) },
			{ _unknown, new(PropertyTokenType.Number, 0u) },

		});


		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _model:
					return JsonSerializer.Deserialize<Node>(ref reader, options);
				case _modelBounds:
					return JsonSerializer.Deserialize<Bounds>(ref reader, options);
				case _surfaceAttributes:
					return JsonSerializer.Deserialize<SurfaceAttributes>(ref reader, options);
				case _blockBit:
					return reader.GetString()!.HexToUInt("Landentry blockbit");
				case _unknown:
					return reader.GetUInt32();
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override LandEntry Create(ReadOnlyDictionary<string, object?> values)
		{
			Node model = (Node?)values[_model]
				?? throw new InvalidDataException($"Landentry requires \"{_model}\" property");

			return new(model, (SurfaceAttributes)values[_surfaceAttributes]!)
			{
				ModelBounds = (Bounds)values[_modelBounds]!,
				BlockBit = (uint)values[_blockBit]!,
				Unknown = (uint)values[_unknown]!
			};
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, LandEntry value, JsonSerializerOptions options)
		{
			if(value.BlockBit != 0)
			{
				writer.WriteString(_blockBit, value.BlockBit.ToString("X", NumberFormatInfo.InvariantInfo));
			}

			if(value.Unknown != 0)
			{
				writer.WriteNumber(_unknown, value.Unknown);
			}

			writer.WritePropertyName(_surfaceAttributes);
			JsonSerializer.Serialize(writer, value.SurfaceAttributes, options);

			writer.WritePropertyName(_modelBounds);
			JsonSerializer.Serialize(writer, value.ModelBounds, options);

			writer.WritePropertyName(_model);
			JsonSerializer.Serialize(writer, value.Model, options);
		}
	}
}
