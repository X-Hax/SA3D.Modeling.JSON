using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Gamecube;
using SA3D.Modeling.Mesh.Gamecube.Enums;
using SA3D.Modeling.Structs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Numerics;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Gamecube
{
	/// <summary>
	/// Json converter for <see cref="GCVertexSet"/>
	/// </summary>
	public class GCVertexSetJsonConverter : SimpleJsonObjectConverter<GCVertexSet>
	{
		private const string _data = "Data";
		private const string _type = nameof(GCVertexSet.Type);
		private const string _dataType = nameof(GCVertexSet.DataType);
		private const string _structType = nameof(GCVertexSet.StructType);

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _type, new(PropertyTokenType.String, null) },
			{ _dataType, new(PropertyTokenType.String, null) },
			{ _structType, new(PropertyTokenType.String, null) },
			{ _data, new(PropertyTokenType.Array, null) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _type:
					return JsonSerializer.Deserialize<GCVertexType>(ref reader, options);
				case _dataType:
					return JsonSerializer.Deserialize<GCDataType>(ref reader, options);
				case _structType:
					return JsonSerializer.Deserialize<GCStructType>(ref reader, options);
				case _data:
					GCStructType structType = (GCStructType?)values[_structType]
						?? throw new InvalidDataException($"{_structType} property of GCVertexSet has to be specified before {_data} property!");

					switch(structType)
					{
						case GCStructType.PositionXY:
						case GCStructType.PositionXYZ:
						case GCStructType.NormalXYZ:
							return JsonSerializer.Deserialize<Vector3[]>(ref reader, options);
						case GCStructType.ColorRGB:
						case GCStructType.ColorRGBA:
							return JsonSerializer.Deserialize<Color[]>(ref reader, options);
						case GCStructType.TexCoordU:
						case GCStructType.TexCoordUV:
							return JsonSerializer.Deserialize<Vector2[]>(ref reader, options);
						case GCStructType.NormalNBT:
						case GCStructType.NormalNBT3:
						default:
							throw new NotSupportedException($"GC VertexSet struct type of \"{structType}\" is not supported.");
					}
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override GCVertexSet Create(ReadOnlyDictionary<string, object?> values)
		{
			GCVertexType type = (GCVertexType?)values[_type]
				?? throw new InvalidDataException($"GCVertexSet requires \"{_type}\" property!");

			GCDataType dataType = (GCDataType?)values[_dataType]
				?? throw new InvalidDataException($"GCVertexSet requires \"{_dataType}\" property!");

			GCStructType structType = (GCStructType?)values[_structType]
				?? throw new InvalidDataException($"GCVertexSet requires \"{_structType}\" property!");

			Array data = (Array?)values[_data]
				?? throw new InvalidDataException($"GCVertexSet requires \"{_data}\" property!");

			return new(type, dataType, structType, data);
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, GCVertexSet value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_type);
			JsonSerializer.Serialize(writer, value.Type, options);

			writer.WritePropertyName(_dataType);
			JsonSerializer.Serialize(writer, value.DataType, options);

			writer.WritePropertyName(_structType);
			JsonSerializer.Serialize(writer, value.StructType, options);

			writer.WritePropertyName(_data);
			JsonSerializer.Serialize(writer, value.Data, options);
		}
	}
}
