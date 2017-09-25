using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace UUEX
{
	/// <summary>
	/// Locale data.
	/// Contains the list of text to be localised
	/// </summary>
	[System.Serializable]
	public partial class LocaleData
	{
		[XmlElement("LocaleText")] 
		public List<LocaleText> TextList = new List<LocaleText>(); 
	}

	/// <summary>
	/// Locale text.
	/// Contains the string and the associated locale ID
	/// </summary>
	public class LocaleText
	{
		[XmlElement("Text")]
		public string Text { get; set; }
		[XmlElement("TextID")] 
		public int TextID { get; set; } 
	}
}
