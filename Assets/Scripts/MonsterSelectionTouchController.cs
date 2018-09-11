using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class MonsterSelectionTouchController : MonoBehaviour
, IBeginDragHandler, IDragHandler, IEndDragHandler
, IPointerClickHandler
{
    public delegate void onDelegate();
    public onDelegate onBeginDrag;
    public onDelegate onDrag;
    public onDelegate onEndDrag;
    public onDelegate onPointerClick;


    void OnEnable()
    {
        transform.localScale = new Vector3(1, 1, 1);
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (onBeginDrag != null)
        {
            onBeginDrag();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (onDrag != null)
        {
            onDrag();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (onEndDrag != null)
        {
            onEndDrag();
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (onPointerClick != null)
        {
            onPointerClick();
        }
    }
}
