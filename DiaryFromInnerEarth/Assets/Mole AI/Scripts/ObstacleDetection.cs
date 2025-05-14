using System;
using UnityEngine;

// 此脚本的主要功能
// 1. 检测前方是否有障碍物
// 2. 检测前方的障碍物是否可以跨越{0-不可跨越；1-可以跨越；}
namespace Mole_AI.Scripts
{
    public class ObstacleDetection : MonoBehaviour
    {
        // 面朝向
        public int facingDirection = 1;
        [Header("地面检测")]
        public Transform groundCheckPoint; // 角色脚底的位置
        public LayerMask groundLayer;      // 地面所在的层
        public float checkDistance = 0.1f; // 射线长度
        private bool isGrounded;
        [Header("障碍检测")]
        public Transform detectionPoint; // 射线起点（如角色脚部）
        public float detectionDistance = 0.5f; // 探测距离
        public LayerMask obstacleLayer; // 障碍物所在的层
        private bool isNearObstacle;
        [Header("是否可以跃过")]
        public Transform maxJumpHeightPoint; // 跳跃点
        
        public Action<int> OnObstacleDetected;
        
        private void Update()
        {
            // 检测是否在地面上
            CheckGround();
            // 检测前方是否有障碍物
            DetectObstacleHeight();
            if (isGrounded && isNearObstacle && CanJumpOver())
            {
                // 在地面上且前方有障碍物时，跳跃
                print("可以越过眼前的障碍物");
                OnObstacleDetected?.Invoke(0);
            }
            else if (isGrounded && isNearObstacle && !CanJumpOver())
            {
                // 在地面上且前方有障碍物时，跳跃
                print("不可以越过眼前的障碍物");
                OnObstacleDetected?.Invoke(1);
            }
        }
        
        private void CheckGround()
        {
            var hit = Physics2D.Raycast(groundCheckPoint.position, Vector2.down, checkDistance, groundLayer);

            isGrounded = hit.collider != null;

            // 可视化调试
            Debug.DrawRay(groundCheckPoint.position, Vector3.down * checkDistance, isGrounded ? Color.green : Color.red);
        }
        
        private void DetectObstacleHeight()
        {
            var direction = new Vector2(facingDirection, 0);
            var hit = Physics2D.Raycast(detectionPoint.position, direction, detectionDistance, obstacleLayer);
            isNearObstacle = hit.collider != null;
            // 可视化调试
            Debug.DrawRay(detectionPoint.position, direction * detectionDistance, isNearObstacle ? Color.green : Color.red);
        }
        
        private bool CanJumpOver()
        {
            var direction = new Vector2(facingDirection, 0);
            var hit = Physics2D.Raycast(maxJumpHeightPoint.position, direction, detectionDistance, obstacleLayer);
            // 可视化调试
            var canJump = hit.collider == null;
            Debug.DrawRay(maxJumpHeightPoint.position, direction * detectionDistance, canJump ? Color.green : Color.red);
            return canJump;
        }
    }
}
