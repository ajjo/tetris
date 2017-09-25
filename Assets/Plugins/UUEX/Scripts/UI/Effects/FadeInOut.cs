using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UUEX.UI;

namespace UUEX.UI.Effects
{
	public class FadeInOut : BaseEffect
	{
		public float _FadeInValue = 0.0f;
		public float _FadeOutValue = 1.0f;
		
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
				if(_Ui != null)
				{
					List<UIItem> items = _Ui.GetItems ();

					foreach(UIItem item in items)
						UpdateEffect(item);
				}
				else if(_UiItem != null)
				{
					UpdateEffect(_UiItem);
				}
			}
		}

		private void UpdateEffect(UIItem item)
		{
			List<GraphicInfo> graphics = _UiItem.GetGraphics();

			float t = (Time.realtimeSinceStartup - mStartTime) / _Duration;

			foreach(GraphicInfo graphicInfo in graphics)
			{
				if(mIn)
				{
					Color color = graphicInfo._Graphic.color;
					color.a = _FadeInValue;
					graphicInfo._Graphic.color = Color.Lerp(graphicInfo._Graphic.color,color,t);
				}
				else
				{
					Color color = graphicInfo._Graphic.color;
					color.a = _FadeOutValue;
					graphicInfo._Graphic.color = Color.Lerp(graphicInfo._Graphic.color,color,t);
				}
			}
		}
	}
}