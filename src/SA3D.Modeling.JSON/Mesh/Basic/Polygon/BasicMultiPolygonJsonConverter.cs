using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Basic.Polygon;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Basic.Polygon
{
	/// <summary>
	/// Json converter for <see cref="BasicMultiPolygon"/>
	/// </summary>
	public class BasicMultiPolygonJsonConverter : SimpleJsonObjectConverter<BasicMultiPolygon>
	{
		private const string _reversed = nameof(BasicMultiPolygon.Reversed);
		private const string _indices = nameof(BasicMultiPolygon.Indices);

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _reversed, new(PropertyTokenType.Bool, false) },
			{ _indices, new(PropertyTokenType.Array, null) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			return propertyName switch
			{
				_reversed => reader.GetBoolean(),
				_indices => JsonSerializer.Deserialize<ushort[]>(ref reader, options),
				_ => throw new InvalidPropertyException(),
			};
		}

		/// <inheritdoc/>
		protected override BasicMultiPolygon Create(ReadOnlyDictionary<string, object?> values)
		{
			bool reversed = (bool)values[_reversed]!;

			ushort[] indices = (ushort[]?)values[_indices] 
				?? throw new InvalidDataException("Multipolygon requires indices!");

			return new(indices, reversed);
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, BasicMultiPolygon value, JsonSerializerOptions options)
		{
			writer.WriteBoolean(_reversed, value.Reversed);
			writer.WritePropertyName(_indices);

			JsonSerializer.Serialize(writer, value.Indices, options);
		}
	}
}
