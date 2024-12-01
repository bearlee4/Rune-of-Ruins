using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drop : MonoBehaviour, IDropHandler
{
    public GameObject NodeManagers;
    NodeManager NodeManager;

    public void Start()
    {
        NodeManager = NodeManagers.GetComponent<NodeManager>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        //GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        //if (eventData.pointerDrag != null)
        //{

        //}
    }
}
