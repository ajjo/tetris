using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace UUEX.UI
{
	public class UIMenu : UI
	{
		protected ScrollRect mScrollRect = null;
		protected LayoutGroup mLayoutGroup = null;
		private int mCurrentPage = 0;

		public override void Awake()
		{
			transform = GetComponent<Transform> ();
			mCanvas = GetComponent<Canvas> ();
			mScrollRect = mCanvas.GetComponentInChildren<ScrollRect> ();
			mLayoutGroup = mScrollRect.viewport.GetComponentInChildren<LayoutGroup> ();

			Init (mLayoutGroup.transform);

			mScrollRect.onValueChanged.AddListener (ScrollRectChanged);
		}

		public void AddValueChangedListener(UnityAction<Vector2> action)
		{
			mScrollRect.onValueChanged.AddListener (action);
		}

		public void RemoveValueChangedListener(UnityAction<Vector2> action)
		{
			mScrollRect.onValueChanged.RemoveListener (action);
		}

		public virtual void ScrollRectChanged(Vector2 value)
		{
			int pageCount = GetPageCount ();
			mCurrentPage = 1 + (pageCount - Mathf.CeilToInt (value.y * pageCount));
		}

		public int GetCurrentPage()
		{
			return mCurrentPage;
		}

		public ScrollRect GetScrollRect()
		{
			return mScrollRect;
		}

		public void SetSpacing(float spacing)
		{
			if (mLayoutGroup is HorizontalLayoutGroup) 
			{
				HorizontalLayoutGroup layoutGroup = (HorizontalLayoutGroup)mLayoutGroup;
				layoutGroup.spacing = spacing;
			}
			else if (mLayoutGroup is VerticalLayoutGroup) 
			{
				VerticalLayoutGroup layoutGroup = (VerticalLayoutGroup)mLayoutGroup;
				layoutGroup.spacing = spacing;
			}
		}

		public void SetSpacing(float rowSpacing, float colSpacing)
		{
			if (mLayoutGroup is GridLayoutGroup) 
			{
				GridLayoutGroup layoutGroup = (GridLayoutGroup)mLayoutGroup;
				Vector2 spacing = new Vector2(rowSpacing, colSpacing);
				layoutGroup.spacing = spacing;
			}
		}

		public void SetPadding(int left, int right, int top, int bottom)
		{
			mLayoutGroup.padding.left = left;
			mLayoutGroup.padding.right = right;
			mLayoutGroup.padding.top = top;
			mLayoutGroup.padding.bottom = bottom;
		}

		public void SetCellSize(int width, int height)
		{
			if(mLayoutGroup is VerticalLayoutGroup || mLayoutGroup is HorizontalLayoutGroup)
			{
				foreach (UIItem item in mItems) 
				{
					LayoutElement layoutElement = item.GetComponent<LayoutElement>();
					layoutElement.preferredWidth = width;
					layoutElement.preferredHeight = height;
				}
			}
			else if(mLayoutGroup is GridLayoutGroup)
			{
				GridLayoutGroup layoutGroup = (GridLayoutGroup)mLayoutGroup;
				Vector2 newSize = new Vector2(width, height);
				layoutGroup.cellSize = newSize;
			}
		}

		public void SetRowCount(int rowCount)
		{
			GridLayoutGroup layoutGroup = (GridLayoutGroup)mLayoutGroup;
			layoutGroup.constraint = GridLayoutGroup.Constraint.FixedRowCount;
			layoutGroup.constraintCount = rowCount;
		}

		public int GetRowCount()
		{
			if(mLayoutGroup is GridLayoutGroup)
			{
				GridLayoutGroup layoutGroup = (GridLayoutGroup)mLayoutGroup;

				if (layoutGroup.constraint == GridLayoutGroup.Constraint.FixedRowCount)
					return layoutGroup.constraintCount;
				else
				{
					int colCount = GetColumnCount();
					return Mathf.CeilToInt((float)mItems.Count / colCount);
				}
			}
			else
				return mItems.Count;
		}

		public int GetColumnCount()
		{
			if(mLayoutGroup is GridLayoutGroup)
			{
				GridLayoutGroup layoutGroup = (GridLayoutGroup)mLayoutGroup;

				if (layoutGroup.constraint == GridLayoutGroup.Constraint.FixedColumnCount)
					return layoutGroup.constraintCount;
				else
				{
					int rowCount = GetRowCount();
					return Mathf.CeilToInt((float)mItems.Count / rowCount);
				}
			}
			else
				return mItems.Count;
		}

		public int GetPageCount()
		{
			// Use the scroll rect to determine how many cells can fit this page
			RectMask2D rectMask2D = mScrollRect.viewport.GetComponent<RectMask2D>();
			float maskWidth = rectMask2D.canvasRect.width;
			float maskHeight = rectMask2D.canvasRect.height;

			if(mLayoutGroup is GridLayoutGroup)
			{
				GridLayoutGroup gridLayout = (GridLayoutGroup)mLayoutGroup;
				int numColsInPage = Mathf.FloorToInt(maskWidth / (gridLayout.cellSize.x + gridLayout.spacing.x));
				int numRowsInPage = Mathf.FloorToInt(maskHeight / (gridLayout.cellSize.y + gridLayout.spacing.y));

				int numItemsPerPage = numColsInPage * numRowsInPage;
				return Mathf.CeilToInt((float)mItems.Count / numItemsPerPage);
			}
			else
			{
				float itemWidth = 0.0f;
				float itemHeight = 0.0f;

				foreach(UIItem item in mItems)
				{
					RectTransform rectTransform = item.GetComponent<RectTransform>();

					if(mLayoutGroup is VerticalLayoutGroup)
						itemHeight += rectTransform.rect.height;
					else
						itemWidth += rectTransform.rect.width;
				}

				if(mLayoutGroup is VerticalLayoutGroup)
				{
					if(itemHeight > maskHeight)
						return Mathf.CeilToInt(itemHeight / maskHeight);
					else
						return 1;
				}
				else if(mLayoutGroup is HorizontalLayoutGroup)
				{
					if(itemWidth > maskWidth)
						return Mathf.CeilToInt(itemWidth / maskWidth);
					else
						return 1;
				}
			}

			return 0;
		}

		public void GotoPage(int pageNumber)
		{
			Scrollbar scrollBar = null;

			if (mScrollRect.horizontal)
				scrollBar = mScrollRect.horizontalScrollbar;
			else
				scrollBar = mScrollRect.verticalScrollbar;

			if(scrollBar != null)
			{
				int pageCount = GetPageCount ();
				scrollBar.value = (float)pageNumber / pageCount;
			}

			mCurrentPage = pageNumber;
		}

		public List<UIItem> GetItemsInPage(int pageNumber)
		{
			return new List<UIItem> ();
		}

		public void SetColumnCount(int colCount)
		{
			GridLayoutGroup layoutGroup = (GridLayoutGroup)mLayoutGroup;
			layoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
			layoutGroup.constraintCount = colCount;
		}

		public override void OnCullStateChanged (UIItem item, bool culled)
		{
			base.OnCullStateChanged (item, culled);

			// Load item on demand
			if(!culled)
				LoadItem (item);
		}

		public virtual void LoadItem(UIItem item)
		{
			// Do something here
		}

		public virtual void UnloadItem(UIItem item)
		{
			// Do something here
		}

		public void SetBackgroundImage(Texture2D backgroundImage)
		{
			Image menuBackgroundImage = mScrollRect.GetComponent<Image> ();
			menuBackgroundImage.sprite = Sprite.Create(backgroundImage, new Rect(0,0,backgroundImage.width,backgroundImage.height), new Vector2(0.5f,0.5f));
		}
	}
}
