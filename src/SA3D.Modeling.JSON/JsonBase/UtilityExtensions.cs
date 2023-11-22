using System;
using System.Globalization;

namespace SA3D.Modeling.JSON.JsonBase
{
	internal static class UtilityExtensions
	{
		public static byte HexToByte(this string input, string targetDebug)
		{
			if(!byte.TryParse(input, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out byte attribute))
			{
				throw new FormatException(targetDebug + " are ill formated! Require hex string!");
			}

			return attribute;
		}

		public static ushort HexToUShort(this string input, string targetDebug)
		{
			if(!ushort.TryParse(input, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out ushort attribute))
			{
				throw new FormatException(targetDebug + " are ill formated! Require hex string!");
			}

			return attribute;
		}

		public static uint HexToUInt(this string input, string targetDebug)
		{
			if(!uint.TryParse(input, NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out uint attribute))
			{
				throw new FormatException(targetDebug + " are ill formated! Require hex string!");
			}

			return attribute;
		}
	}
}
