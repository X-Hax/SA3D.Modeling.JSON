using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.JSON.Mesh.Basic;
using SA3D.Modeling.JSON.Mesh.Chunk;
using SA3D.Modeling.JSON.Mesh.Gamecube;
using SA3D.Modeling.Mesh;
using SA3D.Modeling.Mesh.Buffer;
using SA3D.Modeling.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh
{
	/// <summary>
	/// Json converter for <see cref="Attach"/>
	/// </summary>
	public class AttachJsonConverter : ParentJsonObjectConverter<AttachFormat, Attach>
	{
		internal static readonly AttachJsonConverter _globalAttachConverter = new();

		internal const string _format = nameof(Attach.Format);
		internal const string _label = nameof(Attach.Label);
		internal const string _meshBounds = nameof(Attach.MeshBounds);
		internal const string _meshData = nameof(Attach.MeshData);

		/// <inheritdoc/>
		protected internal override string KeyPropertyName => _format;

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _format, new(PropertyTokenType.String, null) },
			{ _label, new(PropertyTokenType.String, string.Empty) },
			{ _meshBounds, new(PropertyTokenType.Object, default(Bounds)) },
			{ _meshData, new(PropertyTokenType.Array, null) }
		});


		/// <inheritdoc/>
		protected internal override object? ReadBaseValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _format:
					return JsonSerializer.Deserialize<AttachFormat>(ref reader, options);
				case _label:
					return reader.GetString();
				case _meshBounds:
					return JsonSerializer.Deserialize<Bounds>(ref reader, options);
				case _meshData:
					return JsonSerializer.Deserialize<BufferMesh[]>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override Attach CreateBase(ReadOnlyDictionary<string, object?> values)
		{
			if(values[_meshData] is not BufferMesh[] meshData)
			{
				throw new InvalidDataException("No mesh data in attach!");
			}

			return new Attach(meshData)
			{
				Label = (string)values[_label]!,
				MeshBounds = (Bounds)values[_meshBounds]!,
			};
		}

		/// <inheritdoc/>
		protected internal override void WriteBaseValues(Utf8JsonWriter writer, Attach value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_format);
			JsonSerializer.Serialize(writer, value.Format, options);

			writer.WriteString(_label, value.Label);

			if(value.MeshBounds != default)
			{
				writer.WritePropertyName(_meshBounds);
				JsonSerializer.Serialize(writer, value.MeshBounds, options);
			}

			if(value.MeshData.Length > 0 && !value.GetType().IsSubclassOf(typeof(Attach)))
			{
				writer.WritePropertyName(_meshData);
				JsonSerializer.Serialize(writer, value.MeshData, options);
			}
		}

		/// <inheritdoc/>
		protected override AttachFormat GetKeyFromValue(Attach value)
		{
			return value.Format;
		}

		/// <inheritdoc/>
		protected override Dictionary<AttachFormat, IChildJsonConverter<Attach>> CreateConverters()
		{
			return new()
			{
				{ AttachFormat.BASIC, new BasicAttachJsonConverter() },
				{ AttachFormat.GC, new GCAttachJsonConverter() },
				{ AttachFormat.CHUNK, new ChunkAttachJsonConverter() },
			};
		}
	}
}
