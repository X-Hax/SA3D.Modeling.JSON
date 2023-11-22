using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Chunk.Structs;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Chunk.Structs
{
	/// <summary>
	/// Json converter for <see cref="ChunkStrip"/>
	/// </summary>
	public class ChunkStripJsonConverter : SimpleJsonObjectConverter<ChunkStrip>
	{
		private const string _reversed = nameof(ChunkStrip.Reversed);
		private const string _corners = nameof(ChunkStrip.Corners);

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _reversed, new(PropertyTokenType.Bool, false) },
			{ _corners, new(PropertyTokenType.Array, null) },
		});

		/// <inheritdoc/>
		protected override object? ReadValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _reversed:
					return reader.GetBoolean();
				case _corners:
					return JsonSerializer.Deserialize<ChunkCorner[]>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override ChunkStrip Create(ReadOnlyDictionary<string, object?> values)
		{
			ChunkCorner[] corners = (ChunkCorner[]?)values[_corners]
				?? throw new InvalidDataException($"Chunk strip requires \"{_corners}\" property!");

			return new(corners, (bool)values[_reversed]!);
		}

		/// <inheritdoc/>
		protected override void WriteValues(Utf8JsonWriter writer, ChunkStrip value, JsonSerializerOptions options)
		{
			if(value.Reversed)
			{
				writer.WriteBoolean(_reversed, value.Reversed);
			}

			writer.WritePropertyName(_corners);
			JsonSerializer.Serialize(writer, value.Corners, options);
		}
	}
}
