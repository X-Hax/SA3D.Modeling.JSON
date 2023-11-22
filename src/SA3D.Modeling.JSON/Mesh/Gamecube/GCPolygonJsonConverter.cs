using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Gamecube;
using SA3D.Modeling.Mesh.Gamecube.Enums;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Gamecube
{
	/// <summary>
	/// Json converter for <see cref="GCPolygon"/>
	/// </summary>
	public class GCPolygonJsonConverter : SimpleJsonObjectConverter<GCPolygon>
	{
		private const string _type = nameof(GCPolygon.Type);
		private const string _corners = nameof(GCPolygon.Corners);

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _type, new(PropertyTokenType.String, null) },
			{ _corners, new(PropertyTokenType.Array, null) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _type:
					return JsonSerializer.Deserialize<GCPolyType>(ref reader, options);
				case _corners:
					return JsonSerializer.Deserialize<GCCorner[]>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override GCPolygon Create(ReadOnlyDictionary<string, object?> values)
		{
			GCPolyType type = (GCPolyType?)values[_type] 
				?? throw new InvalidDataException($"GCPolygon requires property \"{_type}\"!");

			GCCorner[] corners = (GCCorner[]?)values[_corners] 
				?? throw new InvalidDataException($"GCPolygon requires property \"{_corners}\"!");

			return new(type, corners);
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, GCPolygon value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_type);
			JsonSerializer.Serialize(writer, value.Type, options);

			writer.WritePropertyName(_corners);
			JsonSerializer.Serialize(writer, value.Corners, options);
		}
	}
}
