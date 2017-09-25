using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UUEX.UI
{
	/// <summary>
	/// UIInput - Locale, visibility, enabled etc... visiblity, enabled at run time
	/// </summary>
	[RequireComponent(typeof(InputField))]
	public class UIInput : UIItem
	{
		public LocaleString _LocaleText;
		private InputField mInputField = null;

		private CanvasRenderer [] mCanvasRenderers = null;

		public override void Awake ()
		{
			base.Awake ();

			mInputField = GetComponent<InputField>();

			mCanvasRenderers = GetComponentsInChildren<CanvasRenderer>();
		}

		public override void Start ()
		{
			base.Start ();

			if(!string.IsNullOrEmpty(_LocaleText._Text))
				mInputField.text = _LocaleText.GetLocalisedString ();
		}

		public string GetText()
		{
			return mInputField.text;
		}

		public InputField GetInputField()
		{
			return mInputField;
		}
		
		protected override void UpdateVisibility (bool visibility)
		{
			base.UpdateVisibility (visibility);

			foreach(CanvasRenderer cr in mCanvasRenderers)
				cr.cull = !visibility;

			// Pending caret issue - fixed in 5.3.2... update!!!
		}
		
		protected override void UpdateInteractivity (bool interactive)
		{
			base.UpdateInteractivity (interactive);
			
			mInputField.interactable = interactive;
		}

	}
}
