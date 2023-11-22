using SA3D.Modeling.File;
using SA3D.Modeling.JSON.JsonBase;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.File
{
	/// <summary>
	/// Json converter for <see cref="MetaData"/>
	/// </summary>
	public class MetaDataJsonConverter : SimpleJsonObjectConverter<MetaData>
	{
		private const string _author = nameof(MetaData.Author);
		private const string _description = nameof(MetaData.Description);
		private const string _actionName = nameof(MetaData.ActionName);
		private const string _objectName = nameof(MetaData.ObjectName);
		private const string _animFiles = nameof(MetaData.AnimFiles);
		private const string _morphFiles = nameof(MetaData.MorphFiles);

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _author, new(PropertyTokenType.String, null) },
			{ _description, new(PropertyTokenType.String, null) },
			{ _actionName, new(PropertyTokenType.String, null) },
			{ _objectName, new(PropertyTokenType.String, null) },
			{ _animFiles, new(PropertyTokenType.Array, null) },
			{ _morphFiles, new(PropertyTokenType.Array, null) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _author:
				case _description:
				case _actionName:
				case _objectName:
					return reader.GetString();
				case _animFiles:
				case _morphFiles:
					return JsonSerializer.Deserialize<string[]>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override MetaData Create(ReadOnlyDictionary<string, object?> values)
		{
			MetaData result = new()
			{
				Author = _author,
				Description = _description,
				ActionName = _actionName,
				ObjectName = _objectName,
			};

			if(values[_animFiles] is string[] animFiles)
			{
				result.AnimFiles.AddRange(animFiles);
			}

			if(values[_morphFiles] is string[] morphFiles)
			{
				result.MorphFiles.AddRange(morphFiles);
			}

			return result;
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, MetaData value, JsonSerializerOptions options)
		{
			void writeString(string name, string? value)
			{
				if(value != null)
				{
					writer.WriteString(name, value);
				}
			}

			writeString(_author, value.Author);
			writeString(_description, value.Description);
			writeString(_actionName, value.ActionName);
			writeString(_objectName, value.ObjectName);

			if(value.AnimFiles.Count != 0)
			{
				writer.WritePropertyName(_animFiles);
				JsonSerializer.Serialize(writer, value.AnimFiles, options);
			}

			if(value.MorphFiles.Count != 0)
			{
				writer.WritePropertyName(_morphFiles);
				JsonSerializer.Serialize(writer, value.MorphFiles, options);
			}
		}
	}
}
