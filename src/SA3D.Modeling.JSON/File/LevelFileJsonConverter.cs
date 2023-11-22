using SA3D.Modeling.File;
using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.ObjectData;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.File
{
	/// <summary>
	/// Json converter for <see cref="LevelFile"/>
	/// </summary>
	public class LevelFileJsonConverter : SimpleJsonObjectConverter<LevelFile>
	{
		private const string _level = nameof(LevelFile.Level);
		private const string _metaData = nameof(LevelFile.MetaData);


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _level, new(PropertyTokenType.Object | PropertyTokenType.String, null) },
			{ _metaData, new(PropertyTokenType.Object, null) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _level:
					return JsonSerializer.Deserialize<LandTable>(ref reader, options);
				case _metaData:
					return JsonSerializer.Deserialize<MetaData>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override LevelFile Create(ReadOnlyDictionary<string, object?> values)
		{
			LandTable level = (LandTable?)values[_level]
				?? throw new InvalidDataException($"Levelfile requires \"{_level}\" property");

			MetaData metadata = (MetaData?)values[_metaData] ?? new();

			return new(level, metadata);
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, LevelFile value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_metaData);
			JsonSerializer.Serialize(writer, value.MetaData, options);

			writer.WritePropertyName(_level);
			JsonSerializer.Serialize(writer, value.Level, options);
		}
	}
}
