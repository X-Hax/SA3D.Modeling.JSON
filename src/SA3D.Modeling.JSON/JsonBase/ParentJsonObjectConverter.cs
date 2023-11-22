using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.JsonBase
{
	/// <summary>
	/// Base class json converter for polymorphic base types.
	/// </summary>
	/// <typeparam name="TKey">Type of the key.</typeparam>
	/// <typeparam name="TBase">The Polymorphic type to handle.</typeparam>
	public abstract class ParentJsonObjectConverter<TKey, TBase> : JsonObjectConverter<TBase> where TKey : notnull where TBase : notnull
	{
		private Dictionary<TKey, IChildJsonConverter<TBase>>? _converters = null;

		/// <summary>
		/// Property name for the type key.
		/// </summary>
		protected internal abstract string KeyPropertyName { get; }

		/// <summary>
		/// Reads values from the base type.
		/// </summary>
		/// <param name="reader">Reader responsible for reading</param>
		/// <param name="propertyName">Name of the property to read</param>
		/// <param name="values">Values read so far.</param>
		/// <param name="options">An object that specifies serialization options to use</param>
		/// <returns></returns>
		protected internal abstract object? ReadBaseValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options);

		/// <summary>
		/// Writes values for the base type.
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="value">The value to convert to JSON</param>
		/// <param name="options">An object that specifies serialization options to use</param>
		/// <returns></returns>
		protected internal abstract void WriteBaseValues(Utf8JsonWriter writer, TBase value, JsonSerializerOptions options);

		/// <summary>
		/// Creates object for when the base type is used.
		/// </summary>
		/// <param name="values">The values that were read.</param>
		/// <returns></returns>
		protected abstract TBase CreateBase(ReadOnlyDictionary<string, object?> values);

		/// <summary>
		/// Returns the key of the value.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		protected abstract TKey GetKeyFromValue(TBase value);

		/// <summary>
		/// Creates and returns the converter LUT.
		/// </summary>
		/// <returns></returns>
		protected abstract Dictionary<TKey, IChildJsonConverter<TBase>> CreateConverters();


		/// <inheritdoc/>
		protected sealed override object? ReadValueRaw(ref Utf8JsonReader reader, ref JsonObjectReaderInstance readerInstance, string propertyName, JsonSerializerOptions options)
		{
			try
			{
				object? result = ReadBaseValue(ref reader, propertyName, readerInstance.Values, options);

				if(propertyName == KeyPropertyName)
				{
					if(readerInstance.CurrentChildKey != null)
					{
						throw new InvalidDataException($"{typeof(TBase).Name} has multiple \"{propertyName}\" properties!");
					}
					else
					{
						TKey key = (TKey)result!;
						readerInstance.CurrentChildKey = key;
						_converters ??= CreateConverters();

						if(_converters.TryGetValue(key, out IChildJsonConverter<TBase>? converter))
						{
							readerInstance.CurrentChildConverter = converter;
							readerInstance.AddPropertyDefinitions(converter.PolyPropertyDefinitions);
						}
					}
				}

				return result;
			}
			catch(InvalidPropertyException) { }

			if(readerInstance.CurrentChildKey == null)
			{
				throw new InvalidDataException($"{typeof(TBase).Name} has no \"{KeyPropertyName}\" property, cannot read further!");
			}
			else if(readerInstance.CurrentChildConverter is not IChildJsonConverter<TBase> converter)
			{
				throw new InvalidPropertyException();
			}
			else
			{
				return converter.ReadValue(ref reader, propertyName, readerInstance.Values, options);
			}
		}

		/// <inheritdoc/>
		protected sealed override TBase CreateRaw(ref JsonObjectReaderInstance readerInstance)
		{
			if(readerInstance.CurrentChildKey == null)
			{
				throw new InvalidDataException($"{typeof(TBase).Name} has no \"{KeyPropertyName}\" property, cannot create!");
			}
			else if(readerInstance.CurrentChildConverter is not IChildJsonConverter<TBase> converter)
			{
				return CreateBase(readerInstance.Values);
			}
			else
			{
				return converter.Create(readerInstance.Values);
			}
		}

		/// <inheritdoc/>
		protected sealed override void WriteValues(Utf8JsonWriter writer, TBase value, JsonSerializerOptions options)
		{
			WriteBaseValues(writer, value, options);

			_converters ??= CreateConverters();
			if(_converters.TryGetValue(GetKeyFromValue(value), out IChildJsonConverter<TBase>? converter))
			{
				converter.WriteValues(writer, value, options);
			}
		}
	}
}
