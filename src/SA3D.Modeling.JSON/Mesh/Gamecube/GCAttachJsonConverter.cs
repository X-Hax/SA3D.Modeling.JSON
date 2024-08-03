using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh;
using SA3D.Modeling.Mesh.Buffer;
using SA3D.Modeling.Mesh.Gamecube;
using SA3D.Modeling.Mesh.Gamecube.Enums;
using SA3D.Modeling.Structs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Gamecube
{
	/// <summary>
	/// Json converter for <see cref="GCAttach"/>
	/// </summary>
	public class GCAttachJsonConverter : ChildJsonObjectConverter<AttachFormat, GCAttach, Attach>
	{
		private const string _vertexData = nameof(GCAttach.VertexData);
		private const string _opaqueMeshes = nameof(GCAttach.OpaqueMeshes);
		private const string _transparentMeshes = nameof(GCAttach.TransparentMeshes);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<AttachFormat, Attach> ParentConverter => AttachJsonConverter._globalAttachConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _vertexData, new(PropertyTokenType.Array, null) },
			{ _opaqueMeshes, new(PropertyTokenType.Array, null) },
			{ _transparentMeshes, new(PropertyTokenType.Array, null) },
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(AttachFormat key)
		{
			return key == AttachFormat.GC;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _vertexData:
					return JsonSerializer.Deserialize<GCVertexSet[]>(ref reader, options);
				case _opaqueMeshes:
				case _transparentMeshes:
					return JsonSerializer.Deserialize<GCMesh[]>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override GCAttach CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			GCVertexSet[] vertexData = (GCVertexSet[]?)values[_vertexData]
				?? throw new InvalidDataException("GCAttahc requires Vertexdata!");

			Dictionary<GCVertexType, GCVertexSet> vertexDict = [];

			foreach(GCVertexSet set in vertexData)
			{
				if(!vertexDict.TryAdd(set.Type, set))
				{
					throw new InvalidDataException($"GCAttach has multiple vertex sets of the type \"{set.Type}\"!");
				}
			}

			GCAttach result = new(
				vertexDict,
				(GCMesh[]?)values[_opaqueMeshes] ?? Array.Empty<GCMesh>(),
				(GCMesh[]?)values[_transparentMeshes] ?? Array.Empty<GCMesh>()
			) {
				Label = (string)values[AttachJsonConverter._label]!,
				MeshBounds = (Bounds)values[AttachJsonConverter._meshBounds]!
			};

			if(values[AttachJsonConverter._meshData] is BufferMesh[] meshData)
			{
				result.MeshData = meshData;
			}

			return result;
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, GCAttach value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_vertexData);
			GCVertexSet[] vertexData = value.VertexData.Values.ToArray();
			JsonSerializer.Serialize(writer, vertexData, options);

			if(value.OpaqueMeshes.Length > 0)
			{
				writer.WritePropertyName(_opaqueMeshes);
				JsonSerializer.Serialize(writer, value.OpaqueMeshes, options);
			}

			if(value.TransparentMeshes.Length > 0)
			{
				writer.WritePropertyName(_transparentMeshes);
				JsonSerializer.Serialize(writer, value.TransparentMeshes, options);
			}
		}
	}
}
