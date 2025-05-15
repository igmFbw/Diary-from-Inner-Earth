using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum groundType
{
    soil,warter,plant,animal,stone,wood,empty
}
public class groundSquare : MonoBehaviour
{
    public groundType type;
    [SerializeField] private List<GameObject> fallingObjectList;
    public void excavated()//被挖掘后更新地块列表和掉落物
    {
        int length = fallingObjectList.Count;
        int posX = Mathf.RoundToInt(transform.position.x);
        int posY = Mathf.RoundToInt(transform.position.y);
        if (length >= 0)
        {
            int index = Random.Range(0, length);
            Instantiate(fallingObjectList[index], new Vector2(posX, posY), Quaternion.identity);
        }
        groundManage.instance.destroySquare(new Vector2Int(posX,posY));
    }
}

