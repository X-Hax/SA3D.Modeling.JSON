using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Gamecube.Enums;
using SA3D.Modeling.Mesh.Gamecube.Parameters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Gamecube.Parameters
{
	/// <summary>
	/// Json converter for <see cref="GCVertexFormatParameter"/>
	/// </summary>
	public class GCVertexFormatParameterJsonConverter : ChildJsonObjectConverter<GCParameterType, GCVertexFormatParameter, IGCParameter>
	{
		private const string _vertexType = nameof(GCVertexFormatParameter.VertexType);
		private const string _vertexStructType = nameof(GCVertexFormatParameter.VertexStructType);
		private const string _vertexDataType = nameof(GCVertexFormatParameter.VertexDataType);
		private const string _attributes = nameof(GCVertexFormatParameter.Attributes);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<GCParameterType, IGCParameter> ParentConverter => GCParameterJsonConverter._globalGCParameterConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _vertexType, new(PropertyTokenType.String, null) },
			{ _vertexStructType, new(PropertyTokenType.String, null) },
			{ _vertexDataType, new(PropertyTokenType.String, null) },
			{ _attributes, new(PropertyTokenType.String, (byte)0) },
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(GCParameterType key)
		{
			return key == GCParameterType.VertexFormat;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _vertexType:
					return JsonSerializer.Deserialize<GCVertexType>(ref reader, options);
				case _vertexStructType:
					return JsonSerializer.Deserialize<GCStructType>(ref reader, options);
				case _vertexDataType:
					return JsonSerializer.Deserialize<GCDataType>(ref reader, options);
				case _attributes:
					return reader.GetString()!.HexToByte("Vertex format attributes");
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override GCVertexFormatParameter CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				VertexType = (GCVertexType)values[_vertexType]!,
				VertexStructType = (GCStructType)values[_vertexStructType]!,
				VertexDataType = (GCDataType)values[_vertexDataType]!,
				Attributes = (byte)values[_attributes]!,

			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, GCVertexFormatParameter value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_vertexType);
			JsonSerializer.Serialize(writer, value.VertexType, options);

			writer.WritePropertyName(_vertexStructType);
			JsonSerializer.Serialize(writer, value.VertexStructType, options);

			writer.WritePropertyName(_vertexDataType);
			JsonSerializer.Serialize(writer, value.VertexDataType, options);

			if(value.Attributes != 0)
			{
				writer.WriteString(_attributes, value.Attributes.ToString("X", CultureInfo.InvariantCulture));
			}
		}
	}
}
