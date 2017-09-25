using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

namespace UUEX.UI
{
	[RequireComponent(typeof(ToggleGroup))]
	public class UIRadioButtons : UIItem 
	{
		private ToggleGroup mToggleGroup;
		private List<UIToggleButton> mToggleButtons = new List<UIToggleButton> ();

		public override void Awake ()
		{
			base.Awake ();

			mToggleGroup = GetComponent<ToggleGroup> ();

			UIToggleButton [] toggleButtons = GetComponentsInChildren<UIToggleButton> ();
			mToggleButtons = new List<UIToggleButton> (toggleButtons);
		}

		public override void Start ()
		{
			base.Start ();

			foreach(UIToggleButton toggleButton in mToggleButtons)
				toggleButton.SetGroup(mToggleGroup);
		}

		public UIToggleButton GetSelectedItem()
		{
			foreach(UIToggleButton toggleButton in mToggleButtons)
			{
				if(toggleButton.IsChecked())
					return toggleButton;
			}

			return null;
		}

		public void SetSelectedItem(UIToggleButton button)
		{
			foreach(UIToggleButton toggleButton in mToggleButtons)
			{
				if(toggleButton == button)
					toggleButton.SetChecked();
				else
					toggleButton.SetUnchecked();
			}
		}

		public override void SetInteractive (bool interactive)
		{
			base.SetInteractive (interactive);

			foreach (UIToggleButton toggleButton in mToggleButtons) 
			{
				toggleButton.SetInteractive (interactive);
			}
		}

		public override void SetDisabled (bool disabled)
		{
			base.SetDisabled (disabled);

			foreach (UIToggleButton toggleButton in mToggleButtons) 
			{
				toggleButton.SetDisabled(disabled);
			}
		}

		protected override void UpdateVisibility (bool visibility)
		{
			base.UpdateVisibility(visibility);

			foreach(UIToggleButton tb in mToggleButtons)
				tb.SetVisibility(visibility);
		}
		
		protected override void UpdateInteractivity (bool interactive)
		{
			base.UpdateInteractivity(interactive);
			
			foreach(UIToggleButton tb in mToggleButtons)
				tb.SetInteractive(interactive);
		}

	}
}
