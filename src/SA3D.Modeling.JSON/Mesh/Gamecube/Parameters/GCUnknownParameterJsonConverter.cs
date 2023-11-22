using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Gamecube.Enums;
using SA3D.Modeling.Mesh.Gamecube.Parameters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Gamecube.Parameters
{
	/// <summary>
	/// Json converter for <see cref="GCUnknownParameter"/>
	/// </summary>
	public class GCUnknownParameterJsonConverter : ChildJsonObjectConverter<GCParameterType, GCUnknownParameter, IGCParameter>
	{
		private const string _unknown1 = nameof(GCUnknownParameter.Unknown1);
		private const string _unknown2 = nameof(GCUnknownParameter.Unknown2);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<GCParameterType, IGCParameter> ParentConverter => GCParameterJsonConverter._globalGCParameterConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _unknown1, new(PropertyTokenType.Number, (ushort)0u) },
			{ _unknown2, new(PropertyTokenType.Number, (ushort)0u) },
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(GCParameterType key)
		{
			return key == GCParameterType.Unknown;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _unknown1:
				case _unknown2:
					return reader.GetUInt16();
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override GCUnknownParameter CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				Unknown1 = (ushort)values[_unknown1]!,
				Unknown2 = (ushort)values[_unknown2]!,
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, GCUnknownParameter value, JsonSerializerOptions options)
		{
			writer.WriteNumber(_unknown1, value.Unknown1);
			writer.WriteNumber(_unknown2, value.Unknown2);
		}
	}
}
