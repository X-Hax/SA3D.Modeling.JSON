using SA3D.Common.Lookup;
using SA3D.Modeling.Animation;
using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Animation
{
	/// <summary>
	/// Json Converter for <see cref="Keyframes"/>
	/// </summary>
	public class KeyframesJsonConverter : SimpleJsonObjectConverter<Keyframes>
	{
		private const string _position = nameof(Keyframes.Position);
		private const string _eulerRotation = nameof(Keyframes.EulerRotation);
		private const string _scale = nameof(Keyframes.Scale);
		private const string _vector = nameof(Keyframes.Vector);
		private const string _vertex = nameof(Keyframes.Vertex);
		private const string _normal = nameof(Keyframes.Normal);
		private const string _target = nameof(Keyframes.Target);
		private const string _roll = nameof(Keyframes.Roll);
		private const string _angle = nameof(Keyframes.Angle);
		private const string _lightColor = nameof(Keyframes.LightColor);
		private const string _intensity = nameof(Keyframes.Intensity);
		private const string _spot = nameof(Keyframes.Spot);
		private const string _point = nameof(Keyframes.Point);
		private const string _quaternionRotation = nameof(Keyframes.QuaternionRotation);


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _position, new(PropertyTokenType.Object, null) },
			{ _eulerRotation, new(PropertyTokenType.Object, null) },
			{ _scale, new(PropertyTokenType.Object, null) },
			{ _vector, new(PropertyTokenType.Object, null) },
			{ _vertex, new(PropertyTokenType.Object, null) },
			{ _normal, new(PropertyTokenType.Object, null) },
			{ _target, new(PropertyTokenType.Object, null) },
			{ _roll, new(PropertyTokenType.Object, null) },
			{ _angle, new(PropertyTokenType.Object, null) },
			{ _lightColor, new(PropertyTokenType.Object, null) },
			{ _intensity, new(PropertyTokenType.Object, null) },
			{ _spot, new(PropertyTokenType.Object, null) },
			{ _point, new(PropertyTokenType.Object, null) },
			{ _quaternionRotation, new(PropertyTokenType.Object, null) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _position:
				case _eulerRotation:
				case _scale:
				case _vector:
				case _target:
					return JsonSerializer.Deserialize<SortedDictionary<uint, Vector3>>(ref reader, options);
				case _vertex:
				case _normal:
					return JsonSerializer.Deserialize<SortedDictionary<uint, LabeledArray<Vector3>>>(ref reader, options);
				case _roll:
				case _angle:
				case _intensity:
					return JsonSerializer.Deserialize<SortedDictionary<uint, float>>(ref reader, options);
				case _lightColor:
					return JsonSerializer.Deserialize<SortedDictionary<uint, Color>>(ref reader, options);
				case _spot:
					return JsonSerializer.Deserialize<SortedDictionary<uint, Spotlight>>(ref reader, options);
				case _point:
					return JsonSerializer.Deserialize<SortedDictionary<uint, Vector2>>(ref reader, options);
				case _quaternionRotation:
					return JsonSerializer.Deserialize<SortedDictionary<uint, Quaternion>>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override Keyframes Create(ReadOnlyDictionary<string, object?> values)
		{
			Keyframes result = new();

			void copyKeyframes<T>(string name, SortedDictionary<uint, T> target)
			{
				if(values[name] is not SortedDictionary<uint, T> source)
				{
					return;
				}

				foreach(KeyValuePair<uint, T> item in source)
				{
					target.Add(item.Key, item.Value);
				}
			}

			copyKeyframes(_position, result.Position);
			copyKeyframes(_eulerRotation, result.EulerRotation);
			copyKeyframes(_scale, result.Scale);
			copyKeyframes(_vector, result.Vector);
			copyKeyframes(_vertex, result.Vertex);
			copyKeyframes(_normal, result.Normal);
			copyKeyframes(_target, result.Target);
			copyKeyframes(_roll, result.Roll);
			copyKeyframes(_angle, result.Angle);
			copyKeyframes(_lightColor, result.LightColor);
			copyKeyframes(_intensity, result.Intensity);
			copyKeyframes(_spot, result.Spot);
			copyKeyframes(_point, result.Point);
			copyKeyframes(_quaternionRotation, result.QuaternionRotation);

			return result;
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, Keyframes value, JsonSerializerOptions options)
		{
			void writeKeyframes<T>(string name, SortedDictionary<uint, T> target)
			{
				if(target.Count == 0)
				{
					return;
				}

				writer.WritePropertyName(name);
				JsonSerializer.Serialize(writer, target, options);
			}

			writeKeyframes(_position, value.Position);
			writeKeyframes(_eulerRotation, value.EulerRotation);
			writeKeyframes(_scale, value.Scale);
			writeKeyframes(_vector, value.Vector);
			writeKeyframes(_vertex, value.Vertex);
			writeKeyframes(_normal, value.Normal);
			writeKeyframes(_target, value.Target);
			writeKeyframes(_roll, value.Roll);
			writeKeyframes(_angle, value.Angle);
			writeKeyframes(_lightColor, value.LightColor);
			writeKeyframes(_intensity, value.Intensity);
			writeKeyframes(_spot, value.Spot);
			writeKeyframes(_point, value.Point);
			writeKeyframes(_quaternionRotation, value.QuaternionRotation);
		}
	}
}
