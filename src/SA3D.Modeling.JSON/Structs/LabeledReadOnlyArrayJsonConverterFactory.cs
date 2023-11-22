using SA3D.Common.Lookup;
using SA3D.Modeling.JSON.JsonBase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SA3D.Modeling.JSON.Structs
{
	/// <summary>
	/// Json converter factory for <see cref="LabeledReadOnlyArray{T}"/>
	/// </summary>
	public class LabeledReadOnlyArrayJsonConverterFactory : JsonConverterFactory
	{
		/// <inheritdoc/>
		public override bool CanConvert(Type typeToConvert)
		{
			return typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(LabeledReadOnlyArray<>);
		}

		/// <inheritdoc/>
		public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
		{
			Type elementType = typeToConvert.GetGenericArguments()[0];

			JsonConverter converter = (JsonConverter)Activator.CreateInstance(
				typeof(JsonLabeledReadOnlyArrayConverter<>).MakeGenericType(
					new Type[] { elementType }),
				BindingFlags.Instance | BindingFlags.Public,
				binder: null,
				args: null,
				culture: null)!;

			return converter;
		}

		private class JsonLabeledReadOnlyArrayConverter<T> : SimpleJsonObjectConverter<LabeledReadOnlyArray<T>>
		{
			private const string _label = nameof(LabeledArray<T>.Label);
			private const string _array = nameof(LabeledArray<T>.Array);

			public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
			{
				{ _label, new(PropertyTokenType.String, "")},
				{ _array, new(PropertyTokenType.Array, null) }
			});

			protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
			{
				return propertyName switch
				{
					_label => reader.GetString(),
					_array => JsonSerializer.Deserialize<T[]>(ref reader, options),
					_ => throw new InvalidPropertyException(),
				};
			}

			protected override LabeledReadOnlyArray<T> Create(ReadOnlyDictionary<string, object?> values)
			{
				string label = (string)values[_label]!;
				T[] array = (T[]?)values[_array] ?? throw new InvalidDataException("Labeled array is missing required property \"Array\"!");

				return new(label, array);
			}

			protected override void WriteValues(Utf8JsonWriter writer, LabeledReadOnlyArray<T> value, JsonSerializerOptions options)
			{
				writer.WriteString(_label, value.Label);

				writer.WritePropertyName(_array);
				JsonSerializer.Serialize(writer, value.Array.ToArray(), options);
			}
		}
	}
}
