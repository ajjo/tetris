using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace UUEX.UI
{
	[RequireComponent(typeof(Toggle))]
	public class UIToggleButton : UIButton
	{
		private Toggle mToggle;
		private Image mBackgroundImage = null;
		private Image mCheckmarkImage = null;

		public override void Awake ()
		{
			transform = GetComponent<Transform> ();

			mToggle = GetComponent<Toggle> ();
			mToggle.interactable = _Interactive;

			mBackgroundImage = (Image)mToggle.targetGraphic;
			mCheckmarkImage = (Image)mToggle.graphic;

			mText = GetComponentInChildren<Text> ();
		}

		public override void Start ()
		{
			base.Start ();
		}

		public bool IsChecked()
		{
			return mToggle.isOn;
		}

		public void SetChecked()
		{
			mToggle.isOn = true;
		}

		public void SetUnchecked()
		{
			mToggle.isOn = false;
		}

		public void SetGroup(ToggleGroup group)
		{
			mToggle.group = group;
		}

		public override void AsyncUpdate (WWWAsync.DownloadState progression, object result, object userData)
		{
			if (progression == UUEX.UI.WWWAsync.DownloadState.COMPLETED) 
			{
				Texture2D texture = result as Texture2D;

				Sprite sprite = Sprite.Create(texture, new Rect(0,0,texture.width,texture.height), new Vector2(0.5f,0.5f));
				mBackgroundImage.sprite = sprite;
			}
		}

		public override void SetTextureFromURL (string url)
		{
			ResourceManager.DownloadAsset(this, url, typeof(Texture2D));
		}

		public override void SetTexture (Texture2D texture)
		{
			Sprite backgroundSprite = Sprite.Create (texture, new Rect (0, 0, texture.width, texture.height), new Vector2 (0.5f, 0.5f));
			mBackgroundImage.sprite = backgroundSprite;
		}

		public override void SetSprite (string atlasName, string spriteName)
		{
			Sprite sprite = ResourceManager.GetSprite (atlasName, spriteName);
			
			if(sprite != null)
			{
				mBackgroundImage.sprite = sprite;
			}
		}

		public override void SetSprite(Sprite sprite)
		{
			mBackgroundImage.sprite = sprite;
		}

		public void SetCheckmarkTexture(Texture2D checkmarkTexture)
		{
			Sprite checkmarkSprite = Sprite.Create (checkmarkTexture, new Rect (0, 0, checkmarkTexture.width, checkmarkTexture.height), new Vector2 (0.5f, 0.5f));
			mCheckmarkImage.sprite = checkmarkSprite;
		}

		public void SetCheckmarkTexture(Sprite checkmarkSprite)
		{
			mCheckmarkImage.sprite = checkmarkSprite;
		}

		public override void SetInteractive (bool interactive)
		{
			base.SetInteractive (interactive);
		
			mToggle.interactable = interactive;
		}

		public override void SetDisabled (bool disabled)
		{
			base.SetDisabled (disabled);

			mToggle.enabled = !disabled;
		}

		protected override void UpdateVisibility (bool visibility)
		{
			mVisible = visibility;
			
			mBackgroundImage.canvasRenderer.cull = !visibility;
			mCheckmarkImage.canvasRenderer.cull = !visibility;
			mText.canvasRenderer.cull = !visibility;
		}
		
		protected override void UpdateInteractivity (bool interactive)
		{
			mInteractive = interactive;
			
			mToggle.interactable = interactive;
		}

	}
}
