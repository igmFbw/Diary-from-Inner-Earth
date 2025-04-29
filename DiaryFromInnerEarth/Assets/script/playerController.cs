using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class playerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        rb.velocity = new Vector2(x*5, y*5);
        if(x!=0||y!=0)
            updateGroundEvent.Trigger();
    }
}
