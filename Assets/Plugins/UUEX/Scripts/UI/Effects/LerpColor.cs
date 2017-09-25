using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UUEX.UI.Effects
{
	public class LerpColor : BaseEffect
	{
		public Color _TargetColor;
		
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
					graphicInfo._Graphic.color = Color.Lerp(graphicInfo._Graphic.color,_TargetColor,t);
				}
				else
				{
					graphicInfo._Graphic.color = Color.Lerp(graphicInfo._Graphic.color,graphicInfo._OriginalColor,t);
				}
			}
		}
	}
}
