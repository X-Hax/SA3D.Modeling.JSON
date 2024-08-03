using SA3D.Common.Lookup;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SA3D.Modeling.JSON
{
	/// <summary>
	/// Reference resolver.
	/// </summary>
	public class JsonReferenceResolver : ReferenceResolver
	{
		private uint _referenceCount;

		private readonly Dictionary<string, object> _referenceIdToObjectMap = [];
		private readonly Dictionary<object, string> _objectToReferenceIdMap = new(ReferenceEqualityComparer.Instance);

		/// <inheritdoc/>
		public override void AddReference(string referenceId, object value)
		{
			if(!_referenceIdToObjectMap.TryAdd(referenceId, value))
			{
				throw new JsonException();
			}
		}

		/// <inheritdoc/>
		public override string GetReference(object value, out bool alreadyExists)
		{
			if(_objectToReferenceIdMap.TryGetValue(value, out string? referenceId))
			{
				alreadyExists = true;
			}
			else
			{
				_referenceCount++;

				referenceId = value is ILabel label
					? label.Label
					: _referenceCount.ToString();

				_objectToReferenceIdMap.Add(value, referenceId);
				alreadyExists = false;
			}

			return referenceId;
		}

		/// <inheritdoc/>
		public override object ResolveReference(string referenceId)
		{
			if(!_referenceIdToObjectMap.TryGetValue(referenceId, out object? value))
			{
				throw new JsonException();
			}

			return value;
		}

	}
}
