using SA3D.Modeling.Mesh.Gamecube.Parameters;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text.Json;
using SA3D.Modeling.Mesh.Gamecube.Enums;
using SA3D.Modeling.JSON.JsonBase;
using System;

namespace SA3D.Modeling.JSON.Mesh.Gamecube.Parameters
{
	/// <summary>
	/// Json converter for <see cref="IGCParameter"/>
	/// </summary>
	public class GCParameterJsonConverter : ParentJsonObjectConverter<GCParameterType, IGCParameter>
	{
		internal static readonly GCParameterJsonConverter _globalGCParameterConverter = new();

		internal const string _type = nameof(IGCParameter.Type);

		/// <inheritdoc/>
		protected internal override string KeyPropertyName => _type;

		/// <inheritdoc/>
		public override ReadOnlyDictionary<string, PropertyDefinition> PropertyDefinitions { get; } = new(new Dictionary<string, PropertyDefinition>()
		{
			{ _type, new(PropertyTokenType.String, null) },
		});

		/// <inheritdoc/>
		protected internal override object? ReadBaseValue(ref Utf8JsonReader reader, string propertyName, ReadOnlyDictionary<string, object?> values, JsonSerializerOptions options)
		{
			switch(propertyName)
			{
				case _type:
					return JsonSerializer.Deserialize<GCParameterType>(ref reader, options);
				default:
					throw new InvalidPropertyException();
			}
		}

		/// <inheritdoc/>
		protected override IGCParameter CreateBase(ReadOnlyDictionary<string, object?> values)
		{
			throw new NotSupportedException("Cannot create typeless gc parameter!");
		}

		/// <inheritdoc/>
		protected internal override void WriteBaseValues(Utf8JsonWriter writer, IGCParameter value, JsonSerializerOptions options)
		{
			writer.WritePropertyName(_type);
			JsonSerializer.Serialize(writer, value.Type, options);
		}

		/// <inheritdoc/>
		protected override GCParameterType GetKeyFromValue(IGCParameter value)
		{
			return value.Type;
		}

		/// <inheritdoc/>
		protected override Dictionary<GCParameterType, IChildJsonConverter<IGCParameter>> CreateConverters()
		{
			return new()
			{
				{ GCParameterType.VertexFormat, new GCVertexFormatParameterJsonConverter() },
				{ GCParameterType.IndexFormat, new GCIndexFormatParameterJsonConverter() },
				{ GCParameterType.Lighting, new GCLightingParameterJsonConverter() },
				{ GCParameterType.BlendAlpha, new GCBlendAlphaParameterJsonConverter() },
				{ GCParameterType.AmbientColor, new GCAmbientColorParameterJsonConverter() },
				{ GCParameterType.DiffuseColor, new GCDiffuseColorParameterJsonConverter() },
				{ GCParameterType.SpecularColor, new GCSpecularColorParameterJsonConverter() },
				{ GCParameterType.Texture, new GCTextureParameterJsonConverter() },
				{ GCParameterType.Unknown, new GCUnknownParameterJsonConverter() },
				{ GCParameterType.Texcoord, new GCTexCoordParameterJsonConverter() },
			};
		}
	}
}
