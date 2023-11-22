using System.Text.Json.Serialization;

namespace SA3D.Modeling.JSON
{
	/// <summary>
	/// Reference handler.
	/// </summary>
	public class JsonReferenceHandler : ReferenceHandler
	{
		/// <inheritdoc/>
		public JsonReferenceHandler()
		{
			Reset();
		}

		private JsonReferenceResolver? _rootedResolver;

		/// <inheritdoc/>
		public override JsonReferenceResolver CreateResolver()
		{
			return _rootedResolver!;
		}

		/// <inheritdoc/>
		public void Reset()
		{
			_rootedResolver = new JsonReferenceResolver();
		}
	}
}
