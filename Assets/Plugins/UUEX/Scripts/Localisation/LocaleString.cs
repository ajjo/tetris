using UnityEngine;
using System.Collections;

namespace UUEX
{
	/// <summary>
	/// Locale string.
	/// Gives a localised string for a given text based on ID
	/// </summary>
	[System.Serializable]
	public class LocaleString 
	{
		public string _Text;
		public int _TextID;

		public string GetLocalisedString()
		{
			return LocaleData.pInstance.GetStringFromID(_Text, _TextID);
		}
	}
}
