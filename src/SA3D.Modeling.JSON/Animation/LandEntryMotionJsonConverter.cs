using SA3D.Modeling.Animation;
using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.ObjectData;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Animation
{
	/// <summary>
	/// Json Converter for <see cref="LandEntryMotion"/>
	/// </summary>
	public class LandEntryMotionJsonConverter : SimpleJsonObjectConverter<LandEntryMotion>
	{
		private const string _frame = nameof(LandEntryMotion.Frame);
		private const string _step = nameof(LandEntryMotion.Step);
		private const string _maxFrame = nameof(LandEntryMotion.MaxFrame);
		private const string _model = nameof(LandEntryMotion.Model);
		private const string _nodeMotion = nameof(LandEntryMotion.NodeMotion);
		private const string _textureListPointer = nameof(LandEntryMotion.TextureListPointer);


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _frame, new(PropertyTokenType.Number, 0f) },
			{ _step, new(PropertyTokenType.Number, 0f) },
			{ _maxFrame, new(PropertyTokenType.Number, 0f) },
			{ _model, new(PropertyTokenType.Object | PropertyTokenType.String, null) },
			{ _nodeMotion, new(PropertyTokenType.Object | PropertyTokenType.String, null) },
			{ _textureListPointer, new(PropertyTokenType.String, 0u) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _frame:
				case _step:
				case _maxFrame:
					return reader.GetSingle();
				case _model:
					return JsonSerializer.Deserialize<Node>(ref reader, options);
				case _nodeMotion:
					return JsonSerializer.Deserialize<NodeMotion>(ref reader, options);
				case _textureListPointer:
					return reader.GetString()!.HexToUInt("Landentry motion texlistptr");
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override LandEntryMotion Create(ReadOnlyDictionary<string, object?> values)
		{
			Node model = (Node?)values[_model]
				?? throw new InvalidDataException($"Landentry motion requires \"{_model}\" property");

			NodeMotion nodeMotion = (NodeMotion?)values[_nodeMotion]
				?? throw new InvalidDataException($"Landentry motion requires \"{_nodeMotion}\" property");

			return new(
				(float)values[_frame]!,
				(float)values[_step]!,
				(float)values[_maxFrame]!,
				model,
				nodeMotion,
				(uint)values[_textureListPointer]!);
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, LandEntryMotion value, JsonSerializerOptions options)
		{
			writer.WriteNumber(_frame, value.Frame);
			writer.WriteNumber(_step, value.Step);
			writer.WriteNumber(_maxFrame, value.MaxFrame);

			if(value.TextureListPointer != 0)
			{
				writer.WriteString(_textureListPointer, value.TextureListPointer.ToString("X", CultureInfo.InvariantCulture));
			}

			writer.WritePropertyName(_model);
			JsonSerializer.Serialize(writer, value.Model, options);

			writer.WritePropertyName(_nodeMotion);
			JsonSerializer.Serialize(writer, value.NodeMotion, options);
		}
	}
}
