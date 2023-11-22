using SA3D.Common.Lookup;
using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh;
using SA3D.Modeling.Mesh.Basic;
using SA3D.Modeling.Mesh.Buffer;
using SA3D.Modeling.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Numerics;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Basic
{
	/// <summary>
	/// Json converter for <see cref="BasicAttach"/>
	/// </summary>
	public class BasicAttachJsonConverter : ChildJsonObjectConverter<AttachFormat, BasicAttach, Attach>
	{
		private const string _positions = nameof(BasicAttach.Positions);
		private const string _normals = nameof(BasicAttach.Normals);
		private const string _meshes = nameof(BasicAttach.Meshes);
		private const string _materials = nameof(BasicAttach.Materials);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<AttachFormat, Attach> ParentConverter => AttachJsonConverter._globalAttachConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _positions, new (PropertyTokenType.Object | PropertyTokenType.String, null) },
			{ _normals, new (PropertyTokenType.Object | PropertyTokenType.String, null) },
			{ _meshes, new(PropertyTokenType.Object | PropertyTokenType.String, null) },
			{ _materials, new(PropertyTokenType.Object | PropertyTokenType.String, null) },
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(AttachFormat key)
		{
			return key == AttachFormat.BASIC;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _positions:
				case _normals:
					return JsonSerializer.Deserialize<LabeledArray<Vector3>>(ref reader, options);
				case _meshes:
					return JsonSerializer.Deserialize<LabeledArray<BasicMesh>>(ref reader, options);
				case _materials:
					return JsonSerializer.Deserialize<LabeledArray<BasicMaterial>>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override BasicAttach CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			LabeledArray<Vector3> positions = (LabeledArray<Vector3>?)values[_positions]
				?? throw new InvalidDataException($"Basic attach requires property \"{_positions}\"!");

			LabeledArray<Vector3> normals = (LabeledArray<Vector3>?)values[_normals]
				?? throw new InvalidDataException($"Basic attach requires property \"{_normals}\"!");

			LabeledArray<BasicMesh> meshes = (LabeledArray<BasicMesh>?)values[_meshes]
				?? throw new InvalidDataException($"Basic attach requires property \"{_meshes}\"!");

			LabeledArray<BasicMaterial> materials = (LabeledArray<BasicMaterial>?)values[_materials]
				?? throw new InvalidDataException($"Basic attach requires property \"{_materials}\"!");

			BasicAttach result = new(positions, normals, meshes, materials)
			{
				Label = (string)values[AttachJsonConverter._label]!,
				MeshBounds = (Bounds)values[AttachJsonConverter._meshBounds]!,
			};

			if(values[AttachJsonConverter._meshData] is BufferMesh[] meshData)
			{
				result.MeshData = meshData;
			}

			return result;
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, BasicAttach value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_positions);
			JsonSerializer.Serialize(writer, value.Positions, options);

			writer.WritePropertyName(_normals);
			JsonSerializer.Serialize(writer, value.Normals, options);

			writer.WritePropertyName(_meshes);
			JsonSerializer.Serialize(writer, value.Meshes, options);

			writer.WritePropertyName(_materials);
			JsonSerializer.Serialize(writer, value.Materials, options);
		}
	}
}
