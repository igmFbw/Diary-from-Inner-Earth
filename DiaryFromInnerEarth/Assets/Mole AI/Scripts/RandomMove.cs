using System.Collections;
using UnityEngine;

namespace Mole_AI.Scripts
{
    public class RandomMove : MonoBehaviour
    {
        [Header("移动设置")]
        public float moveSpeed = 2.0f;          // 移动速度
        public float randomMoveRadius = 3f;         // 随机移动半径（用于可视化）

        [Header("随机移动参数")]
        public float minChangeInterval = 1f;    // 方向改变的最短时间间隔
        public float maxChangeInterval = 3f;    // 方向改变的最长时间间隔

        private float xDirection;               // 当前移动方向（-1 或 1）

        private void Start()
        {
            // 初始化随机方向
            xDirection = Random.Range(0, 2) == 0 ? -1 : 1;

            // 启动协程，定期改变方向
            StartCoroutine(ChangeDirection());
        }

        private void FixedUpdate()
        {
            var direction = new Vector2(xDirection, 0);
            transform.Translate(direction * (moveSpeed * Time.deltaTime), Space.World);
        }

        private IEnumerator ChangeDirection()
        {
            while (true)
            {
                // 随机等待一段时间后改变方向
                yield return new WaitForSeconds(Random.Range(minChangeInterval, maxChangeInterval));

                // 随机选择新的方向
                xDirection = Random.Range(0, 2) == 0 ? -1 : 1;
            }
        }

        // 可选：绘制搜索范围
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, randomMoveRadius);
        }
    }
}