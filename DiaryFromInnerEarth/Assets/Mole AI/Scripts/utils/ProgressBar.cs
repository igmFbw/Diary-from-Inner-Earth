using UnityEngine;

namespace Mole_AI.Scripts.utils
{
    public class ProgressBar : MonoBehaviour
    {
        public SpriteRenderer spriteRenderer;
        private static readonly int Progress = Shader.PropertyToID("_Progress");
        

        /// <summary>
        /// 设置进度值
        /// </summary>
        /// <param name="value">范围：0 ~ 1</param>
        public void SetProgress(float value)
        {
            var propBlock = new MaterialPropertyBlock();
            spriteRenderer.GetPropertyBlock(propBlock);
            // 修改属性
            propBlock.SetFloat(Progress, value);
            spriteRenderer.SetPropertyBlock(propBlock);
        }
    }
}