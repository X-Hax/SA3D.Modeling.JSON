using SA3D.Modeling.JSON.JsonBase;
using SA3D.Modeling.Mesh.Gamecube.Enums;
using SA3D.Modeling.Mesh.Gamecube.Parameters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text.Json;

namespace SA3D.Modeling.JSON.Mesh.Gamecube.Parameters
{
	/// <summary>
	/// Json converter for <see cref="GCLightingParameter"/>
	/// </summary>
	public class GCLightingParameterJsonConverter : ChildJsonObjectConverter<GCParameterType, GCLightingParameter, IGCParameter>
	{
		private const string _lightingAttributes = nameof(GCLightingParameter.LightingAttributes);
		private const string _shadowStencil = nameof(GCLightingParameter.ShadowStencil);
		private const string _unknown1 = nameof(GCLightingParameter.Unknown1);
		private const string _unknown2 = nameof(GCLightingParameter.Unknown2);


		/// <inheritdoc/>
		protected override ParentJsonObjectConverter<GCParameterType, IGCParameter> ParentConverter => GCParameterJsonConverter._globalGCParameterConverter;

		/// <inheritdoc/>
		protected override ReadOnlyDictionary<string, PropertyDefinition> TargetPropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _lightingAttributes, new(PropertyTokenType.String, (ushort)0u) },
			{ _shadowStencil, new(PropertyTokenType.Number, (byte)0) },
			{ _unknown1, new(PropertyTokenType.Number, (byte)0) },
			{ _unknown2, new(PropertyTokenType.Number, (byte)0) },
		});


		/// <inheritdoc/>
		protected override bool CheckTypeMatches(GCParameterType key)
		{
			return key == GCParameterType.Lighting;
		}

		/// <inheritdoc/>
		protected override object? ReadTargetValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _lightingAttributes:
					return reader.GetString()!.HexToUShort("Lighting attributes");
				case _shadowStencil:
				case _unknown1:
				case _unknown2:
					return reader.GetByte();
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override GCLightingParameter CreateTarget(ReadOnlyDictionary<string, object?> values)
		{
			return new()
			{
				LightingAttributes = (ushort)values[_lightingAttributes]!,
				ShadowStencil = (byte)values[_shadowStencil]!,
				Unknown1 = (byte)values[_unknown1]!,
				Unknown2 = (byte)values[_unknown2]!,
			};
		}

		/// <inheritdoc/>
		protected override void WriteTargetValues(Utf8JsonWriter writer, GCLightingParameter value, JsonSerializerOptions options)
		{
			writer.WriteString(_lightingAttributes, value.LightingAttributes.ToString("X", NumberFormatInfo.InvariantInfo));
			writer.WriteNumber(_shadowStencil, value.ShadowStencil);
			writer.WriteNumber(_unknown1, value.Unknown1);
			writer.WriteNumber(_unknown2, value.Unknown2);
		}
	}
}
