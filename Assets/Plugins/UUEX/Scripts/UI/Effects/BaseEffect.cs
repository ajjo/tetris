using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace UUEX.UI.Effects
{
	public abstract class BaseEffect : BaseMonoBehavior 
	{
		public UnityAction<string, UIBase> _Listener;
		public float _Duration;
		public string _ActionName;
		public bool _YoYo = false;
		public bool _PlayOnStart = false;
		public int _RepeatCount = -1;

		public UI _Ui = null;
		public UIItem _UiItem = null;

		protected float mStartTime = 0.0f;
		protected bool mStarted = false;
		protected bool mIsRepeat = false;
		protected bool mIn = true;
		protected int mRepeatCount = 0;
		protected int mCount = 1;

		public abstract void PlayIn ();
		public abstract void PlayOut ();

		protected virtual void Start()
		{
			mStartTime = Time.realtimeSinceStartup;
			mIsRepeat = (_RepeatCount > 0);
			mRepeatCount = _RepeatCount;

			if(_PlayOnStart)
				PlayIn();
		}

		public override void Update()
		{
			if (!mStarted)
				return;

			if((Time.realtimeSinceStartup - mStartTime) > _Duration)
			{
				if(_YoYo)
				{
					if(!mIsRepeat && mRepeatCount <= 0)
						NextEffect();
					else
					{
						if(mRepeatCount == 0)
						{
							InvokeListener();
							return;
						}
						else
						{
							mCount++;
							if(mCount % 2 == 0)
								mRepeatCount--;

							NextEffect();
						}
					}
				}
				else
					InvokeListener();
			}
		}

		protected virtual void NextEffect()
		{
			mIn = !mIn;
			mStartTime = Time.realtimeSinceStartup;
			
			if(mIn)
				PlayIn();
			else
				PlayOut();
		}

		protected virtual void InvokeListener()
		{
			if(_Listener != null)
			{
				_Listener(_ActionName, _Ui);
				_Listener = null;
			}

			Stop ();
		}

		public virtual void Stop(bool resetToOriginal = false)
		{
			if(resetToOriginal)
			{
				if(_UiItem != null)
					_UiItem.ResetToOriginal();
			}
			
			GameObject.Destroy (this);
		}
	}
}
