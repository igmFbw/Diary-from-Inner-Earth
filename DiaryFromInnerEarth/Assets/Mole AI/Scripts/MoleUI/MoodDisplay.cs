using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 引入TextMeshPro命名空间

namespace Mole_AI.Scripts.MoleUI
{
    public class MoodDisplay : MonoBehaviour
    {
        // 添加TextMeshProUGUI引用（需在Inspector中拖拽赋值）
        [SerializeField] private TextMeshProUGUI moodText;

        /// <summary>
        /// 更新情绪显示（当前值/最大值）
        /// </summary>
        /// <param name="currentValue">当前数值</param>
        /// <param name="maxValue">最大数值</param>
        public void UpdateMoodDisplay(int currentValue, int maxValue)
        {
            if (moodText != null)
            {
                moodText.text = $"{currentValue}/{maxValue}";
            }
            else
            {
                Debug.LogError("TextMeshProUGUI component is not assigned!");
            }
        }
    }
}