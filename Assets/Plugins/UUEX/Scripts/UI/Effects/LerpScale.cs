using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UUEX.UI.Effects
{
	public class LerpScale : BaseEffect
	{
		public Vector3 _ScaleIn;
		public Vector3 _ScaleOut;

		public override void PlayIn()
		{
			mStarted = true;
			mIn = true;
		}

		public override void PlayOut()
		{
			mStarted = true;
			mIn = false;
		}

		public override void Update ()
		{
			base.Update ();

			if(mStarted)
			{
				float t = (Time.realtimeSinceStartup - mStartTime) / _Duration;
				
				if(mIn)
				{
					Vector3 itemScale = _UiItem.transform.localScale;
					_UiItem.transform.localScale = Vector3.Lerp(itemScale, _ScaleIn, t);
				}
				else
				{
					Vector3 itemScale = _UiItem.transform.localScale;
					_UiItem.transform.localScale = Vector3.Lerp(itemScale, _ScaleOut, t);
				}
			}
		}
	}
}

