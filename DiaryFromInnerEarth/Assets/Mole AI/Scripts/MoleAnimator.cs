using System;
using UnityEngine;
using UnityEngine.Events;

namespace Mole_AI.Scripts
{
    using UnityEngine;

    public class MoleAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator moleAnimationPlayer;

        public UnityEvent digDone;

        // Animator 参数名
        private static readonly int Walking = Animator.StringToHash("walking");
        private static readonly int Climb = Animator.StringToHash("climbing");
        private static readonly int Dig = Animator.StringToHash("dig");
        private static readonly int ScratchHead = Animator.StringToHash("scratchHead");

        // 概率与冷却时间
        [SerializeField] private float scratchHeadProbability = 0.01f; // 每帧触发概率，建议 0~1 之间较小的值
        [SerializeField] private float scratchHeadCooldown = 10f; // 冷却时间，防止频繁触发

        private float lastScratchHeadTime;
        private bool isIdleState;

        private void Start()
        {
            lastScratchHeadTime = -scratchHeadCooldown; // 初始化为负数以便第一次可以触发
        }

        private void Update()
        {
            CheckCurrentState();

            if (isIdleState && Time.time > lastScratchHeadTime + scratchHeadCooldown)
            {
                if (Random.value < scratchHeadProbability)
                {
                    PlayScratchHead();
                    lastScratchHeadTime = Time.time;
                }
            }
        }

        private void CheckCurrentState()
        {
            // 假设 Idle 是默认状态，即所有 Bool 都为 false 时才是 Idle
            isIdleState = !moleAnimationPlayer.GetBool(Walking) &&
                          !moleAnimationPlayer.GetBool(Climb) &&
                          !moleAnimationPlayer.GetBool(Dig);
        }

        public void PlayWaking()
        {
            SetAllAnimationsToFalse();
            moleAnimationPlayer.SetBool(Walking, true);
        }

        public void PlayClimbing()
        {
            SetAllAnimationsToFalse();
            moleAnimationPlayer.SetBool(Climb, true);
        }

        public void PlayDigging()
        {
            SetAllAnimationsToFalse();
            moleAnimationPlayer.SetTrigger(Dig);
        }

        public void TriggerDigDone()
        {
            digDone?.Invoke();
        }

        public void PlayIdle()
        {
            SetAllAnimationsToFalse();
        }

        public void PlayScratchHead()
        {
            moleAnimationPlayer.SetTrigger(ScratchHead);
        }

        private void SetAllAnimationsToFalse()
        {
            moleAnimationPlayer.SetBool(Walking, false);
            moleAnimationPlayer.SetBool(Climb, false);
        }
    }
}
