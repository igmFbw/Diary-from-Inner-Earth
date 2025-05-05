using UnityEngine;

namespace Mole_AI.Scripts
{
    [CreateAssetMenu(fileName = "NewMole", menuName = "Mole/Mole Data")]
    public class MoleData : ScriptableObject
    {
        public int maxMood = 100;
        public int moodChangeSpeed = 1;
        public int hungerInterval = 5;  // 间隔多少秒产生饥饿
        public int hungerDuration = 5;  // 需要在多少秒内解决饥饿
    }
}
