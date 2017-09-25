using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UUEX.UI.Effects;

namespace UUEX.UI.Tutorial
{
	[System.Serializable]
	public class StepInfo
	{
		public bool _IsTheOnlyInteractiveItem;
		public bool _IsTheOnlyEnabledItem;
		public bool _IsTheOnlyVisibleItem;
	}

	public class NormalStep : BaseMonoBehavior
	{
		public StepInfo _StepBegin;
		public StepInfo _StepEnd;

		public UI _UI;
		public string _ItemName;

		protected BaseEffect [] mEffects;
		private TutorialManager mTutorialManager;

		public override void Awake()
		{
			base.Awake ();

			mEffects = GetComponents<BaseEffect> ();

			foreach (BaseEffect effect in mEffects)
				effect.enabled = false;

			mTutorialManager = GetComponentInParent<TutorialManager> ();
		}

		public virtual void Start()
		{
			if(_StepBegin._IsTheOnlyEnabledItem)
			{
				List<UIItem> items = _UI.GetItems();
				if(items != null)
				{
					foreach(UIItem item in items)
					{
						if(!item.HasName(_ItemName))
							item.SetDisabled(true);
					}
				}
			}
			else if(_StepBegin._IsTheOnlyInteractiveItem)
			{
				List<UIItem> items = _UI.GetItems();
				if(items != null)
				{
					foreach(UIItem item in items)
					{
						if(!item.HasName(_ItemName))
							item.SetInteractive(false);
					}
				}
			}
		}

		public void ItemClicked(UIItem item)
		{
			if(item.HasName(_ItemName))
			{
				Debug.Log ("Executing the next step " + item.GetName());
				End ();
				mTutorialManager.NextStep();
			}
		}

		public virtual void Begin()
		{
			enabled = true;
			_UI.AddItemClickListener (ItemClicked);

			foreach(BaseEffect effect in mEffects)
			{
				effect.enabled = true;
				effect.PlayIn();
			}
		}

		public virtual void End()
		{
			enabled = false;
			_UI.RemoveItemClickListener(ItemClicked);

			foreach (BaseEffect effect in mEffects)
				effect.Stop(true);

			if(!_StepBegin._IsTheOnlyEnabledItem)
			{
				List<UIItem> items = _UI.GetItems();
				if(items != null)
				{
					foreach(UIItem item in items)
						item.SetDisabled(false);
				}
			}
		}
	}
}
