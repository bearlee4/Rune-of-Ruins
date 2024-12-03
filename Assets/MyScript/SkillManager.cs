using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public Object Manager;
    public bool Sword_stop;
    InventoryManager InventoryManager;
    Sword Sword;

    // Start is called before the first frame update
    void Start()
    {
        Sword_stop = false;
        InventoryManager = Manager.GetComponent<InventoryManager>();
        Sword = GetComponent<Sword>();
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Backspace))
        //{
        //    StartCoroutine(Sword.spawnSword());
        //}
        
    }

    public void SetWeapon()
    {
        for (int i = 0; i < InventoryManager.Inventory_Item.Count; i++)
        {
            Debug.Log("테스트 1");
            Debug.Log("InventoryManager.Inventory_Item[i].isActivate =" + InventoryManager.Inventory_Item[i].isActivate);
            string itemname = InventoryManager.Inventory_Item[i].m_itemName;
            if (InventoryManager.Inventory_Item[i].isActivate == true)
            {
                Debug.Log("테스트 2");
                if (InventoryManager.Inventory_Item[i].type == "Weapon")
                {
                    
                    switch (itemname)
                    {
                        case "Sword":
                            {
                                if (Sword.sword_obj.activeSelf == false)
                                {
                                    //Sword.sword_obj.SetActive(true);
                                }    
                                break;
                            }
                    }
                }
            }

            else
            {
                switch(itemname)
                {
                    case "Sword":
                        {
                            Sword.sword_obj.SetActive(false);
                            break;
                        }
                }
            }
        }
    }

    public IEnumerator Reset_Coroutine()
    {
        Sword_stop = true;

        yield return new WaitForSeconds(1f);

        Sword_stop = false;

        yield break;
    }
}
