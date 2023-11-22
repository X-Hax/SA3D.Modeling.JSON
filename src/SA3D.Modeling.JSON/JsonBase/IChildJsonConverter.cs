using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.JsonBase
{
	/// <summary>
	/// Base interface for polymorphic converters.
	/// </summary>
	public interface IChildJsonConverter<T>
	{
		/// <summary>
		/// Property definitions added by the target type.
		/// </summary>
		public ReadOnlyDictionary<string, PropertyDefinition> PolyPropertyDefinitions { get; }

		/// <summary>
		/// Reads value specific values.
		/// </summary>
		/// <param name="reader">Reader responsible for reading</param>
		/// <param name="propertyName">Name of the property to read</param>
		/// <param name="values">Values read so far.</param>
		/// <param name="options">An object that specifies serialization options to use</param>
		/// <returns></returns>
		public object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options);

		/// <summary>
		/// Creates the value.
		/// </summary>
		/// <param name="values">The read values</param>
		/// <returns></returns>
		public T Create(ReadOnlyDictionary<string, object?> values);

		/// <summary>
		/// Writes value specific values.
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="value">The value to convert to JSON</param>
		/// <param name="options">An object that specifies serialization options to use</param>
		public void WriteValues(Utf8JsonWriter writer, T value, JsonSerializerOptions options);
	}
}
