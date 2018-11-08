using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public Transform parentToReturnTo = null;
    public Transform initialParent = null;
    public Transform placeholderParent = null;
    public int initialSiblingIndex;
    Canvas myCanvas;
    GameObject placeholder = null;

    public void OnBeginDrag(PointerEventData eventData)
    {
        
        placeholder = new GameObject();
        placeholder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeholder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        placeholder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
        initialSiblingIndex = this.transform.GetSiblingIndex();

        parentToReturnTo = this.transform.parent;
        placeholderParent = parentToReturnTo;
        initialParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {


        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        this.transform.position = Input.mousePosition;//= new Vector2(ray.origin.x, ray.origin.y);
        
        if (!((placeholderParent.childCount >= 2) && placeholderParent.name.StartsWith("Goalkeeper")))

        {
            placeholder.transform.SetParent(placeholderParent);


            //if (placeholderParent.name.StartsWith("Goalkeeper"))
            //{
            //    if (placeholderParent.childCount < 1)
            //    {
            int newSiblingIndex = placeholderParent.childCount;

            for (int i = 0; i < placeholderParent.childCount; i++)
            {
                if (this.transform.position.x < placeholderParent.GetChild(i).position.x)
                {

                    newSiblingIndex = i;

                    if (placeholder.transform.GetSiblingIndex() < newSiblingIndex)
                        newSiblingIndex--;

                    break;
                }
            }

            placeholder.transform.SetSiblingIndex(newSiblingIndex);
        }
    }
      
    

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!((placeholderParent.childCount >= 2) && placeholderParent.name.StartsWith("Goalkeeper")))
              
        {
            this.transform.SetParent(parentToReturnTo);

            this.transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        }
        else
        {
            this.transform.SetParent(initialParent);

            this.transform.SetSiblingIndex(initialSiblingIndex);
        }

        GetComponent<CanvasGroup>().blocksRaycasts = true;
        //Debug.Log(placeholderParent.childCount);
        Destroy(placeholder);
    }



}
