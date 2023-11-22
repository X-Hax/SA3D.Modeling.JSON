using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.JsonBase
{
	/// <summary>
	/// Simplified read and write calls for 
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public abstract class SimpleJsonObjectConverter<T> : JsonObjectConverter<T> where T : notnull
	{
		/// <summary>
		/// Read a value
		/// </summary>
		/// <param name="reader">Reader responsible for reading</param>
		/// <param name="propertyName">Name of the property to read</param>
		/// <param name="values">Values that have been read or their defaults.</param>
		/// <param name="options">An object that specifies serialization options to use</param>
		/// <returns></returns>
		protected abstract object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options);

		/// <summary>
		/// Create the object from the read values
		/// </summary>
		/// <param name="values">Values that have been read or their defaults.</param>
		/// <returns></returns>
		protected abstract T Create(ReadOnlyDictionary<string, object?> values);

		/// <inheritdoc/>
		protected override object? ReadValueRaw(ref Utf8JsonReader reader, ref JsonObjectReaderInstance readerInstance, string propertyName, JsonSerializerOptions options)
		{
			return ReadValue(ref reader, propertyName, readerInstance.Values, options);
		}

		/// <inheritdoc/>
		protected sealed override T CreateRaw(ref JsonObjectReaderInstance readerInstance)
		{
			return Create(readerInstance.Values);
		}
	}
}
