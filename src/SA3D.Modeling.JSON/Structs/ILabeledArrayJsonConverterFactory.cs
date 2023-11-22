using SA3D.Common.Lookup;
using System;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SA3D.Modeling.JSON.Structs
{
	/// <summary>
	/// Json converter factory for writing <see cref="ILabeledArray{T}"/>.
	/// </summary>
	public class ILabeledArrayJsonConverterFactory : JsonConverterFactory
	{
		/// <inheritdoc/>
		public override bool CanConvert(Type typeToConvert)
		{
			return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(ILabeledArray<>);
		}

		/// <inheritdoc/>
		public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
		{
			Type elementType = typeToConvert.GetGenericArguments()[0];

			JsonConverter converter = (JsonConverter)Activator.CreateInstance(
				typeof(ILabeledArrayJsonConverter<>).MakeGenericType(
					new Type[] { elementType }),
				BindingFlags.Instance | BindingFlags.Public,
				binder: null,
				args: null,
				culture: null)!;

			return converter;
		}

		private class ILabeledArrayJsonConverter<T> : JsonConverter<ILabeledArray<T>>
		{
			public override ILabeledArray<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
			{
				throw new NotSupportedException("Cannot read interface!");
			}

			public override void Write(Utf8JsonWriter writer, ILabeledArray<T> value, JsonSerializerOptions options)
			{
				switch(value)
				{
					case LabeledArray<T> labeledArray:
						JsonSerializer.Serialize(writer, labeledArray, options);
						break;
					case LabeledReadOnlyArray<T> labeledReadonlyArray:
						JsonSerializer.Serialize(writer, labeledReadonlyArray, options);
						break;
					default:
						throw new NotSupportedException($"Unknown ILabeledArray Type: {value.GetType()}");
				}
			}
		}
	}
}
