using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace  TheGamedevGuru
{
	public static class PlayFabAddressablesManager
	{
		/// <summary>
		/// If true, we download from the CDN. This is faster but is cached up to 24h, so set it to false if you always want to have the latest version.
		/// In production builds it should be left to true.
		/// </summary>
		public static bool UseCdn = true;
	}
}
