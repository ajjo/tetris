using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UUEX.UI
{
	/// <summary>
	/// UIButton
	/// UI Button implementation
	/// </summary>
	[RequireComponent(typeof(Button))]
	public class UIButton : UIItem
	{
		public LocaleString _LocaleText;
		protected Text mText;
		protected UIAnim2D mAnim2D;

		protected Image mImage;
		protected Button mButton;

		public override void Awake()
		{
			base.Awake ();

			mButton = GetComponent<Button> ();
			mButton.interactable = _Interactive;

			mText = GetComponentInChildren<Text> ();
			mImage = mButton.image;

			mAnim2D = GetComponent<UIAnim2D>();
			if(mAnim2D != null)
				mAnim2D.AddAnimUpdateListener(AnimUpdate);
		}

		public override void Start()
		{
			base.Start ();

			if(!string.IsNullOrEmpty(_LocaleText._Text))
				mText.text = _LocaleText.GetLocalisedString ();
		}

		public void SetText(string newText)
		{
			mText.text = newText;
		}

		public override void AsyncUpdate(WWWAsync.DownloadState progression, object result, object userData)
		{
			base.AsyncUpdate(progression, result, userData);

			if (progression == UUEX.UI.WWWAsync.DownloadState.COMPLETED && result is Texture2D) 
			{
				Texture2D texture = result as Texture2D;

				Image image = GetComponent<Image>();
				Sprite sprite = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height), new Vector2(0.5f,0.5f));
				image.sprite = sprite;
			}
		}
		
		public virtual void SetTextureFromURL(string url)
		{
			ResourceManager.DownloadAsset(this, url, typeof(Texture2D));
		}
		
		public virtual void SetTexture(Texture2D texture)
		{
			Image image = GetComponent<Image>();
			Sprite sprite = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height), new Vector2(0.5f,0.5f));
			image.sprite = sprite;
		}

		public virtual void SetSprite(string atlasName, string spriteName)
		{
			Sprite sprite = ResourceManager.GetSprite (atlasName, spriteName);
			
			if(sprite != null)
			{
				Image image = GetComponent<Image> ();
				image.sprite = sprite;
			}
		}

		public virtual void SetSprite(Sprite sprite)
		{
			Image image = GetComponent<Image> ();
			image.sprite = sprite;
		}

		public override void SetDisabled (bool disabled)
		{
			base.SetDisabled (disabled);

			mButton.enabled = !disabled;
		}

		protected override void UpdateVisibility (bool visibility)
		{
			base.UpdateVisibility (visibility);

			mText.canvasRenderer.cull = !visibility;
			mImage.canvasRenderer.cull = !visibility;
		}

		protected override void UpdateInteractivity (bool interactive)
		{
			base.UpdateInteractivity (interactive);

			mButton.interactable = interactive;
		}

		public void AnimUpdate(Sprite sprite)
		{
			SetSprite(sprite);
		}

		public UIAnim2D GetAnim()
		{
			return mAnim2D;
		}
	}
}
