using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class item : MonoBehaviour
{
    public string itemName;
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "player")
        {
            Destroy(gameObject);
        }
    }
}
