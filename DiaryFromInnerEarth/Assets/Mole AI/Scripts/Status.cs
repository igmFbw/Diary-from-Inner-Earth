using System;
using Mole_AI.Scripts.utils;
using UnityEngine;
using UnityEngine.Events;

namespace Mole_AI.Scripts
{
    public enum Demand
    {
        Food,
        Null,
        Explore,
        Rest
    }
    
    public class Status : MonoBehaviour
    {
        public MoleData moleData;
        public float currentMood;
        public GameObject demandToken;
        public ProgressBar progressBar;
        
        [Header("心情值更新")]
        public UnityEvent<int, int> moodChangedEvent;
        [Header("动作控制")]
        public RbController rbController;
        [Header("随机移动状态")] public RandomMove randomMove;
        
        [Header("饥饿状态")]
        public LookForFood lookForFood;
        private CustomTimer hungerIntervalTimer;
        private CustomTimer hungerDurationTimer;
        private Demand currentDemand;
        
        private void Start()
        {
            currentDemand = Demand.Null;
            currentMood = moleData.maxMood;
            // 等时间到出现饥饿状态
            hungerIntervalTimer = new(moleData.hungerInterval);
            hungerDurationTimer = new(moleData.hungerDuration);
            hungerIntervalTimer.Start();
            // 找到食物是否开吃
            lookForFood.GetFoodAction = GetFood;
        }

        // Update is called once per frame
        private void Update()
        {
            randomMove.enabled = currentDemand == Demand.Null;
            // 处理饥饿间隔定时器的更新
            if (hungerIntervalTimer.IsRunning)
            {
                hungerIntervalTimer.Update(Time.deltaTime);
            }
            if (hungerIntervalTimer.IsFinished)
            {
                hungerIntervalTimer.Reset();
                NeedFood();
            }
            if (!hungerIntervalTimer.IsRunning && currentDemand != Demand.Food)
            {
                print("重新开始计时饥饿间隔");
                hungerIntervalTimer.Start();
            }
            // 在食物需求状态会优先寻找附近的食物
            if (currentDemand == Demand.Food)
            {
                lookForFood.isHungry = true;
                if (lookForFood.target == null)
                {
                    lookForFood.FindTarget();
                }
                else
                {
                    lookForFood.MoveTowardsTarget();
                }
            }
            else
            {
                lookForFood.isHungry = false;
            }
            // 饥饿状态需要在一定时间内被满足
            if (hungerDurationTimer.IsRunning)
            {
                hungerDurationTimer.Update(Time.deltaTime);
                progressBar.SetProgress(hungerDurationTimer.Progress);
            }
            else if (currentDemand == Demand.Food)
            {
                progressBar.SetProgress(1);
                print("没能够及时吃到东西，心情变坏了");
                rbController.moveSpeed = 0;
                currentMood -= Time.deltaTime * moleData.moodChangeSpeed;
            }
            else
            {
                currentMood += Time.deltaTime * moleData.moodChangeSpeed;
            }
            
            currentMood = Mathf.Clamp(currentMood, 0, moleData.maxMood);
            // 最后更新心情值
            moodChangedEvent.Invoke((int)currentMood, moleData.maxMood);
        }
        
        
        # region 食物需求相关
        private void NeedFood()
        {
            currentDemand = Demand.Food;
            print("needFood");
            demandToken.SetActive(true);
            hungerDurationTimer.Start();
        }

        private void GetFood()
        {
            if (currentDemand == Demand.Food)
            {
                print("吃到食物了");
                currentDemand = Demand.Null;
                demandToken.SetActive(false);
                hungerDurationTimer.Reset();
            }
        }
        # endregion
    }
}
