using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;

namespace UUEX.UI
{
	/// <summary>
	/// UIText
	/// UI Text implementation
	/// </summary>
	[RequireComponent(typeof(Text))]
	public class UIText : UIItem 
	{
		public LocaleString _LocaleText;
		private Text mText;

		public override void Awake()
		{
			base.Awake ();

			mText = GetComponent<Text> ();
		}

		public override void Start()
		{
			base.Start ();

			if(!string.IsNullOrEmpty(_LocaleText._Text))
				mText.text = _LocaleText.GetLocalisedString ();
		}

		public void SetText(string newText)
		{
			mText.text = newText;
		}

		public void SetText(object value)
		{
			mText.text = value.ToString();
		}

		protected override void UpdateVisibility (bool visibility)
		{
			base.UpdateVisibility (visibility);
			
			mText.canvasRenderer.cull = !visibility;
		}
		
		protected override void UpdateInteractivity (bool interactive)
		{
			base.UpdateInteractivity (interactive);

			// Anything needed here?
		}
	}
}
