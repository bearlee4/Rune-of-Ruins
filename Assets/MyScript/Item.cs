using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour {

	public string m_itemName;
	public int m_horizonCount;
	public int m_verticalCount;
    public int CenterNode = -1;


	//업그레이드 단계
	public int upgrade;
	//아이템 중복 갯수
	public int get_count;
    //작동 여부
    public bool isActivate;

    //버프 종류
    public List<string> buff;
    //아이템 종류
    public string type;
}
