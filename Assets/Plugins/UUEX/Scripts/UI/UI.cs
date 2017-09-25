using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

// Update enabled, visibility at run time
namespace UUEX.UI
{
	[RequireComponent(typeof(Canvas))]
	public class UI : UIBase
	{
		public bool _Visible = true;
		public bool _Interactive = true;

		protected Canvas mCanvas;
		protected List<UIItem> mItems;
		protected UnityAction<UIItem> onItemClickEvent = null;
		protected UnityAction onClickEvent = null;
		protected UnityAction<UIItem, bool> onCullStateListener = null;

		protected bool mInteractive = true;
		protected bool mVisible = true;

		private static UI mCurrentExclusiveUI = null;

		public override void Awake()
		{
			base.Awake();

			mCanvas = GetComponentInChildren<Canvas> ();

			Init (transform);
		}

		protected virtual void Init(Transform gameObjectTransform)
		{
			mItems = new List<UIItem> ();

			foreach (Transform child in gameObjectTransform) 
			{
				UIItem item = child.GetComponent<UIItem> ();
				mItems.Add (item);

				item.SetParent (this);
			}

			if (!_Visible)
				SetVisibility (false);
			if (!_Interactive)
				SetInteractive (false);
		}

		public List<UIItem> GetItems()
		{
			return mItems;
		}

		public void SetPriority(int priority)
		{
			mCanvas.sortingOrder = priority;
		}

		public int GetPriority()
		{
			return mCanvas.sortingOrder;
		}

		public void SetExclusive(Color exclusiveColor)
		{
			if (mCurrentExclusiveUI != null)
				mCurrentExclusiveUI.RemoveExclusive ();

			Image image = mCanvas.gameObject.GetComponent<Image> ();
			if(image == null)
				image = mCanvas.gameObject.AddComponent<Image> ();

			image.color = exclusiveColor;
			mCurrentExclusiveUI = this;
		}

		public void SetExclusive(float alpha = 0.3f)
		{
			if (mCurrentExclusiveUI != null)
				mCurrentExclusiveUI.RemoveExclusive ();

			Image image = mCanvas.gameObject.GetComponent<Image> ();
			if(image == null)
				image = mCanvas.gameObject.AddComponent<Image> ();

			Color exclusiveColor = image.color;
			exclusiveColor.a = alpha;
			image.color = exclusiveColor;

			mCurrentExclusiveUI = this;
		}

		public void RemoveExclusive()
		{
			Image image = mCanvas.gameObject.GetComponent<Image> ();
			if (image != null)
				Destroy (image);
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
			Transform parentTransform = transform.parent;
			Vector3 localPosition = parentTransform.localPosition;
			localPosition.x = x;
			localPosition.y = y;
			parentTransform.localPosition = localPosition;
		}

		public override void OnPointerClick(PointerEventData eventData)
		{
			if(eventData != null)
			{
				if(EventSystem.current.currentSelectedGameObject == null)
					OnClick();
			}		
		}

		public override void OnDrag (PointerEventData data)
		{
			Debug.Log ("Coming dragging");
		}

		public override void OnScroll (PointerEventData eventData)
		{
			Debug.Log ("Scrolling");
		}

		// When the UI is clicked
		public virtual void OnClick()
		{
			if (onClickEvent != null)
				onClickEvent ();
		}

		// When the item is clicked
		public virtual void OnItemClick(UIItem item)
		{
			if (onItemClickEvent != null)
				onItemClickEvent (item);
		}

		public void AddItemClickListener(UnityAction<UIItem> action)
		{
			onItemClickEvent += action;
		}

		public void RemoveItemClickListener(UnityAction<UIItem> action)
		{
			onItemClickEvent -= action;
		}

		public void AddClickListener(UnityAction action)
		{
			onClickEvent += action;
		}
		
		public void RemoveItemClickListener(UnityAction action)
		{
			onClickEvent -= action;
		}

		public virtual void SetVisibility(bool visibility)
		{
			_Visible = visibility;
		}

		protected virtual void UpdateVisibility(bool visibility)
		{
			mCanvas.enabled = visibility;
			mVisible = visibility;
		}

		public virtual void SetInteractive(bool interactive)
		{
			_Interactive = interactive;
		}

		protected virtual void UpdateInteractivity(bool interactive)
		{
			foreach(UIItem item in mItems)
				item.SetInteractive(interactive);
			
			mInteractive = interactive;
		}

		public virtual void Enable()
		{

		}

		public virtual void Disable()
		{

		}

		public virtual int GetItemCount()
		{
			return mItems.Count;
		}
		
		public bool IsVisible()
		{
			return mCanvas.enabled;
		}

		public virtual UIItem AddItem(string duplicateItemName, string itemName, Transform parentTransform)
		{
			UIItem item = GetItem (duplicateItemName);
			UIItem newItem = item.Clone ();
			newItem.SetName (itemName);
			newItem.SetParent (this);
			newItem.transform.SetParent(parentTransform);
			return AddItem (newItem);
		}

		public virtual UIItem AddItem(UIItem item, Transform parentTransform = null)
		{
			mItems.Add (item);

			if(parentTransform != null)
			{
				item.transform.SetParent(parentTransform);
				item.SetParent(this);
			}

			return item;
		}

		public T GetItem<T>(string itemName)
		{
			foreach (UIItem item in mItems) 
			{
				if(item.HasName(itemName))
					return (T)Convert.ChangeType(item, typeof(T));
			}
			
			return default(T);
		}


		public UIItem GetItem(string itemName)
		{
			foreach (UIItem item in mItems) 
			{
				if(item.HasName(itemName))
					return item;
			}

			return null;
		}

		public UIItem GetItemAt(int index)
		{
			return mItems [index];
		}

		public void RemoveItem(string itemName)
		{
			UIItem item = GetItem (itemName);
			RemoveItem (item);
		}

		public void RemoveItem(UIItem item)
		{
			mItems.Remove (item);
			GameObject.Destroy (item.gameObject);
		}

		public void RemoveItemAt(int index)
		{
			UIItem item = GetItemAt (index);
			RemoveItem (item);
		}

		public void SetItem(UIItem item, int index)
		{
			mItems.RemoveAt (index);
			mItems.Insert (index, item);
		}

		public void Clear()
		{
			foreach(UIItem item in mItems)
				GameObject.Destroy(item.gameObject);

			mItems.Clear ();
		}

		public override void Update()
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
		
		public void AddCullStateListener(UnityAction<UIItem,bool> action)
		{
			onCullStateListener += action;
		}
		
		public void RemoveCullStateListener(UnityAction<UIItem,bool> action)
		{
			onCullStateListener -= action;
		}

		public virtual void OnCullStateChanged(UIItem item, bool culled)
		{
			if (onCullStateListener != null)
				onCullStateListener (item, culled);
		}
	}
}
