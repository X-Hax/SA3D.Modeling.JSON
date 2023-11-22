using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Gamecube;
using SA3D.Modeling.Mesh.Gamecube.Parameters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Gamecube
{
	/// <summary>
	/// Json converter for <see cref="GCMesh"/>
	/// </summary>
	public class GCMeshJsonConverter : SimpleJsonObjectConverter<GCMesh>
	{
		private const string _parameters = nameof(GCMesh.Parameters);
		private const string _polygons = nameof(GCMesh.Polygons);

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _parameters, new(PropertyTokenType.Array, null) },
			{ _polygons, new(PropertyTokenType.Array, null) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _parameters:
					return JsonSerializer.Deserialize<IGCParameter[]>(ref reader, options);
				case _polygons:
					return JsonSerializer.Deserialize<GCPolygon[]>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override GCMesh Create(ReadOnlyDictionary<string, object?> values)
		{
			IGCParameter[] parameters = (IGCParameter[]?)values[_parameters]
				?? throw new InvalidDataException($"GCMesh requires property \"{_parameters}\"!");

			GCPolygon[] polygons = (GCPolygon[]?)values[_polygons]
				?? throw new InvalidDataException($"GCMesh requires property \"{_polygons}\"!");

			return new(parameters, polygons);
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, GCMesh value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_parameters);
			JsonSerializer.Serialize(writer, value.Parameters, options);

			writer.WritePropertyName(_polygons);
			JsonSerializer.Serialize(writer, value.Polygons, options);
		}
	}
}
