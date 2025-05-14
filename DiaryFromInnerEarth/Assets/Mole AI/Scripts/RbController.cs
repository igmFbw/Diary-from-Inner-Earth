using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mole_AI.Scripts
{
    public class RbController : MonoBehaviour
    {
        private Rigidbody2D rb;
        
        [Header("移动设置")]
        public float moveSpeed = 2.0f;          // 移动速度
        public float jumpForce = 2.5f;      // 跳跃力度
        
        public int XDirection
        {
            get =>
                // 根据当前缩放返回方向
                transform.localScale.x > 0 ? 1 : -1;
            set
            {
                // 确保值只能是 1 或 -1
                var direction = value >= 0 ? 1 : -1;
                transform.localScale = new Vector3(direction, 1, 1);
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            // 初始化随机方向
            XDirection = Random.Range(0, 2) == 0 ? -1 : 1;
        }

        public void Jump()
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }

        public void TurnBack()
        {
            XDirection = - 1 * XDirection;
        }

        public void FixedUpdate()
        {
            // 使用 Rigidbody2D 控制移动
            var velocity = rb.velocity;
            velocity.x = XDirection * moveSpeed; // 设置水平速度
            rb.velocity = velocity;
        }
    }
}
