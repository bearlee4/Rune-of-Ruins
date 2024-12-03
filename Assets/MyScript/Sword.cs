using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform
    public GameObject sword_obj;

    private SkillManager SkillManager;

    public GameObject Manager;
    InventoryManager InventoryManager;

    public float orbitRadius = 5f; // ���� �˵��� ������
    public float orbitSpeed = 1f; // ȸ�� �ӵ� (���� ����)
    private float angle; // ���� ����

    private float active_count = 5f;
    private float cooldown = 3f;

    private bool istrigger;
    private bool isRotating;  // ȸ�� ���θ� �����ϴ� ����

    // Start is called before the first frame update
    void Start()
    {
        SkillManager = GetComponent<SkillManager>();
        InventoryManager = Manager.GetComponent<InventoryManager>();

        StartCoroutine(spawnSword());
        StartCoroutine(RotaionSword());
        istrigger = false;
        isRotating = false;
        sword_obj.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < InventoryManager.Inventory_Item.Count; i++)
        {
            if (InventoryManager.Inventory_Item[i].m_itemName == "Sword" && InventoryManager.Inventory_Item[i].isActivate == true)
            {
                istrigger = true;
            }

            else
            {
                istrigger= false;
            }
        }
    }

    public IEnumerator spawnSword()
    {
        while (true)
        {
            if(istrigger == true)
            {
                sword_obj.gameObject.SetActive(true);
                isRotating = true;
                Debug.Log("isRotating = " + isRotating);
                yield return new WaitForSeconds(active_count);


                sword_obj.gameObject.SetActive(false);
                isRotating = false;
                Debug.Log("isRotating = " + isRotating);
                
            }
            yield return new WaitForSeconds(cooldown);
        }
    }

    public IEnumerator RotaionSword()
    {
        while(true)
        {
            if(isRotating == true)
            {
                // ������ ȸ�� �ӵ��� ���� ������Ŵ
                angle += orbitSpeed * Time.deltaTime;

                // ���ο� x, y ��ǥ ���
                float x = player.position.x + Mathf.Cos(angle) * orbitRadius;
                float y = player.position.y + Mathf.Sin(angle) * orbitRadius;

                // ������Ʈ ��ġ ����
                sword_obj.transform.position = new Vector3(x, y, player.position.z);

                // �÷��̾�� ������Ʈ ������ ���� ���� ���
                Vector3 direction = sword_obj.transform.position - player.position;

                // z�� ȸ���� ���߱� ���� LookAt�� ���
                float angleToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // ������Ʈ ȸ�� ���� (ȸ�� ���� ����, �عٴ��� �÷��̾ ���ϰ� ȸ��)
                sword_obj.transform.rotation = Quaternion.Euler(0, 0, angleToPlayer + -90f); // 180���� ���ؼ� �عٴ��� �÷��̾ ����Ű���� ��

            }
            yield return null;
        }

    }

}
