using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh;
using SA3D.Modeling.ObjectData;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Numerics;
using System.Text.Json;

namespace SA3D.Modeling.JSON.ObjectData
{
	/// <summary>
	/// Json converter for <see cref="Node"/>
	/// </summary>
	public class NodeJsonConverter : SimpleJsonObjectConverter<Node>
	{
		private const string _label = nameof(Node.Label);

		private const string _noPosition = nameof(Node.NoPosition);
		private const string _noRotation = nameof(Node.NoRotation);
		private const string _noScale = nameof(Node.NoScale);
		private const string _skipDraw = nameof(Node.SkipDraw);
		private const string _skipChildren = nameof(Node.SkipChildren);
		private const string _rotateZYX = nameof(Node.RotateZYX);
		private const string _noAnimate = nameof(Node.NoAnimate);
		private const string _noMorph = nameof(Node.NoMorph);
		private const string _useQuaternionRotation = nameof(Node.UseQuaternionRotation);

		private const string _position = nameof(Node.Position);
		private const string _eulerRotation = nameof(Node.EulerRotation);
		private const string _quaternionRotation = nameof(Node.QuaternionRotation);
		private const string _scale = nameof(Node.Scale);

		private const string _attach = nameof(Node.Attach);

		private const string _children = "Children";


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _label, new(PropertyTokenType.String, string.Empty) },
			{ _noPosition, new(PropertyTokenType.Bool, false) },
			{ _noRotation, new(PropertyTokenType.Bool, false) },
			{ _noScale, new(PropertyTokenType.Bool, false) },
			{ _skipDraw, new(PropertyTokenType.Bool, false) },
			{ _skipChildren, new(PropertyTokenType.Bool, false) },
			{ _rotateZYX, new(PropertyTokenType.Bool, false) },
			{ _noAnimate, new(PropertyTokenType.Bool, false) },
			{ _noMorph, new(PropertyTokenType.Bool, false) },
			{ _useQuaternionRotation, new(PropertyTokenType.Bool, false) },
			{ _position, new(PropertyTokenType.String, null) },
			{ _eulerRotation, new(PropertyTokenType.String, null) },
			{ _quaternionRotation, new(PropertyTokenType.String, null) },
			{ _scale, new(PropertyTokenType.String, null) },
			{ _attach, new(PropertyTokenType.Object | PropertyTokenType.String, null) },
			{ _children, new(PropertyTokenType.Array, null) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _label:
					return reader.GetString();
				case _noPosition:
				case _noRotation:
				case _noScale:
				case _skipDraw:
				case _skipChildren:
				case _rotateZYX:
				case _noAnimate:
				case _noMorph:
				case _useQuaternionRotation:
					return reader.GetBoolean();
				case _position:
				case _eulerRotation:
				case _scale:
					return JsonSerializer.Deserialize<Vector3>(ref reader, options);
				case _quaternionRotation:
					return JsonSerializer.Deserialize<Quaternion>(ref reader, options);
				case _attach:
					return JsonSerializer.Deserialize<Attach>(ref reader, options);
				case _children:
					return JsonSerializer.Deserialize<Node[]>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override Node Create(ReadOnlyDictionary<string, object?> values)
		{
			Node result = new()
			{
				Label = (string)values[_label]!,
				Attach = (Attach?)values[_attach],
				NoPosition = (bool)values[_noPosition]!,
				NoRotation = (bool)values[_noRotation]!,
				NoScale = (bool)values[_noScale]!,
				SkipDraw = (bool)values[_skipDraw]!,
				SkipChildren = (bool)values[_skipChildren]!,
				NoAnimate = (bool)values[_noAnimate]!,
				NoMorph = (bool)values[_noMorph]!,
				UseQuaternionRotation = (bool)values[_useQuaternionRotation]!,
			};

			result.SetRotationZYX((bool)values[_rotateZYX]!, Modeling.ObjectData.Enums.RotationUpdateMode.Keep);

			if(result.UseQuaternionRotation)
			{
				result.UpdateTransforms((Vector3?)values[_position], (Quaternion?)values[_quaternionRotation], (Vector3?)values[_scale]);
			}
			else
			{
				result.UpdateTransforms((Vector3?)values[_position], (Vector3?)values[_eulerRotation], (Vector3?)values[_scale]);
			}

			if(values[_children] is Node[] children)
			{
				foreach(Node child in children)
				{
					result.AppendChild(child);
				}
			}

			return result;
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, Node value, JsonSerializerOptions options)
		{
			writer.WriteString(_label, value.Label);

			void writeBoolean(string name, bool value)
			{
				if(value)
				{
					writer.WriteBoolean(name, value);
				}
			}

			writeBoolean(_noPosition, value.NoPosition);
			writeBoolean(_noRotation, value.NoRotation);
			writeBoolean(_noScale, value.NoScale);
			writeBoolean(_skipDraw, value.SkipDraw);
			writeBoolean(_skipChildren, value.SkipChildren);
			writeBoolean(_rotateZYX, value.RotateZYX);
			writeBoolean(_noAnimate, value.NoAnimate);
			writeBoolean(_noMorph, value.NoMorph);
			writeBoolean(_useQuaternionRotation, value.UseQuaternionRotation);

			if(value.Position != Vector3.Zero)
			{
				writer.WritePropertyName(_position);
				JsonSerializer.Serialize(writer, value.Position, options);
			}

			if(value.UseQuaternionRotation && value.QuaternionRotation != Quaternion.Identity)
			{
				writer.WritePropertyName(_quaternionRotation);
				JsonSerializer.Serialize(writer, value.QuaternionRotation, options);
			}
			else if(!value.UseQuaternionRotation && value.EulerRotation != Vector3.Zero)
			{
				writer.WritePropertyName(_eulerRotation);
				JsonSerializer.Serialize(writer, value.EulerRotation, options);
			}

			if(value.Scale != Vector3.One)
			{
				writer.WritePropertyName(_scale);
				JsonSerializer.Serialize(writer, value.Scale, options);
			}

			if(value.Attach != null)
			{
				writer.WritePropertyName(_attach);
				JsonSerializer.Serialize(writer, value.Attach, options);
			}

			if(value.ChildCount > 0)
			{
				writer.WritePropertyName(_children);
				JsonSerializer.Serialize(writer, value.GetChildren(), options);
			}
		}
	}
}
