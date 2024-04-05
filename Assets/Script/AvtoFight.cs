using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class AvtoFight : MonoBehaviour
    {
        public bool onFight=false;

        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(0))
            {
                         
                onFight = !onFight;
            }

        }
    }
}