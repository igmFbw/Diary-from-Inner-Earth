using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class groundManage : MonoBehaviour
{
    private const int height = 7;
    private const int length = 11;
    private Dictionary<Vector2Int, GameObject> groundDic;//地块
    private Dictionary<Vector2Int, GameObject> cuGroundDic;//当前显示的地块
    private Dictionary<Vector2Int, GameObject> unLoadGroundDic;//将要卸载的地块
    [SerializeField] private Transform playerPos;
    #region 地块种类
    [SerializeField] private GameObject squarePrefab;
    private GameObject squarePrefab1
    {
        get
        {
            int index1 = UnityEngine.Random.Range(1, 144);
            int index2 = UnityEngine.Random.Range(1, 3);
            if (index1 >= 1 && index1 < 7)
                return animalSquare;
            else if (index1 >= 7 && index1 < 14)
                return waterSquare;
            else if (index1 >= 14 && index1 < 21)
                return plantSquare;
            else if (index1 >= 21 && index1 < 25)
                return stoneSquare;
            else if (index1 >= 25 && index1 < 29)
                return woodSquare;
            else
            {
                if(index2 == 1)
                {
                    return soilSquare1;
                }
                else if (index2 == 2) 
                {
                    return soilSquare2;
                }
                else
                {
                    return soilSquare3;
                }
            }
        }
    }
    [SerializeField] private GameObject soilSquare1;
    [SerializeField] private GameObject soilSquare2;
    [SerializeField] private GameObject soilSquare3;
    [SerializeField] private GameObject waterSquare;
    [SerializeField] private GameObject plantSquare;
    [SerializeField] private GameObject animalSquare;
    [SerializeField] private GameObject stoneSquare;
    [SerializeField] private GameObject woodSquare;
    [SerializeField] private Transform groundParent;
    #endregion
    private void Awake()
    {
        groundDic = new Dictionary<Vector2Int, GameObject>();
        cuGroundDic = new Dictionary<Vector2Int, GameObject>();
        unLoadGroundDic = new Dictionary<Vector2Int, GameObject>();
        updateGroundEvent.Register(updateGround);
    }
    private void OnDestroy()
    {
        updateGroundEvent.UnRegister(updateGround);
    }
    private void updateGround()
    {
        int playerX = Convert.ToInt32(playerPos.position.x);
        int playerY = Convert.ToInt32(playerPos.position.y);
        for (int i = -length; i <= length; i++)
        {
            for (int j = -height; j <= height; j++)
            {
                if (i == 0 && j == 0)
                    continue;
                int dx = playerX + i;
                int dy = playerY + j;
                loadGround(new Vector2Int(dx, dy));//加载视野里的地块
            }
        }
        unLoadGroundDic.Clear();
        foreach(var item in cuGroundDic)
        {
            float maxX = playerPos.position.x + length;
            float minX = playerPos.position.x - length;
            float maxY = playerPos.position.y + height;
            float minY = playerPos.position.y - height;
            if (item.Key.x > maxX || item.Key.x < minX || item.Key.y > maxY || item.Key.y < minY)
                unLoadGround(item.Key);//将移出视野的地块卸载
        }
        unLoadGround(new Vector2Int(playerX, playerY));
        foreach(var item in unLoadGroundDic)
            cuGroundDic.Remove(item.Key);//清空当前地块列表
    }
    private void loadGround(Vector2Int pos)
    {
        GameObject squareToLoad;
        if (groundDic.TryGetValue(pos, out squareToLoad))
        {
            if (!cuGroundDic.ContainsKey(pos))
                cuGroundDic.Add(pos, squareToLoad);
            if(squarePrefab!=null)
                squareToLoad.SetActive(true);
        }
        else
        {
            squareToLoad = Instantiate(squarePrefab, new Vector2(pos.x, pos.y), Quaternion.identity);
            groundDic.Add(pos, squareToLoad);
            cuGroundDic.Add(pos, squareToLoad);
            squareToLoad.transform.SetParent(groundParent);
        }
    }
    private void unLoadGround(Vector2Int pos)
    {
        GameObject squareToUnLoad;
        if (groundDic.TryGetValue(pos, out squareToUnLoad))
        {
            if(squareToUnLoad!=null)
                squareToUnLoad.SetActive(false);
            unLoadGroundDic.Add(pos, squareToUnLoad);
        }
    }
}