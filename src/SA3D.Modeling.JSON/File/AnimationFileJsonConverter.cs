using SA3D.Modeling.Animation;
using SA3D.Modeling.File;
using SA3D.Modeling.JSON.JsonBase;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.File
{
	/// <summary>
	/// Json converter for <see cref="AnimationFile"/>
	/// </summary>
	public class AnimationFileJsonConverter : SimpleJsonObjectConverter<AnimationFile>
	{
		private const string _animation = nameof(AnimationFile.Animation);
		private const string _metaData = nameof(AnimationFile.MetaData);


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _animation, new(PropertyTokenType.Object | PropertyTokenType.String, null) },
			{ _metaData, new(PropertyTokenType.Object, null) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _animation:
					return JsonSerializer.Deserialize<Motion>(ref reader, options);
				case _metaData:
					return JsonSerializer.Deserialize<MetaData>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override AnimationFile Create(ReadOnlyDictionary<string, object?> values)
		{
			Motion level = (Motion?)values[_animation]
				?? throw new InvalidDataException($"Animationfile requires \"{_animation}\" property");

			MetaData metadata = (MetaData?)values[_metaData] ?? new();

			return new(level, metadata);
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, AnimationFile value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_metaData);
			JsonSerializer.Serialize(writer, value.MetaData, options);

			writer.WritePropertyName(_animation);
			JsonSerializer.Serialize(writer, value.Animation, options);
		}
	}
}
