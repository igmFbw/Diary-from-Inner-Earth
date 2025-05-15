using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class mouseClick : MonoBehaviour
{//处理卡牌点击事件的类
    private const string targetLayer = "ground";
    private int layerMask;
    private GameObject squareToChange;
    public void setSquare(GameObject go)
    {
        squareToChange = go;
    }
    private void Start()
    {
        layerMask = LayerMask.NameToLayer(targetLayer);
        layerMask = 1 << layerMask;
    }
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, Mathf.Infinity, layerMask);
            if (hit.collider != null)
            {
                int posX = Mathf.RoundToInt(hit.transform.position.x);
                int posY = Mathf.RoundToInt(hit.transform.position.y);
                Vector2Int pos = new Vector2Int(posX, posY);
                groundManage.instance.changeSquare(pos, squareToChange);
                Destroy(gameObject);
            }
        }
    }
}
