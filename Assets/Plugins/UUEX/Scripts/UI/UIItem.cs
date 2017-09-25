using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

namespace UUEX.UI
{
	public class UIItem : UIItemBase, IWWWAsync
	{
		public bool _Interactive = true;
		public bool _Visible = true;

		public AudioClip _ClickSound;
		public string _ClickAudioURL = "";

		private UI mUI;
		private System.Object mItemData;

		protected bool mInteractive = true;
		protected bool mVisible = true;

		public override void Start()
		{
			base.Start ();

			Image image = GetComponent<Image> ();
			if(image != null && image.sprite != null)
			{
				Texture2D atlas = image.sprite.texture;
				ResourceManager.AddAtlas(atlas, image.sprite);
			}

			// Add a listener when the cull state changes
			MaskableGraphic maskableGraphic = GetComponent<MaskableGraphic> ();
			if(maskableGraphic != null)
				maskableGraphic.onCullStateChanged.AddListener(OnCullStateChanged);
		}

		// This is for the individual items
		public override void OnPointerClick(PointerEventData eventData)
		{
			if(mUI != null)
				mUI.OnItemClick (this);

			// Play the click sound
			PlaySound(_ClickAudioURL,_ClickSound);
		}

		private void PlaySound(string audioURL, AudioClip audioClip)
		{
			if(!string.IsNullOrEmpty(audioURL))
				ResourceManager.DownloadAsset(this, audioURL, typeof(AudioClip));
			else if(_ClickSound != null)
				SoundManager.Play(audioClip);
		}

		public virtual void AsyncUpdate(WWWAsync.DownloadState progression, object result, object userData)
		{
			if (progression == UUEX.UI.WWWAsync.DownloadState.COMPLETED && result != null && result is AudioClip) 
			{
				SoundManager.Play(result as AudioClip);
			}
		}

		public virtual bool IsVisible()
		{
			return transform.gameObject.activeSelf;
		}

		public virtual void Enable()
		{
			Graphic [] graphics = gameObject.GetComponentsInChildren<Graphic> ();
			foreach (Graphic graphic in graphics)
				graphic.enabled = true;

			Selectable [] selectables = gameObject.GetComponentsInChildren<Selectable> ();
			foreach (Selectable selectable in selectables)
				selectable.interactable = true;
		}

		public virtual void Disable()
		{
			Graphic [] graphics = gameObject.GetComponentsInChildren<Graphic> ();
			foreach (Graphic graphic in graphics)
				graphic.enabled = false;
			
			Selectable [] selectables = gameObject.GetComponentsInChildren<Selectable> ();
			foreach (Selectable selectable in selectables)
				selectable.interactable = false;
		}

		public void SetParent(UI parentUI)
		{
			mUI = parentUI;
		}

		public bool HasName(string itemName)
		{
			return (GetName().Equals (itemName));
		}

		public string GetName()
		{
			return transform.name;
		}

		public void SetName(string itemName)
		{
			transform.name = itemName;
		}

		public void SetPosition(Vector3 pos)
		{
			SetPosition (pos.x, pos.y);
		}

		public void SetPosition(Vector2 pos)
		{
			SetPosition (pos.x, pos.y);
		}

		public void SetPosition(float x, float y)
		{
			RectTransform rectTransform = GetComponent<RectTransform> ();
			Vector3 localPosition = rectTransform.localPosition;
			localPosition.x = x;
			localPosition.y = y;
			rectTransform.localPosition = localPosition;
		}

		public virtual void SetDisabled(bool disabled)
		{

		}

		public void SetScale(Vector2 scale)
		{
			transform.localScale = scale;
		}

		public void SetScale(float x, float y)
		{
			Vector3 scale = transform.localScale;
			scale.x = x;
			scale.y = y;
			transform.localScale = scale;
		}

		public void SetRotation(float eulerAngleZ)
		{
			transform.Rotate (0.0f, 0.0f, eulerAngleZ);
		}

		public void SetRotation(Vector3 eulerAngle)
		{
			transform.Rotate (eulerAngle);
		}

		public void SetItemData(System.Object data)
		{
			mItemData = data;
		}

		public System.Object GetItemData()
		{
			return mItemData;
		}

		public UIItem Clone()
		{
			GameObject clonedGameObject = GameObject.Instantiate (transform.gameObject);
			UIItem newItem = clonedGameObject.GetComponent<UIItem> ();
			return newItem;
		}

		public UIItem Clone(string newItemName)
		{
			UIItem newItem = Clone ();
			newItem.SetName (newItemName);
			return newItem;
		}

		private void OnCullStateChanged(bool culled)
		{
			if(mUI != null)
				mUI.OnCullStateChanged (this, culled);
		}

		public virtual void SetVisibility(bool visibility)
		{
			_Visible = visibility;
		}

		public virtual void SetInteractive(bool interactive)
		{
			_Interactive = interactive;
		}
		
		protected virtual void UpdateVisibility(bool visibility)
		{
			mVisible = visibility;
		}

		protected virtual void UpdateInteractivity(bool interactive)
		{
			mInteractive = interactive;
		}

		public override void Update ()
		{
			base.Update ();

			if(mVisible && !_Visible)
				UpdateVisibility(false);
			else if(!mVisible && _Visible)
				UpdateVisibility(true);
			
			if(mInteractive && !_Interactive)
				UpdateInteractivity(false);
			else if(!mInteractive && _Interactive)
				UpdateInteractivity(true);


		}
	}
}
