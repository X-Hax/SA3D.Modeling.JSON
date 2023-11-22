using System;

namespace SA3D.Modeling.JSON.JsonBase
{
	/// <summary>
	/// Type of the property
	/// </summary>
	[Flags]
	public enum PropertyTokenType
	{
		/// <summary>
		/// Boolean.
		/// </summary>
		Bool = 0x01,

		/// <summary>
		/// Number.
		/// </summary>
		Number = 0x02,

		/// <summary>
		/// Object.
		/// </summary>
		Object = 0x04,

		/// <summary>
		/// Array.
		/// </summary>
		Array = 0x08,

		/// <summary>
		/// String.
		/// </summary>
		String = 0x10,

		/// <summary>
		/// Any.
		/// </summary>
		Any = Bool | Number | Object | Array | String
	}
}
