using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform
    public GameObject sword_obj;

    private SkillManager SkillManager;

    public GameObject Manager;
    InventoryManager InventoryManager;

    public float orbitRadius = 5f; // 원형 궤도의 반지름
    public float orbitSpeed = 1f; // 회전 속도 (라디안 단위)
    private float angle; // 현재 각도

    private float active_count = 5f;
    private float cooldown = 3f;

    private bool istrigger;
    private bool isRotating;  // 회전 여부를 제어하는 변수

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
                // 각도를 회전 속도에 맞춰 증가시킴
                angle += orbitSpeed * Time.deltaTime;

                // 새로운 x, y 좌표 계산
                float x = player.position.x + Mathf.Cos(angle) * orbitRadius;
                float y = player.position.y + Mathf.Sin(angle) * orbitRadius;

                // 오브젝트 위치 갱신
                sword_obj.transform.position = new Vector3(x, y, player.position.z);

                // 플레이어와 오브젝트 사이의 방향 벡터 계산
                Vector3 direction = sword_obj.transform.position - player.position;

                // z축 회전을 맞추기 위해 LookAt을 사용
                float angleToPlayer = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                // 오브젝트 회전 설정 (회전 각도 적용, 밑바닥이 플레이어를 향하게 회전)
                sword_obj.transform.rotation = Quaternion.Euler(0, 0, angleToPlayer + -90f); // 180도를 더해서 밑바닥이 플레이어를 가리키도록 함

            }
            yield return null;
        }

    }

}
