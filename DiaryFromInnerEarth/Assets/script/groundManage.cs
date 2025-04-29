using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class groundManage : MonoBehaviour
{
    private const int height = 7;
    private const int length = 11;
    private Dictionary<Vector2Int, GameObject> groundDic;//地块
    private Dictionary<Vector2Int, GameObject> cuGroundDic;//当前显示的地块
    private Dictionary<Vector2Int, GameObject> unLoadGroundDic;//将要卸载的地块
    [SerializeField] private Transform playerPos;
    [SerializeField] private GameObject squarePrefab;
    [SerializeField] private Transform gourndParent;
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
                loadGround(new Vector2Int(dx, dy));
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
            {
                unLoadGround(item.Key);
            }
        }
        unLoadGround(new Vector2Int(playerX, playerY));
        foreach(var item in unLoadGroundDic)
            cuGroundDic.Remove(item.Key);
    }
    private void loadGround(Vector2Int pos)
    {
        GameObject squareToLoad;
        if (groundDic.TryGetValue(pos, out squareToLoad))
        {
            if (!cuGroundDic.ContainsKey(pos))
                cuGroundDic.Add(pos, squareToLoad);
            squareToLoad.SetActive(true);
        }
        else
        {
            squareToLoad = Instantiate(squarePrefab, new Vector2(pos.x, pos.y), Quaternion.identity);
            groundDic.Add(pos, squareToLoad);
            cuGroundDic.Add(pos, squareToLoad);
            squareToLoad.transform.SetParent(gourndParent);
        }
    }
    private void unLoadGround(Vector2Int pos)
    {
        GameObject squareToUnLoad;
        if (groundDic.TryGetValue(pos, out squareToUnLoad))
        {
            squareToUnLoad.SetActive(false);
            unLoadGroundDic.Add(pos, squareToUnLoad);
        }
    }
}