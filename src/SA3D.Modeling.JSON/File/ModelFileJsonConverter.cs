using SA3D.Modeling.File;
using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.ObjectData;
using SA3D.Modeling.ObjectData.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.File
{
	/// <summary>
	/// Json converter for <see cref="ModelFile"/>
	/// </summary>
	public class ModelFileJsonConverter : SimpleJsonObjectConverter<ModelFile>
	{
		private const string _njFile = nameof(ModelFile.NJFile);
		private const string _format = nameof(ModelFile.Format);
		private const string _model = nameof(ModelFile.Model);
		private const string _metaData = nameof(ModelFile.MetaData);


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _njFile, new(PropertyTokenType.Bool, false) },
			{ _format, new(PropertyTokenType.String, null) },
			{ _model, new(PropertyTokenType.Object | PropertyTokenType.String, null) },
			{ _metaData, new(PropertyTokenType.Object, null) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _njFile:
					return reader.GetBoolean();
				case _format:
					return JsonSerializer.Deserialize<ModelFormat>(ref reader, options);
				case _model:
					return JsonSerializer.Deserialize<Node>(ref reader, options);
				case _metaData:
					return JsonSerializer.Deserialize<MetaData>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override ModelFile Create(ReadOnlyDictionary<string, object?> values)
		{
			bool njFile = (bool)values[_njFile]!;

			ModelFormat format = (ModelFormat?)values[_format]
				?? throw new InvalidDataException($"Modelfile requires \"{_format}\" property");

			Node model = (Node?)values[_model]
				?? throw new InvalidDataException($"Modelfile requires \"{_model}\" property");

			MetaData metadata = (MetaData?)values[_metaData] ?? new();

			return new(format, model, metadata, njFile);
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, ModelFile value, JsonSerializerOptions options)
		{
			if(value.NJFile)
			{
				writer.WriteBoolean(_njFile, value.NJFile);
			}

			writer.WritePropertyName(_format);
			JsonSerializer.Serialize(writer, value.Format, options);

			writer.WritePropertyName(_metaData);
			JsonSerializer.Serialize(writer, value.MetaData, options);

			writer.WritePropertyName(_model);
			JsonSerializer.Serialize(writer, value.Model, options);
		}
	}
}
