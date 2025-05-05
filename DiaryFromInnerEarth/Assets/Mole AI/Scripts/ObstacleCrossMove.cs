using UnityEngine;

namespace Mole_AI.Scripts
{
    public class ObstacleCrossMove : MonoBehaviour
    {
        public float moveSpeed = 5f; // 移动速度
        private Rigidbody2D rb;
        private float originalGravityScale;
        private float movementInput;
        // 面朝向
        private int facingDirection = 1; // 1 表示向右，-1 表示向左
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
        public float jumpForce=10f; // 跳跃力度
        
        // 处理爬梯子
        [Header("爬梯子")]
        public float climbSpeed = 3.0f;
        private Vector3 startPoint;
        private Vector3 endPoint;
        public bool isClimbing;
        

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            originalGravityScale = rb.gravityScale;
        }

        private void Update()
        {
            // 更新朝向
            UpdateFacingDirection();
            // 获取水平输入（A/D 或 左/右箭头）
            movementInput = Input.GetAxisRaw("Horizontal");
            // 检测是否在地面上
            CheckGround();
            // 检测前方是否有障碍物
            DetectObstacleHeight();
            if (isGrounded && isNearObstacle && CanJumpOver() && !isClimbing)
            {
                // 在地面上且前方有障碍物时，跳跃
                print("可以越过眼前的障碍物");
                rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
        
        private void UpdateFacingDirection()
        {
            var velocityX = rb.velocity.x;

            facingDirection = velocityX switch
            {
                > 0f => 1,
                < 0f => -1,
                _ => facingDirection
            };
            // 翻转物体的 X 缩放
            transform.localScale = new Vector3(facingDirection, 1, 1);
        }

        private void FixedUpdate()
        {
            if (isClimbing)
            {
                ClimbTowardsEndPoint();
            }
            else
            {
                // 正常移动
                rb.velocity = new Vector2(movementInput * moveSpeed, rb.velocity.y);
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
        
        # region 处理爬梯子的动作
        /// <summary>
        /// 爬梯子的行为，首先水平移动到起点，然后垂直方向移动到终点
        /// </summary>
        private void ClimbTowardsEndPoint()
        {
            // 1. 先水平移动到起点的 X 坐标
            if (Mathf.Abs(transform.position.x-startPoint.x) > 0.05f)
            {
                var directionX = Mathf.Sign(startPoint.x - transform.position.x); // 获取方向：+1 或 -1
                rb.velocity = new Vector2(directionX * moveSpeed, 0f); // 水平移动，Y 不动
                return; // 还没到位，返回等待下一次 FixedUpdate
            }

            // 2. 再垂直移动到终点的 Y 坐标
            if (Mathf.Abs(transform.position.y-endPoint.y) > 0.05f)
            {
                var directionY = Mathf.Sign(endPoint.y - transform.position.y); // 获取方向：+1 或 -1
                rb.velocity = new Vector2(0f, directionY * climbSpeed); // 垂直移动，X 不动
                return; // 还没到位，返回等待下一次 FixedUpdate
            }

            // 3. 到达终点，停止移动并退出攀爬状态
            rb.velocity = Vector2.zero;
            isClimbing = false;

            if (endPoint.y < startPoint.y)
            {
                rb.gravityScale = originalGravityScale;
            }
            
            Debug.Log("已到达终点！");
        }

        // 示例：触发攀爬的方法
        public void StartClimbing(Vector3 start, Vector3 end)
        {
            startPoint = start;
            endPoint = end;
            isClimbing = true;
            rb.gravityScale = 0;
        }

        public void EndClimb()
        {
            rb.gravityScale = originalGravityScale;
        }
        # endregion
    }
}
