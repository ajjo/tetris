using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

namespace UUEX.UI
{
	public class UIBehavior : BaseMonoBehavior, 
							IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, 
							ISelectHandler, IDeselectHandler,
							IDragHandler,IBeginDragHandler, IEndDragHandler,
							IScrollHandler
	{
		public virtual void OnPointerClick(PointerEventData eventData)
		{

		}
		
		public virtual void OnPointerDown(PointerEventData eventData)
		{

		}
		
		public virtual void OnPointerUp(PointerEventData eventData)
		{

		}
		
		public virtual void OnPointerEnter(PointerEventData eventData)
		{

		}
		
		public virtual void OnPointerExit(PointerEventData eventData)
		{

		}
		
		public virtual void OnSelect(BaseEventData eventData)
		{

		}
		
		public virtual void OnDeselect(BaseEventData eventData)
		{

		}

		public virtual void OnScroll(PointerEventData eventData)
		{

		}

		public void OnBeginDrag(PointerEventData eventData)
		{
		}

		public virtual void OnEndDrag(PointerEventData eventData)
		{
		}

		public virtual void OnDrag(PointerEventData data)
		{
		}
	}
}
