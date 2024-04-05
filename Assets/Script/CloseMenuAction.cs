using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    
    public class CloseMenuAction : MonoBehaviour
    {
        [SerializeField] public MenuAction MenuAction;

        private void OnMouseDown()
        {                              
            if (Input.GetMouseButtonDown(0) && MenuAction.MenuActive)
            {
                MenuAction.CloseMenu();
            }
        }
    }
}