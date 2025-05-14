using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Mole_AI.Scripts
{
    public class RandomMove : MonoBehaviour
    {

        [Header("移动设置")] 
        public RbController rbController;
        public float moveSpeed = 2.0f;          // 移动速度
        public float randomMoveRadius = 3f;         // 随机移动半径（用于可视化）

        [Header("随机移动参数")]
        public float minChangeInterval = 1f;    // 方向改变的最短时间间隔
        public float maxChangeInterval = 3f;    // 方向改变的最长时间间隔

        [Header("处理水平随机移动的时候的越障问题")] public ObstacleDetection obstacleDetection;



        private void Start()
        {
            // 启动协程，定期改变方向
            StartCoroutine(ChangeDirection());
            obstacleDetection.OnObstacleDetected += OnObstacleDetected;
        }

        private void OnObstacleDetected(int value)
        {
            switch (value)
            {
                case 0:
                    // 越过障碍物
                    rbController.Jump();
                    break;
                case 1:
                    rbController.TurnBack();
                    break;
            }
        }

        private void OnEnable()
        {
            rbController.moveSpeed = moveSpeed;
            obstacleDetection.enabled = true;
        }

        private void OnDisable()
        {
            obstacleDetection.enabled = false;
        }

        private void FixedUpdate()
        {
            obstacleDetection.facingDirection = rbController.XDirection;
        }

        private IEnumerator ChangeDirection()
        {
            while (true)
            {
                // 随机等待一段时间后改变方向
                yield return new WaitForSeconds(Random.Range(minChangeInterval, maxChangeInterval));

                // 随机选择新的方向
                rbController.XDirection = Random.Range(0, 2) == 0 ? -1 : 1;
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