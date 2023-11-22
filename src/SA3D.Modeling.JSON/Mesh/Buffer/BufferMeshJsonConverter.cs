using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Buffer;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Buffer
{
	/// <summary>
	/// Json converter for <see cref="BufferMesh"/>
	/// </summary>
	public class BufferMeshJsonConverter : SimpleJsonObjectConverter<BufferMesh>
	{
		private const string _vertices = nameof(BufferMesh.Vertices);
		private const string _material = nameof(BufferMesh.Material);
		private const string _corners = nameof(BufferMesh.Corners);
		private const string _indexList = nameof(BufferMesh.IndexList);
		private const string _strippified = nameof(BufferMesh.Strippified);
		private const string _continueWeight = nameof(BufferMesh.ContinueWeight);
		private const string _hasNormals = nameof(BufferMesh.HasNormals);
		private const string _hasColors = nameof(BufferMesh.HasColors);
		private const string _vertexWriteOffset = nameof(BufferMesh.VertexWriteOffset);
		private const string _vertexReadOffset = nameof(BufferMesh.VertexReadOffset);


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _vertices, new(PropertyTokenType.Array, null, true) },
			{ _material, new(PropertyTokenType.Object, BufferMaterial.DefaultValues) },
			{ _corners, new(PropertyTokenType.Array, null, true) },
			{ _indexList, new(PropertyTokenType.Array, null, true) },
			{ _strippified, new(PropertyTokenType.Bool, false) },
			{ _continueWeight, new(PropertyTokenType.Bool, false) },
			{ _hasNormals, new(PropertyTokenType.Bool, false) },
			{ _hasColors, new(PropertyTokenType.Bool, false) },
			{ _vertexWriteOffset, new(PropertyTokenType.Number, (ushort)0u) },
			{ _vertexReadOffset, new(PropertyTokenType.Number, (ushort)0u) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			return propertyName switch
			{
				_vertices => JsonSerializer.Deserialize<BufferVertex[]?>(ref reader, options),
				_material => JsonSerializer.Deserialize<BufferMaterial>(ref reader, options),
				_corners => JsonSerializer.Deserialize<BufferCorner[]?>(ref reader, options),
				_indexList => JsonSerializer.Deserialize<uint[]?>(ref reader, options),

				_strippified
				or _continueWeight
				or _hasNormals
				or _hasColors => reader.GetBoolean(),

				_vertexWriteOffset
				or _vertexReadOffset => reader.GetUInt16(),

				_ => throw new InvalidPropertyException(),
			};
		}

		/// <inheritdoc/>
		protected override BufferMesh Create(ReadOnlyDictionary<string, object?> values)
		{
			BufferVertex[]? vertices = (BufferVertex[]?)values[_vertices];
			BufferMaterial material = (BufferMaterial)values[_material]!;
			BufferCorner[]? corners = (BufferCorner[]?)values[_corners];
			uint[]? indexList = (uint[]?)values[_indexList];
			bool strippified = (bool)values[_strippified]!;
			bool continueWeight = (bool)values[_continueWeight]!;
			bool hasNormals = (bool)values[_hasNormals]!;
			bool hasColors = (bool)values[_hasColors]!;
			ushort vertexWriteOffset = (ushort)values[_vertexWriteOffset]!;
			ushort vertexReadOffset = (ushort)values[_vertexReadOffset]!;

			if(vertices == null && corners == null)
			{
				throw new InvalidDataException("Buffer mesh improperly formatted: Requires at least vertices or corners.");
			}
			else if(vertices == null)
			{
				return new BufferMesh(material, corners!, indexList, strippified, hasColors, vertexReadOffset);
			}
			else if(corners == null)
			{
				return new BufferMesh(vertices, continueWeight, hasNormals, vertexWriteOffset);
			}
			else
			{
				return new BufferMesh(vertices, material, corners, indexList, strippified, continueWeight, hasNormals, hasColors, vertexWriteOffset, vertexReadOffset);
			}
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, BufferMesh value, JsonSerializerOptions options)
		{
			if(value.Vertices != null)
			{
				writer.WritePropertyName(_vertices);
				JsonSerializer.Serialize(writer, value.Vertices, options);
			}

			if(value.Corners != null)
			{
				if(value.Material != BufferMaterial.DefaultValues)
				{
					writer.WritePropertyName(_material);
					JsonSerializer.Serialize(writer, value.Material, options);
				}

				writer.WritePropertyName(_corners);
				JsonSerializer.Serialize(writer, value.Corners, options);

				if(value.IndexList != null)
				{
					writer.WritePropertyName(_indexList);
					JsonSerializer.Serialize(writer, value.IndexList, options);
				}

				if(value.Strippified)
				{
					writer.WriteBoolean(_strippified, value.Strippified);
				}
			}

			if(value.Vertices != null)
			{
				if(value.ContinueWeight)
				{
					writer.WriteBoolean(_continueWeight, value.ContinueWeight);
				}

				if(value.HasNormals)
				{
					writer.WriteBoolean(_hasNormals, value.HasNormals);
				}
			}

			if(value.Corners != null && value.HasColors)
			{
				writer.WriteBoolean(_hasColors, value.HasColors);
			}

			if(value.Vertices != null && value.VertexWriteOffset != 0)
			{
				writer.WriteNumber(_vertexWriteOffset, value.VertexWriteOffset);
			}

			if(value.Corners != null && value.VertexReadOffset != 0)
			{
				writer.WriteNumber(_vertexReadOffset, value.VertexReadOffset);
			}
		}
	}
}
