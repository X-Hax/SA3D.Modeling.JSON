using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Buffer;
using SA3D.Modeling.Mesh.Weighted;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Weighted
{
	/// <summary>
	/// Json converter for <see cref="WeightedMesh"/>
	/// </summary>
	public class WeightedMeshJsonConverter : SimpleJsonObjectConverter<WeightedMesh>
	{
		private const string _label = nameof(WeightedMesh.Label);
		private const string _vertices = nameof(WeightedMesh.Vertices);
		private const string _triangleSets = nameof(WeightedMesh.TriangleSets);
		private const string _materials = nameof(WeightedMesh.Materials);
		private const string _rootIndices = nameof(WeightedMesh.RootIndices);
		private const string _hasColors = nameof(WeightedMesh.HasColors);
        private const string _hasNormals = nameof(WeightedMesh.HasNormals);
        private const string _forceVertexColors = nameof(WeightedMesh.ForceVertexColors);
		private const string _writeSpecular = nameof(WeightedMesh.WriteSpecular);


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _label, new(PropertyTokenType.String, null, true) },
			{ _vertices, new(PropertyTokenType.Array, null) },
			{ _triangleSets, new(PropertyTokenType.Array, null) },
			{ _materials, new(PropertyTokenType.Array, null) },
			{ _rootIndices, new(PropertyTokenType.Array, null) },
			{ _hasColors, new(PropertyTokenType.Bool, false) },
            { _hasNormals, new(PropertyTokenType.Bool, false) },
            { _forceVertexColors, new(PropertyTokenType.Bool, false) },
			{ _writeSpecular, new(PropertyTokenType.Bool, false) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _label:
					return reader.GetString();
				case _vertices:
					return JsonSerializer.Deserialize<WeightedVertex[]>(ref reader, options);
				case _triangleSets:
					return JsonSerializer.Deserialize<BufferCorner[][]>(ref reader, options);
				case _materials:
					return JsonSerializer.Deserialize<BufferMaterial[]>(ref reader, options);
				case _rootIndices:
					return JsonSerializer.Deserialize<int[]>(ref reader, options);
				case _hasColors:
                case _hasNormals:
				case _forceVertexColors:
				case _writeSpecular:
					return reader.GetBoolean();
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override WeightedMesh Create(ReadOnlyDictionary<string, object?> values)
		{
			WeightedVertex[] vertices = (WeightedVertex[]?)values[_vertices] 
				?? throw new InvalidDataException("Weighted meshes require vertices!");

			BufferCorner[][] triangleSets = (BufferCorner[][]?)values[_triangleSets]
				?? throw new InvalidDataException("Weighted meshes require vertices!");

			BufferMaterial[] materials = (BufferMaterial[]?)values[_materials]
				?? throw new InvalidDataException("Weighted meshes require vertices!");

			WeightedMesh result = WeightedMesh.Create(
				vertices,
				triangleSets,
				materials,
				(bool)values[_hasColors]!,
                (bool)values[_hasNormals]!);

			result.ForceVertexColors = (bool)values[_forceVertexColors]!;
			result.WriteSpecular = (bool)values[_writeSpecular]!;

			if(values[_rootIndices] is int[] rootIndices)
			{
				result.RootIndices.UnionWith(rootIndices);
			}

			return result;
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, WeightedMesh value, JsonSerializerOptions options)
		{
			if(value.Label != null)
			{
				writer.WriteString(_label, value.Label);
			}

			writer.WritePropertyName(_vertices);
			JsonSerializer.Serialize(writer, value.Vertices, options);

			writer.WritePropertyName(_triangleSets);
			JsonSerializer.Serialize(writer, value.TriangleSets, options);

			writer.WritePropertyName(_materials);
			JsonSerializer.Serialize(writer, value.Materials, options);

			if(value.RootIndices.Count > 0)
			{
				writer.WritePropertyName(_rootIndices);
				JsonSerializer.Serialize(writer, value.RootIndices, options);
			}

			if(value.HasColors)
			{
				writer.WriteBoolean(_hasColors, value.HasColors);
			}

            if(value.HasNormals)
            {
                writer.WriteBoolean(_hasNormals, value.HasNormals);
            }

            if(value.ForceVertexColors)
			{
				writer.WriteBoolean(_forceVertexColors, value.ForceVertexColors);
			}

			if(value.WriteSpecular)
			{
				writer.WriteBoolean(_writeSpecular, value.WriteSpecular);
			}
		}
	}
}
