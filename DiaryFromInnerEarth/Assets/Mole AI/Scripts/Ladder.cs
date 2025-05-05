using System;
using UnityEngine;

namespace Mole_AI.Scripts
{
    public class Ladder : MonoBehaviour
    {
        public Transform ladderTop;
        public Transform ladderBottom;
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                print("鼹鼠进入梯子");
                var moleCross = other.gameObject.GetComponent<ObstacleCrossMove>();
                if (Vector3.Distance(ladderTop.position, moleCross.transform.position) <
                    Vector3.Distance(ladderBottom.position, moleCross.transform.position))
                {
                    moleCross.StartClimbing(ladderTop.position, ladderBottom.position);
                }
                else
                {
                    moleCross.StartClimbing(ladderBottom.position, ladderTop.position);
                }
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                print("鼹鼠离开梯子");
                var moleCross = other.gameObject.GetComponent<ObstacleCrossMove>();
                moleCross.EndClimb();
            }
        }
    }
}
