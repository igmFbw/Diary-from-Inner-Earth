using System;
using UnityEngine;

namespace Mole_AI.Scripts.utils
{
    public class CustomTimer
    {
        private float _duration; // 定时器总时长
        public float Duration { get; private set; }
        private float _elapsedTime; // 已过去的时间
        public bool IsRunning { get; private set; }

        public float Progress => Mathf.Clamp01(_elapsedTime / _duration); // 当前进度（0 到 1）
        public bool IsFinished => _elapsedTime >= _duration; // 定时器是否已完成

        public CustomTimer(float duration)
        {
            _duration = duration;
            Duration = duration;
            _elapsedTime = 0f;
            IsRunning = false;
        }

        // 启动或重启定时器
        public void Start()
        {
            _elapsedTime = 0f;
            IsRunning = true;
        }
        
        // 启动或重启定时器（传入新的时长）
        public void Start(float newDuration)
        {
            if (newDuration <= 0f)
            {
                Debug.LogWarning("CustomTimer: 新的计时时长必须大于 0。");
                return;
            }

            _duration = newDuration; // 更新当前定时器时长
            Duration = newDuration; // 同步只读属性
            _elapsedTime = 0f; // 重置已过去的时间
            IsRunning = true; // 开始运行
        }

        // 停止定时器
        public void Stop()
        {
            IsRunning = false;
        }

        public void Reset()
        {
            _elapsedTime = 0f;
            IsRunning = false;
        }

        // 更新定时器
        public void Update(float deltaTime)
        {
            if (!IsRunning) return;
            _elapsedTime += deltaTime;
            if (!(_elapsedTime >= _duration)) return;
            _elapsedTime = _duration;
            IsRunning = false;
        }
    }
}