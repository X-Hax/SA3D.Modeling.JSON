using SA3D.Common.Lookup;
using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Basic;
using SA3D.Modeling.Mesh.Basic.Polygon;
using SA3D.Modeling.Structs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Basic
{
	/// <summary>
	/// Json converter for <see cref="BasicMesh"/>
	/// </summary>
	public class BasicMeshJsonConverter : SimpleJsonObjectConverter<BasicMesh>
	{
		private const string _materialIndex = nameof(BasicMesh.MaterialIndex);
		private const string _polygonType = nameof(BasicMesh.PolygonType);
		private const string _polygons = nameof(BasicMesh.Polygons);
		private const string _polyAttributes = nameof(BasicMesh.PolyAttributes);
		private const string _normals = nameof(BasicMesh.Normals);
		private const string _colors = nameof(BasicMesh.Colors);
		private const string _texcoords = nameof(BasicMesh.Texcoords);


		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _materialIndex, new(PropertyTokenType.Number, 0u) },
			{ _polygonType, new(PropertyTokenType.String, null) },
			{ _polygons, new(PropertyTokenType.Object | PropertyTokenType.String, null) },
			{ _polyAttributes, new(PropertyTokenType.String, 0u) },
			{ _normals, new(PropertyTokenType.Object | PropertyTokenType.String, null, true) },
			{ _colors, new(PropertyTokenType.Object | PropertyTokenType.String, null, true) },
			{ _texcoords, new(PropertyTokenType.Object | PropertyTokenType.String, null, true) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _materialIndex:
					return reader.GetUInt16();
				case _polygonType:
					return JsonSerializer.Deserialize<BasicPolygonType>(ref reader, options);
				case _polygons:
					BasicPolygonType type = (BasicPolygonType?)values[_polygonType]
						?? throw new InvalidDataException($"Basic meshes require property \"{_polygonType}\" before the \"{_polygons}\" array!");

					string label;
					IBasicPolygon[] polygons;

					switch(type)
					{
						case BasicPolygonType.Triangles:
							LabeledReadOnlyArray<BasicTriangle> triangles = JsonSerializer.Deserialize<LabeledReadOnlyArray<BasicTriangle>>(ref reader, options)!;
							label = triangles.Label;
							polygons = triangles.Array.Cast<IBasicPolygon>().ToArray();

							break;
						case BasicPolygonType.Quads:
							LabeledReadOnlyArray<BasicQuad> quads = JsonSerializer.Deserialize<LabeledReadOnlyArray<BasicQuad>>(ref reader, options)!;
							label = quads.Label;
							polygons = quads.Array.Cast<IBasicPolygon>().ToArray();

							break;
						case BasicPolygonType.NPoly:
						case BasicPolygonType.TriangleStrips:
							LabeledReadOnlyArray<BasicMultiPolygon> multiPolygons = JsonSerializer.Deserialize<LabeledReadOnlyArray<BasicMultiPolygon>>(ref reader, options)!;
							label = multiPolygons.Label;
							polygons = multiPolygons.Array.Cast<IBasicPolygon>().ToArray();

							break;
						default:
							throw new InvalidOperationException("Cannot be reached; If reached, basic polygon type somehow invalid.");
					}

					return new LabeledReadOnlyArray<IBasicPolygon>(label, polygons);
				case _polyAttributes:
					return reader.GetString()!.HexToUInt("Polygon attributes");
				case _normals:
					return JsonSerializer.Deserialize<LabeledArray<Vector3>>(ref reader, options);
				case _colors:
					return JsonSerializer.Deserialize<LabeledArray<Color>>(ref reader, options);
				case _texcoords:
					return JsonSerializer.Deserialize<LabeledArray<Vector2>>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override BasicMesh Create(ReadOnlyDictionary<string, object?> values)
		{
			return new(
				(ushort)values[_materialIndex]!,
				(BasicPolygonType)values[_polygonType]!,
				(LabeledReadOnlyArray<IBasicPolygon>)values[_polygons]!,
				(LabeledArray<Vector3>?)values[_normals],
				(LabeledArray<Color>?)values[_colors],
				(LabeledArray<Vector2>?)values[_texcoords])
			{
				PolyAttributes = (uint)values[_polyAttributes]!
			};
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, BasicMesh value, JsonSerializerOptions options)
		{
			writer.WriteNumber(_materialIndex, value.MaterialIndex);

			writer.WritePropertyName(_polygonType);
			JsonSerializer.Serialize(writer, value.PolygonType, options);

			writer.WritePropertyName(_polygons);
			JsonSerializer.Serialize(writer, value.Polygons, options);

			if(value.PolyAttributes != 0)
			{
				writer.WriteString(_polyAttributes, value.PolyAttributes.ToString("X", NumberFormatInfo.InvariantInfo));
			}

			if(value.Normals != null)
			{
				writer.WritePropertyName(_normals);
				JsonSerializer.Serialize(writer, value.Normals, options);
			}

			if(value.Colors != null)
			{
				writer.WritePropertyName(_colors);
				JsonSerializer.Serialize(writer, value.Colors, options);
			}

			if(value.Texcoords != null)
			{
				writer.WritePropertyName(_texcoords);
				JsonSerializer.Serialize(writer, value.Texcoords, options);
			}
		}
	}
}
