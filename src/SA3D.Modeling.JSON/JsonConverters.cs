using SA3D.Modeling.JSON.Mesh;
using SA3D.Modeling.JSON.Mesh.Basic;
using SA3D.Modeling.JSON.Mesh.Basic.Polygon;
using SA3D.Modeling.JSON.Mesh.Buffer;
using SA3D.Modeling.JSON.Mesh.Gamecube.Parameters;
using SA3D.Modeling.JSON.Mesh.Gamecube;
using SA3D.Modeling.JSON.Mesh.Weighted;
using SA3D.Modeling.JSON.ObjectData;
using SA3D.Modeling.JSON.Structs;
using System.Text.Json;
using System.Text.Json.Serialization;
using SA3D.Modeling.JSON.Mesh.Chunk.PolyChunks;
using SA3D.Modeling.JSON.Mesh.Chunk.Structs;
using SA3D.Modeling.JSON.Mesh.Chunk;
using SA3D.Modeling.JSON.File;
using SA3D.Modeling.JSON.Animation;

namespace SA3D.Modeling.JSON
{
	/// <summary>
	/// Json converter manager.
	/// </summary>
	public static class JsonConverters
	{
		/// <summary>
		/// Adds all converters to existing options.
		/// </summary>
		/// <param name="options">The options to add to.</param>
		public static void AddToOptions(JsonSerializerOptions options)
		{
            JsonConverter[] converters =
            [
				// General
				new JsonStringEnumConverter(),

				// File
				new MetaDataJsonConverter(),
                new ModelFileJsonConverter(),
                new LevelFileJsonConverter(),
                new AnimationFileJsonConverter(),
                new TextureNameJsonConverter(),
                new TextureNameListJsonConverter(),

				// ObjectData
				new NodeJsonConverter(),
                new LandEntryJsonConverter(),
                new LandTableJsonConverter(),

				// Animation
				new KeyframesJsonConverter(),
                new LandEntryMotionJsonConverter(),
                new MotionJsonConverter(),
                new NodeMotionJsonConverter(),
                new SpotlightJsonConverter(),

				// Mesh
				new AttachJsonConverter(),

				// Mesh.Buffer
				new BufferMeshJsonConverter(),
                new BufferMaterialJsonConverter(),
                new BufferCornerJsonConverter(),
                new BufferVertexJsonConverter(),

				// Mesh.Weighted
				new WeightedMeshJsonConverter(),
                new WeightedVertexJsonConverter(),

				// Mesh.Basic
				new BasicAttachJsonConverter(),
                new BasicMaterialJsonConverter(),
                new BasicMeshJsonConverter(),
                new BasicTriangleJsonConverter(),
                new BasicQuadJsonConverter(),
                new BasicMultiPolygonJsonConverter(),
                new BasicPolygonJsonConverter(),

				// Mesh.Chunk
				new ChunkAttachJsonConverter(),
                new VertexChunkJsonConverter(),
                new PolyChunkJsonConverter(),

				// Mesh.Chunk.Structs
				new ChunkCornerJsonConverter(),
                new ChunkStripJsonConverter(),
                new ChunkVertexJsonConverter(),
                new ChunkVolumePolygonJsonConverter(),
                new ChunkVolumeQuadJsonConverter(),
                new ChunkVolumeStripJsonConverter(),
                new ChunkVolumeTriangleJsonConverter(),

				// Mesh.Chunk.PolyChunks
				new BlendAlphaChunkJsonConverter(),
                new CacheListChunkJsonConverter(),
                new DrawListChunkJsonConverter(),
                new SpecularExponentChunkJsonConverter(),
                new MipmapDistanceMultiplierChunkJsonConverter(),
                new MaterialBumpChunkJsonConverter(),
                new TextureChunkJsonConverter(),
                new MaterialChunkJsonConverter(),
                new StripChunkJsonConverter(),
                new VolumeChunkJsonConverter(),

				// Mesh.Gamecube
				new GCAttachJsonConverter(),
                new GCVertexSetJsonConverter(),
                new GCMeshJsonConverter(),
                new GCPolygonJsonConverter(),
                new GCCornerJsonConverter(),

				// Mesh.Gamecube.Parameters
				new GCParameterJsonConverter(),
                new GCAmbientColorParameterJsonConverter(),
                new GCDiffuseColorParameterJsonConverter(),
                new GCSpecularColorParameterJsonConverter(),
                new GCBlendAlphaParameterJsonConverter(),
                new GCIndexFormatParameterJsonConverter(),
                new GCLightingParameterJsonConverter(),
                new GCTexCoordParameterJsonConverter(),
                new GCTextureParameterJsonConverter(),
                new GCUnknownParameterJsonConverter(),
                new GCVertexFormatParameterJsonConverter(),

				// Structs
				new ColorJsonConverter(),
                new Vector2JsonConverter(),
                new Vector3JsonConverter(),
                new Vector4JsonConverter(),
                new QuaternionJsonConverter(),
                new Matrix4x4JsonConverter(),
                new BoundsJsonConverter(),

                new ILabeledArrayJsonConverterFactory(),
                new LabeledArrayJsonConverterFactory(),
                new LabeledReadOnlyArrayJsonConverterFactory()
            ];

			foreach(JsonConverter converter in converters)
			{
				options.Converters.Add(converter);
			}
		}

		/// <summary>
		/// Returns new Json serializer options with all converters added.
		/// </summary>
		/// <returns></returns>
		public static JsonSerializerOptions GetOptions()
		{
			JsonSerializerOptions result = new();
			AddToOptions(result);

			return result;
		}
	}
}
