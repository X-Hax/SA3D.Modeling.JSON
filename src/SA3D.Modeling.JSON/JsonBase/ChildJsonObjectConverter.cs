using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.JsonBase
{
	/// <summary>
	/// Converter for inheriting types.
	/// </summary>
	/// <typeparam name="TKey">Type of the key.</typeparam>
	/// <typeparam name="TTarget">Type of object to convert.</typeparam>
	/// <typeparam name="TBase">The Polymorphic type to handle.</typeparam>
	public abstract class ChildJsonObjectConverter<TKey, TTarget, TBase> : JsonObjectConverter<TTarget>, IChildJsonConverter<TBase> 
		where TTarget : notnull, TBase 
		where TBase : notnull 
		where TKey : notnull
	{
		/// <summary>
		/// Returns the parent converter.
		/// </summary>
		protected abstract ParentJsonObjectConverter<TKey, TBase> ParentConverter { get; }

		/// <summary>
		/// Attach specific property definitions.
		/// </summary>
		protected abstract ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; }


		/// <summary>
		/// Checks whether the key matches that of the child object type.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		protected abstract bool CheckTypeMatches(TKey key);

		/// <summary>
		/// Reads target specific values.
		/// </summary>
		/// <param name="reader">Reader responsible for reading</param>
		/// <param name="propertyName">Name of the property to read</param>
		/// <param name="values">Values read so far.</param>
		/// <param name="options">An object that specifies serialization options to use</param>
		/// <returns></returns>
		protected abstract object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options);

		/// <summary>
		/// Creates the target.
		/// </summary>
		/// <param name="values">The read values</param>
		/// <returns></returns>
		protected abstract TTarget CreateTarget(ReadOnlyDictionary<string, object?> values);

		/// <summary>
		/// Writes target specific values.
		/// </summary>
		/// <param name="writer">The writer to write to</param>
		/// <param name="value">The value to convert to JSON</param>
		/// <param name="options">An object that specifies serialization options to use</param>
		protected abstract void WriteTargetValues(Utf8JsonWriter writer, TTarget value, JsonSerializerOptions options);

		#region Interface

		ReadOnlyDictionary<string, PropertyDefinition> IChildJsonConverter<TBase>.PolyPropertyDefinitions => TargetPropertyDefinitions;

		object? IChildJsonConverter<TBase>.ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			return ReadTargetValue(ref reader, propertyName, values, options);
		}

		TBase IChildJsonConverter<TBase>.Create(ReadOnlyDictionary<string, object?> values)
		{
			return CreateTarget(values);
		}

		void IChildJsonConverter<TBase>.WriteValues(Utf8JsonWriter writer, TBase value, JsonSerializerOptions options)
		{
			if(value is TTarget target)
			{
				WriteTargetValues(writer, target, options);
			}
			else
			{
				throw new ArgumentException("Value is not target type!");
			}
		}

		#endregion

		#region Implementations of the base class

		/// <inheritdoc/>
		public sealed override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; }


		/// <summary>
		/// Base constructor.
		/// </summary>
		protected ChildJsonObjectConverter()
		{
			Dictionary<string, PropertyDefinition> propertyDefinitions = new(ParentConverter.PropertyDefinitions);

			foreach(KeyValuePair<string, PropertyDefinition> pair in TargetPropertyDefinitions)
			{
				propertyDefinitions.Add(pair.Key, pair.Value);
			}

			PropertyDefinitions = new(propertyDefinitions);
		}

		/// <inheritdoc/>
		protected sealed override object? ReadValueRaw(ref Utf8JsonReader reader, ref JsonObjectReaderInstance readerInstance, string propertyName, JsonSerializerOptions options)
		{
			try
			{
				return ParentConverter.ReadBaseValue(ref reader, propertyName, readerInstance.Values, options);
			}
			catch(InvalidPropertyException) { }

			return ReadTargetValue(ref reader, propertyName, readerInstance.Values, options);
		}

		/// <inheritdoc/>
		protected sealed override TTarget CreateRaw(ref JsonObjectReaderInstance readerInstance)
		{
			TKey type = (TKey?)readerInstance.Values[ParentConverter.KeyPropertyName]
				?? throw new InvalidDataException($"{typeof(TTarget).Name} has no \"{ParentConverter.KeyPropertyName}\" property, cannot create attach!");

			if(!CheckTypeMatches(type))
			{
				throw new InvalidDataException($"{typeof(TTarget).Name} is not of type {type}!");
			}

			return CreateTarget(readerInstance.Values);
		}

		/// <inheritdoc/>
		protected sealed override void WriteValues(Utf8JsonWriter writer, TTarget value, JsonSerializerOptions options)
		{
			ParentConverter.WriteBaseValues(writer, value, options);
			WriteTargetValues(writer, value, options);
		}

		#endregion
	}
}
