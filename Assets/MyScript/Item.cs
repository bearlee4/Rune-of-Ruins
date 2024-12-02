using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	public string m_itemName;
	public int m_horizonCount;
	public int m_verticalCount;
    public int CenterNode = -1;

	//아이템 종류
	public string type;
	[HideInInspector]
	//아이템 기본 대미지
	public int damage;
	//단계 및 버프 포함 최종 대미지
	public int total_damage;
	//버프 종류
	public string buff;
	//업그레이드 단계
	public int upgrade;
	//아이템 중복 갯수
	public int get_count;
    //작동 여부
    public bool isActivate;
}
