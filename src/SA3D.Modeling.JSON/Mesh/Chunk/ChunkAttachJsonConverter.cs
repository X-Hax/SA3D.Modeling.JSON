using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using SA3D.Modeling.Mesh.Chunk;
using SA3D.Common.Lookup;
using SA3D.Modeling.Structs;
using SA3D.Modeling.Mesh.Buffer;

namespace SA3D.Modeling.JSON.Mesh.Chunk
{
	/// <summary>
	/// Json converter for <see cref="ChunkAttach"/>
	/// </summary>
	public class ChunkAttachJsonConverter : ChildJsonObjectConverter<AttachFormat, ChunkAttach, Attach>
	{
		private const string _vertexChunks = nameof(ChunkAttach.VertexChunks);
		private const string _polyChunks = nameof(ChunkAttach.PolyChunks);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<AttachFormat, Attach> ParentConverter => AttachJsonConverter._globalAttachConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _vertexChunks, new(PropertyTokenType.Object | PropertyTokenType.String, null, true) },
			{ _polyChunks, new(PropertyTokenType.Object | PropertyTokenType.String, null, true) },
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(AttachFormat key)
		{
			return key == AttachFormat.CHUNK;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _vertexChunks:
					return JsonSerializer.Deserialize<LabeledArray<VertexChunk?>?>(ref reader, options);
				case _polyChunks:
					return JsonSerializer.Deserialize<LabeledArray<PolyChunk?>?>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override ChunkAttach CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			ChunkAttach result = new(
				(LabeledArray<VertexChunk?>?)values[_vertexChunks],
				(LabeledArray<PolyChunk?>?)values[_polyChunks]
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
		protected override void WriteTargetValues(Utf8JsonWriter writer, ChunkAttach value, JsonSerializerOptions options)
		{
			if(_vertexChunks != null)
			{
				writer.WritePropertyName(_vertexChunks);
				JsonSerializer.Serialize(writer, value.VertexChunks, options);
			}

			if(_polyChunks != null)
			{
				writer.WritePropertyName(_polyChunks);
				JsonSerializer.Serialize(writer, value.PolyChunks, options);
			}
		}
	}
}
