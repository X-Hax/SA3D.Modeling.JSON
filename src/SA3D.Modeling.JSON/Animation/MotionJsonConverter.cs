using SA3D.Modeling.Animation;
using SA3D.Modeling.JSON.JsonBase;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Animation
{
	/// <summary>
	/// Json Converter for <see cref="Motion"/>
	/// </summary>
	public class MotionJsonConverter : SimpleJsonObjectConverter<Motion>
	{
		private const string _label = nameof(Motion.Label);
		private const string _nodeCount = nameof(Motion.NodeCount);
		private const string _interpolationMode = nameof(Motion.InterpolationMode);
		private const string _shortRot = nameof(Motion.ShortRot);
		private const string _keyframes = nameof(Motion.Keyframes);
		private const string _manualKeyframeTypes = nameof(Motion.ManualKeyframeTypes);


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _label, new(PropertyTokenType.String, string.Empty) },
			{ _nodeCount, new(PropertyTokenType.Number, 0u) },
			{ _interpolationMode, new(PropertyTokenType.String, InterpolationMode.Linear) },
			{ _shortRot, new(PropertyTokenType.Bool, false) },
			{ _manualKeyframeTypes, new(PropertyTokenType.String | PropertyTokenType.Number, default(KeyframeAttributes)) },
			{ _keyframes, new(PropertyTokenType.Object, null) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _label:
					return reader.GetString();
				case _nodeCount:
					return reader.GetUInt32();
				case _interpolationMode:
					return JsonSerializer.Deserialize<InterpolationMode>(ref reader, options);
				case _shortRot:
					return reader.GetBoolean();
				case _manualKeyframeTypes:
					return JsonSerializer.Deserialize<KeyframeAttributes>(ref reader, options);
				case _keyframes:
					return JsonSerializer.Deserialize<Dictionary<int, Keyframes>>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override Motion Create(ReadOnlyDictionary<string, object?> values)
		{
			Motion result = new()
			{
				Label = (string)values[_label]!,
				NodeCount = (uint)values[_nodeCount]!,
				InterpolationMode = (InterpolationMode)values[_interpolationMode]!,
				ShortRot = (bool)values[_shortRot]!,
				ManualKeyframeTypes = (KeyframeAttributes)values[_manualKeyframeTypes]!,
			};

			if(values[_keyframes] is not Dictionary<int, Keyframes> keyframes)
			{
				throw new InvalidDataException($"Motion requires \"{_keyframes}\" property");
			}

			foreach(KeyValuePair<int, Keyframes> item in keyframes)
			{
				result.Keyframes.Add(item.Key, item.Value);
			}

			return result;
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, Motion value, JsonSerializerOptions options)
		{
			writer.WriteString(_label, value.Label);

			if(value.NodeCount != 0)
			{
				writer.WriteNumber(_nodeCount, value.NodeCount);
			}

			if(value.InterpolationMode != InterpolationMode.Linear)
			{
				writer.WritePropertyName(_interpolationMode);
				JsonSerializer.Serialize(writer, value.InterpolationMode, options);
			}

			if(value.ShortRot)
			{
				writer.WriteBoolean(_shortRot, value.ShortRot);
			}

			if(value.ManualKeyframeTypes != default)
			{
				writer.WritePropertyName(_manualKeyframeTypes);
				JsonSerializer.Serialize(writer, value.ManualKeyframeTypes, options);
			}

			writer.WritePropertyName(_keyframes);
			JsonSerializer.Serialize(writer, value.Keyframes, options);
		}
	}
}
