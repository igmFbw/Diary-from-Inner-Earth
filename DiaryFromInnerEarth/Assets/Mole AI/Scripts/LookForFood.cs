using System;
using UnityEngine;

namespace Mole_AI.Scripts
{
    public class LookForFood : MonoBehaviour
    {
        [Header("探测设置")]
        public float searchRadius = 5f;         // 探索半径
        public string targetTag = "Food";      // 目标标签

        [Header("移动设置")] 
        public RbController rbController;
        public float moveSpeed = 3.0f;          // 移动速度
        public Transform target;               // 找到的目标
        
        public Action GetFoodAction;
        public bool isHungry;
        
        public void FindTarget()
        {
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, searchRadius);
            foreach (var collider in hitColliders)
            {
                if (collider.CompareTag(targetTag))
                {
                    // print("发现食物");
                    target = collider.transform;
                    return;
                }
            }

            target = null;
        }

        public void MoveTowardsTarget()
        {
            rbController.moveSpeed = 0;
            if (target == null) return;
            var direction = (Vector2)(target.position - transform.position).normalized;
            rbController.XDirection = direction.x > 0 ? 1 : -1;
            rbController.moveSpeed = moveSpeed;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, searchRadius);
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.CompareTag("Square") && isHungry)
            {
                print("挖掉面前的物块");
                Destroy(other.gameObject);
            }
            if(!other.gameObject.CompareTag("Food")) return;
            Destroy(other.gameObject);
            GetFoodAction?.Invoke();
        }
    }
}
