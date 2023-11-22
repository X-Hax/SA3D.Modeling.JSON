using SA3D.Modeling.Animation;
using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.ObjectData;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Animation
{
	/// <summary>
	/// Json Converter for <see cref="NodeMotion"/>
	/// </summary>
	public class NodeMotionJsonConverter : SimpleJsonObjectConverter<NodeMotion>
	{
		private const string _label = nameof(NodeMotion.Label);
		private const string _model = nameof(NodeMotion.Model);
		private const string _animation = nameof(NodeMotion.Animation);


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _label, new(PropertyTokenType.String, string.Empty) },
			{ _model, new(PropertyTokenType.Object | PropertyTokenType.String, null) },
			{ _animation, new(PropertyTokenType.Object | PropertyTokenType.String, null) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _label:
					return reader.GetString();
				case _model:
					return JsonSerializer.Deserialize<Node>(ref reader, options);
				case _animation:
					return JsonSerializer.Deserialize<Motion>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override NodeMotion Create(ReadOnlyDictionary<string, object?> values)
		{
			Node model = (Node?)values[_model]
				?? throw new InvalidDataException($"NodeMotion requires \"{_model}\" property");

			Motion animation = (Motion?)values[_animation]
				?? throw new InvalidDataException($"NodeMotion requires \"{_animation}\" property");

			return new(model, animation)
			{
				Label = (string)values[_label]!
			};
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, NodeMotion value, JsonSerializerOptions options)
		{
			writer.WriteString(_label, value.Label);

			writer.WritePropertyName(_model);
			JsonSerializer.Serialize(writer, value.Model, options);

			writer.WritePropertyName(_animation);
			JsonSerializer.Serialize(writer, value.Animation, options);
		}
	}
}
