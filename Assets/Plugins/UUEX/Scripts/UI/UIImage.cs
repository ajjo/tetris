using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UUEX.UI
{
	public class UIImage : UIItem
	{
		protected UIAnim2D mAnim2D;
		private RawImage mRawImage = null;
		private Image mImage = null;

		public override void Awake ()
		{
			base.Awake ();

			mRawImage = GetComponent<RawImage>();
			mImage = GetComponent<Image>();

			if(mRawImage == null && mImage == null)
				Debug.LogWarning("UIImage needs either RawImage or Image component");

			mAnim2D = GetComponent<UIAnim2D>();
			if(mAnim2D != null)
				mAnim2D.AddAnimUpdateListener(AnimUpdate);
		}

		public override void AsyncUpdate(WWWAsync.DownloadState progression, object result, object userData)
		{
			base.AsyncUpdate(progression, result, userData);

			if (progression == UUEX.UI.WWWAsync.DownloadState.COMPLETED && result is Texture2D) 
			{
				Texture2D texture = result as Texture2D;

				if(mRawImage != null)
					mRawImage.texture = texture;
			}
		}
		
		public void SetTextureFromURL(string url)
		{
			ResourceManager.DownloadAsset(this, url, typeof(Texture2D));
		}
		
		public void SetTexture(Texture2D texture)
		{
			if(mRawImage != null)
				mRawImage.texture = texture;
		}

		public void SetSprite(Sprite sprite)
		{
			if(mImage != null)
				mImage.sprite = sprite;
		}

		public void AnimUpdate(Sprite sprite)
		{
			SetSprite(sprite);
		}

		protected override void UpdateVisibility (bool visibility)
		{
			base.UpdateVisibility (visibility);
			
			if(mImage != null)
				mImage.canvasRenderer.cull = visibility;

			if(mRawImage != null)
				mRawImage.canvasRenderer.cull = visibility;
		}
		
		protected override void UpdateInteractivity (bool interactive)
		{
			base.UpdateInteractivity (interactive);

			// Anything needed here?
		}
	
	}
}
