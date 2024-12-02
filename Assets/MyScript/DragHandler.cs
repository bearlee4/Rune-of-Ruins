using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR;

//아무래도 부모 설정에서 일어나는 UI 버그인거 같음. 예상이 맞다면 정보는 제대로 돌아가고 있을거라 생각.
public class DragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
	Vector3 startPosition;
    private Transform m_startParent;
    private int[] nodes;
    public GameObject NodeManagers;
    NodeManager NodeManager;
    public HandScript m_hand;
    Item Item;
    public int[] nodesize;

    private int backup_node;

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        if (eventData.button.Equals(PointerEventData.InputButton.Left))
        {
            if (m_hand.GrabItemObject == null)
            {//손이 비어있는 상태
                if (clickedObject.tag == "Item" && clickedObject.GetComponent<Item>().CenterNode != -1)
                {//템창 위다
                    Node m_currentNode = NodeManager.Instance.GetNodeByIndex(clickedObject.GetComponent<Item>().CenterNode);
                    backup_node = clickedObject.GetComponent<Item>().CenterNode;
                    if (m_currentNode.ItemObject != null)
                    {//템이 있다

                        startPosition = clickedObject.transform.position;
                        //템은 손으로 이동
                        m_hand.GrabItemObject = m_currentNode.ItemObject;
                        //인벤 노드 초기화
                        NodeManager.Instance.InitialNode(m_currentNode);
                    }
                }
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        GameObject clickedObject = eventData.pointerCurrentRaycast.gameObject;
        if (eventData.button.Equals(PointerEventData.InputButton.Left))
        {
            if (clickedObject != null && clickedObject.tag == "Item")
            {
                if(clickedObject.transform.position == startPosition)
                {
                    clickedObject.GetComponent<Item>().CenterNode = backup_node;
                    if (clickedObject.GetComponent<Item>().CenterNode != -1)
                    {
                        int[] items = NodeManager.Instance.CalItemPosition(backup_node, m_hand.GrabItemObject.m_horizonCount, m_hand.GrabItemObject.m_verticalCount);
                        Node m_currentNode = NodeManager.Instance.GetNodeByIndex(backup_node);
                        NodeManager.Instance.SetItem(items, m_hand.GrabItemObject);
                        m_hand.GrabItemObject = null;
                        backup_node = -1;
                    }
                }
                

            }

        }

    }

    public void OnBeginDrag(PointerEventData eventData)
	{
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;

        m_hand.GrabItemObject = obj.GetComponent<Item>();
        ////장착칸 안일경우
        //if (NodeManager.Instance.m_currentOverIndex >= 0)
        //{
        //    Debug.Log("그랩중");
        //    Node m_currentNode = NodeManager.Instance.GetNodeByIndex(NodeManager.Instance.m_currentOverIndex); ///.GrabItemObject.CenterNode);
        //    nodes = NodeManager.Instance.CalItemPosition(NodeManager.Instance.m_currentOverIndex, m_hand.GrabItemObject.m_horizonCount, m_hand.GrabItemObject.m_verticalCount);
        //    NodeManager.Instance.InitialNode(m_currentNode);
            
        //}
        ////아닐 경우
        //else
        //{
        //    m_startParent = transform.parent;
        //}

        m_startParent = transform.parent;

        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
        this.transform.SetParent(this.transform.parent.root);
	}

	public void OnDrag(PointerEventData eventData)
	{
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        transform.position = eventData.position;

        NodeManager.Instance.ClearNodeColor();

        if (obj != null)
        {

            if (NodeManager.Instance != null)
            {
                Debug.Log(NodeManager.Instance.m_currentOverIndex);
                int overIndex = NodeManager.Instance.m_currentOverIndex;
                if (overIndex >= 0)
                {
                    nodesize = NodeManager.Instance.CalItemPosition(overIndex, this.GetComponent<Item>().m_horizonCount, this.GetComponent<Item>().m_verticalCount);
                    NodeManager.Instance.ClearNodeColor();

                    if (NodeManager.Instance.CheckClashCount(nodesize) == 1)
                    {//템이 있는데 한개다

                        for (int i = 0; i < nodesize.Length; i++)
                        {
                            NodeManager.Instance.ChangeColor(NodeManager.Instance.m_nodeArray[nodesize[i]].Gameobject, NodeManager.Instance.selectColor);
                        }

                        Node tempNode = NodeManager.Instance.CheckClash(nodesize);
                        //NodeManager.Instance.ChangeColor(tempNode.ItemObject.gameObject, NodeManager.Instance.selectColor);
                    }
                    else if (NodeManager.Instance.CheckClashCount(nodesize) >= 2)
                    {
                        for (int i = 0; i < nodesize.Length; i++)
                        {
                            NodeManager.Instance.ChangeColor(NodeManager.Instance.m_nodeArray[nodesize[i]].Gameobject, NodeManager.Instance.cantdropColor);
                        }
                    }
                    else
                    {//템이 없다

                        for (int i = 0; i < nodesize.Length; i++)
                        {
                            NodeManager.Instance.ChangeColor(NodeManager.Instance.m_nodeArray[nodesize[i]].Gameobject, NodeManager.Instance.selectColor);
                        }
                    }
                }
                
            }
        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        NodeManager.Instance.ClearNodeColor();
        int overIndex = NodeManager.Instance.m_currentOverIndex;

        if (overIndex >= 0)
        {
            int[] items = NodeManager.Instance.CalItemPosition(overIndex, m_hand.GrabItemObject.m_horizonCount, m_hand.GrabItemObject.m_verticalCount);

            int clashCount = NodeManager.Instance.CheckClashCount(items);

            Debug.Log("clashCount " + clashCount);

            if (clashCount == 0)
            {
                //m_hand.m_grapItemID = null;
                NodeManager.Instance.SetItem(items, m_hand.GrabItemObject);
                m_hand.GrabItemObject.CenterNode = overIndex;
                m_hand.GrabItemObject = null;
            }
            else if (clashCount == 1)
            {
                //한개겹침
                //잠깐 치워둠
                Item temp = m_hand.GrabItemObject;

                Node tempNode = NodeManager.Instance.CheckClash(items);
                //템을 주움
                tempNode.ItemObject.gameObject.transform.SetParent(m_startParent);
                NodeManager.Instance.InitialNode(tempNode);

                //치워뒀던거 내려둠
                NodeManager.Instance.SetItem(items, temp);
                m_hand.GrabItemObject.CenterNode = overIndex;
                m_hand.GrabItemObject = null;
            }
            else
            {
                if (m_startParent != null && nodes == null)
                {
                    transform.SetParent(m_startParent);
                }

                else if (m_startParent == null && nodes != null)
                {
                    NodeManager.Instance.SetItem(nodes, m_hand.GrabItemObject);
                    m_hand.GrabItemObject.CenterNode = overIndex;
                }

                m_startParent = null;
                nodes = null;
            }

            //for (int i = 0; i < NodeManager.Instance.m_nodeArray.Length; i++)
            //{
            //    NodeManager.Instance.ChangeColor(NodeManager.Instance.GetNodeByIndex(i).Gameobject, NodeManager.Instance.defaultColor);
            //}

            //NodeManager.Instance.SetItem(nodesize, this.GetComponent<Item>());
            //Debug.Log("드래그 끝날시 셋 아이템 작동");
        }

        else
        {
            if (eventData.pointerEnter != null && eventData.pointerEnter.GetComponent<Drop>() != null)
            {
                transform.SetParent(eventData.pointerEnter.transform);
            }
            else
            {
                transform.SetParent(m_startParent);
            }
        }

        m_hand.GrabItemObject = null;
        m_startParent = null;
    }

}
