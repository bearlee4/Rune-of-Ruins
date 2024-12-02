using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    //아이템 오브젝트 담가둘곳
    public GameObject[] items;

    public GameObject InventoryWindow;
    public GameObject EquipWindow;

    [HideInInspector]
    public List<Item> Inventory_Item;

    private int upgrade_count = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            GetItem(items[0].GetComponent<Item>());
        }

        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            GetItem(items[1].GetComponent<Item>());
        }

        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            GetItem(items[2].GetComponent<Item>());
        }

        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            GetItem(items[3].GetComponent<Item>());
        }
    }

    public void GetItem(Item item)
    {
        //같은게 없을때
        if(!Inventory_Item.Contains(item))
        {
            Inventory_Item.Add(item);
            for (int k = 0; k < Inventory_Item.Count; k++)
            {
                if(item.m_itemName == Inventory_Item[k].m_itemName)
                {
                    Inventory_Item[k].get_count = 1;
                    Inventory_Item[k].upgrade = 1;
                    Inventory_Item[k].isActivate = false;
                    break;
                }
            }

            for (int i = 0; i < items.Length; i++)
            {
                if (item.m_itemName == items[i].GetComponent<Item>().m_itemName)
                {
                    GameObject obj = Instantiate(items[i], InventoryWindow.transform);
                    obj.name = items[i].name;
                }
            }
            
        }

        //같은게 있을때
        else if (Inventory_Item.Contains(item))
        {
            for(int i = 0; i < Inventory_Item.Count; i++)
            {
                if(item.m_itemName == Inventory_Item[i].m_itemName)
                {
                    GameObject obj = Instantiate(Inventory_Item[i].gameObject, InventoryWindow.transform);
                    obj.name = Inventory_Item[i].gameObject.name;
                    Inventory_Item[i].get_count += 1;
                    Debug.Log(Inventory_Item[i].get_count);
                    //업그레이드 할만큼 갯수가 충족되었을 때
                    if (Inventory_Item[i].get_count == upgrade_count && Inventory_Item[i].upgrade == 1)
                    {
                        Debug.Log("업그레이드 충족");
                        //장착하든 안하든 인벤토리에서만 2개 지우기
                        List<GameObject> list = new List<GameObject>();
                        int stop_count = 0;
                        for (int n = 0; n < InventoryWindow.transform.childCount; n++)
                        { 
                            if (obj.name == InventoryWindow.transform.GetChild(n).gameObject.name)
                            {
                                list.Add(InventoryWindow.transform.GetChild(n).gameObject);
                                stop_count++;

                                if(stop_count == 2)
                                {
                                    break;
                                }
                            }
                            
                        }

                        for (int k = 0; k < list.Count; k++)
                        {
                            Destroy(list[k]);
                        }

                        UpGrade_Item(Inventory_Item[i]);
                    }

                    break;
                }
            }
        }
        
    }

    public void UpGrade_Item(Item item)
    {
        item.upgrade ++;
        item.total_damage += item.damage;
        item.get_count = 1;
    }
}
