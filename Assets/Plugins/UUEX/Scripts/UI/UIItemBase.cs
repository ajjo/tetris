using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace UUEX.UI
{
	public class GraphicInfo
	{
		public Graphic _Graphic;
		public Color _OriginalColor;
	}
	
	public class UIItemBase : UIBehavior 
	{
		private Vector3 mOriginalScale;

		public List<GraphicInfo> mGraphicInfo = new List<GraphicInfo> ();

		public virtual void Start()
		{
			mOriginalScale = transform.localScale;

			Graphic [] graphics = GetComponentsInChildren<Graphic>();

			foreach(Graphic graphic in graphics)
			{
				GraphicInfo graphicInfo = new GraphicInfo();

				graphicInfo._Graphic = graphic;
				graphicInfo._OriginalColor = graphic.color;

				mGraphicInfo.Add(graphicInfo);
			}
		}

		public List<GraphicInfo> GetGraphics()
		{
			return mGraphicInfo;
		}

		public void FadeIn(float value = 0.0f, float duration = 0.5f)
		{
			Graphic [] graphics = GetComponentsInChildren<Graphic> ();
			foreach (Graphic g in graphics)
				g.CrossFadeAlpha (value, duration, false);
		}
		
		public void FadeOut(float value = 1.0f, float duration = 0.5f)
		{
			Graphic [] graphics = GetComponentsInChildren<Graphic> ();
			foreach (Graphic g in graphics)
				g.CrossFadeAlpha (value, duration, false);
		}

		public void ColorLerpIn(Color targetColor, float duration = 0.5f)
		{
			Graphic [] graphics = GetComponentsInChildren<Graphic> ();
			foreach (Graphic g in graphics)
				g.CrossFadeColor(targetColor, duration, false, true);
		}

		public void ColorLerpOut(float duration = 0.5f)
		{
			foreach(GraphicInfo graphicInfo in mGraphicInfo)
				graphicInfo._Graphic.CrossFadeColor(graphicInfo._OriginalColor, duration, false, true);
		}

		public void ScaleLerpIn(float scale, float duration = 0.5f)
		{

		}

		public void ScaleLerpOut(float scale, float duration = 0.5f)
		{

		}

		public void ResetToOriginal()
		{
			foreach(GraphicInfo graphicInfo in mGraphicInfo)
			{
				graphicInfo._Graphic.color = graphicInfo._OriginalColor;
				graphicInfo._Graphic.CrossFadeAlpha(1.0f,0.1f,true);
			}

			transform.localScale = mOriginalScale;
		}
	}
}
