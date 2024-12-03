using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject SkillManagers;
    SkillManager SkillManager;

    public GameObject Inventory;
    public GameObject Pause;

    private bool ispause;

    // Start is called before the first frame update
    void Start()
    {
        SkillManager = SkillManagers.GetComponent<SkillManager>();

        ispause = false;

        if(Inventory.activeSelf == true)
        {
            Close_Inventory();
        }

        if(Pause.activeSelf == true)
        {
            Close_Pause();
        }
    }

    // Update is called once per frame
    void Update()
    {
        //R키. 인벤토리 여는 키.
        if (Input.GetKeyDown(KeyCode.R) && ispause == false)
        {
            if(Inventory.activeSelf == false)
            {
                Open_Inventory();
            }

            else
            {
                Close_Inventory();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (ispause == false && Time.timeScale != 0)
            {
                Open_Pause();
                ispause = true;
            }

            else if (ispause == true && Time.timeScale == 0)
            {
                Close_Pause();
                ispause = false;
            }

            else if (ispause == false && Inventory.activeSelf == true)
            {
                Close_Inventory();
            }
        }
    }

    public void Open_Inventory()
    {
        Inventory.SetActive(true);
        Time.timeScale = 0;
    }

    public void Close_Inventory()
    {
        Inventory.SetActive(false);
        SkillManager.SetWeapon();
        Time.timeScale = 1;
    }

    public void Open_Pause()
    {
        Pause.SetActive(true);
        Time.timeScale = 0;
    }

    public void Close_Pause()
    {
        Pause.SetActive(false);
        Time.timeScale = 1;
    }
}
