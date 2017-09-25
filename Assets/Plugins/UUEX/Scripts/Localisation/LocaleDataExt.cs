using UnityEngine;
using System.Xml.Serialization;
using System.IO;
using System.Collections;

namespace UUEX
{
	/// <summary>
	/// Locale data.
	/// Extension of the LocaleData class. All implementation goes here.
	/// </summary>
	public partial class LocaleData
	{
		public static LocaleData pInstance = null;

		private LocaleData()
		{
		}

		public static void Initialize(TextAsset asset)
		{
			XmlSerializer deserializer = new XmlSerializer(typeof(LocaleData));
			MemoryStream assetStream = new MemoryStream(asset.bytes);
			TextReader reader = new StreamReader(assetStream);
			object obj = deserializer.Deserialize(reader);
			pInstance = (LocaleData)obj;
			assetStream.Close ();
			reader.Close();
		}

		public string GetStringFromID(string defaultText, int textID)
		{
			foreach (LocaleText lt in pInstance.TextList)
				if (lt.TextID == textID)
					return lt.Text;

			return defaultText;
		}
	}
}
