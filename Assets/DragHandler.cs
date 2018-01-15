using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    public static GameObject itemBeingDragged;
    Vector3 startPosition;
    Transform startParent;

    #region IBeginDragHandler implementation

    public void OnBeginDrag(PointerEventData eventData)
    {
        itemBeingDragged = gameObject;
        startPosition = transform.position;
        startParent = transform.parent;
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    #endregion

    #region IDragHandler implementation

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    #endregion

    #region IEndDragHandler implementation

    public void OnEndDrag(PointerEventData eventData)
    {
        if (gameObject.tag == "flowerOwn") {
            RaycastHit hitInfo;
            if(ReturnClickedObject(out hitInfo).name == "collectBag"){
                float value = new PlantBuilder(gameObject.transform.GetChild(0).GetComponent<Image>().sprite.name).SetUniqueValues().SetCommonValues().Build().GetValue();
                //Debug.Log(value);
                string[] p = gameObject.name.Split('_');
                int y;
                int.TryParse(p[1], out y);
                int x;
                int.TryParse(p[2], out x);
                int[] index = {y,x};
                GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().AddFunds(100);
                GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().RemoveItem(index);
                gameObject.tag = "";

            }
        }
        itemBeingDragged = null;
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        if (transform.parent == startParent)
        {
            transform.position = startPosition;
        }
    }

    #endregion

    GameObject ReturnClickedObject(out RaycastHit hit)
    {
        GameObject target = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray.origin, ray.direction * 10, out hit))
        {
            target = hit.collider.gameObject;
        }
        return target;
    }
}
