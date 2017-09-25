using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace UUEX.UI
{
	public class UIAnim2D : BaseMonoBehavior
	{
		public Sprite [] _Sprites;
		public float _Duration;
		public bool _PlayOnStart = false;
		public bool _Loop = false;

		private UnityAction<Sprite> onAnimUpdateEvent = null;
		private float mTimeBetweenSprites = 0.1f;
		private float mTime = 0.0f;
		private int mCurrentIndex = 0;
		private bool mAnimEnded = false;
		private bool mAnimStarted = false;

		public void Start ()
		{
			if(_PlayOnStart)
				Play();
		}

		public override void Update ()
		{
			base.Update ();

			if(mAnimStarted && !mAnimEnded && (Time.realtimeSinceStartup - mTime) >= mTimeBetweenSprites)
			{
				mTime = Time.realtimeSinceStartup;
				mCurrentIndex++;

				if(mCurrentIndex == _Sprites.Length)
				{
					if(_Loop)
						mCurrentIndex = 0;
					else
					{
						mAnimEnded = true;
						mAnimStarted = false;
					}
				}

				if(onAnimUpdateEvent != null)
					onAnimUpdateEvent(_Sprites[mCurrentIndex]);
			}
		}

		public void AddAnimUpdateListener(UnityAction<Sprite> action)
		{
			onAnimUpdateEvent += action;
		}

		public void RemoveAnimUpdateListener(UnityAction<Sprite> action)
		{
			onAnimUpdateEvent -= action;
		}

		public void Play()
		{
			mTimeBetweenSprites = _Duration / _Sprites.Length;
			mTime = Time.realtimeSinceStartup;

			mAnimStarted = true;

			if(onAnimUpdateEvent != null)
				onAnimUpdateEvent(_Sprites[mCurrentIndex]);
		}

		public void Stop(bool stopAtStart = false, bool stopAtEnd = false)
		{
			mAnimStarted = false;
			mAnimEnded = true;
			mCurrentIndex = 0;

			if(onAnimUpdateEvent != null)
			{
				if(stopAtEnd)
					onAnimUpdateEvent(_Sprites[_Sprites.Length - 1]);
				else if(stopAtStart)
					onAnimUpdateEvent(_Sprites[0]);
			}
		}
	}
}
