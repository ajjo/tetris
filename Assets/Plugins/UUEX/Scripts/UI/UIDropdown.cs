using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace UUEX.UI
{
	[RequireComponent(typeof(Dropdown))]
	public class UIDropdown : UIItem
	{
		private Dropdown mDropDown = null;
		private int mSelectedIndex = 0;

		private Image mArrow = null;

		public override void Awake ()
		{
			base.Awake ();

			mDropDown = GetComponent<Dropdown> ();
			mDropDown.onValueChanged.AddListener (DropdownValueChanged);

			Transform arrowTransform = transform.FindChild("Arrow");
			if(arrowTransform != null)
				mArrow = arrowTransform.GetComponent<Image>();
		}

		public void AddValueChangedListener(UnityAction<int> action)
		{
			mDropDown.onValueChanged.AddListener (action);
		}

		public void DropdownValueChanged(int item)
		{
			// 0 to N
			mSelectedIndex = item;
		}

		public int GetSelectedItemIndex()
		{
			return mSelectedIndex;
		}

		public void AddOption(Dropdown.OptionData optionData)
		{
			mDropDown.options.Add (optionData);
		}

		public void AddOption(string text)
		{
			Dropdown.OptionData newOption = new Dropdown.OptionData (text);
			mDropDown.options.Add (newOption);
		}

		public void AddOption(Sprite image)
		{
			Dropdown.OptionData newOption = new Dropdown.OptionData (image);
			mDropDown.options.Add (newOption);
		}

		public void AddOption(string text, Sprite image)
		{
			Dropdown.OptionData newOption = new Dropdown.OptionData (text, image);
			mDropDown.options.Add (newOption);
		}

		protected override void UpdateVisibility (bool visibility)
		{
			base.UpdateVisibility (visibility);

			if(mDropDown.itemImage != null)
				mDropDown.itemImage.canvasRenderer.cull = !visibility;

			if(mDropDown.captionText)
				mDropDown.captionText.canvasRenderer.cull = !visibility;
			if(mDropDown.captionImage)
				mDropDown.captionImage.canvasRenderer.cull = !visibility;
			if(mArrow != null)
				mArrow.canvasRenderer.cull = !visibility;

			mDropDown.itemText.canvasRenderer.cull = !visibility;
			mDropDown.image.canvasRenderer.cull = !visibility;

			mDropDown.Hide();
		}
		
		protected override void UpdateInteractivity (bool interactive)
		{
			base.UpdateInteractivity (interactive);
			
			mDropDown.interactable = interactive;
			mDropDown.Hide();
		}
	}
}
