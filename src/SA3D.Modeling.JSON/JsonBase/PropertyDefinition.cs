namespace SA3D.Modeling.JSON.JsonBase
{
	/// <summary>
	/// Property definition information
	/// </summary>
	public readonly struct PropertyDefinition
	{
		/// <summary>
		/// Type of the property.
		/// </summary>
		public PropertyTokenType Type { get; }

		/// <summary>
		/// Default value for when the property is not part of the object.
		/// </summary>
		public object? Default { get; }

		/// <summary>
		/// Whether the property can be null.
		/// </summary>
		public bool Nullable { get; }


		/// <summary>
		/// Creates a new property definition.
		/// </summary>
		/// <param name="type">Type of the property.</param>
		/// <param name="fallback">Fallback value for when the property is not part of the object.</param>
		public PropertyDefinition(PropertyTokenType type, object? fallback)
		{
			Default = fallback;
			Type = type;
			Nullable = false;
		}

		/// <summary>
		/// Creates a new property definition.
		/// </summary>
		/// <param name="type">Type of the property.</param>
		/// <param name="fallback">Fallback value for when the property is not part of the object.</param>
		/// <param name="nullable">Whether the property can be null.</param>
		public PropertyDefinition(PropertyTokenType type, object? fallback, bool nullable)
		{
			Default = fallback;
			Type = type;
			Nullable = nullable;
		}
	}
}
