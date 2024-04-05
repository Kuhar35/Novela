using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class EndFight : MonoBehaviour
    {
        //  private GameObject loos;
       [SerializeField] public SpriteRenderer _win;
        [SerializeField] public SpriteRenderer _loss;
        [SerializeField] public SpriteRenderer _blur;

        public void Winner()
        {
            // SpriteRenderer won = win.GetComponent<SpriteRenderer>();
            _win.enabled = true;
            _blur.enabled = true;
        }
        public void Looser()
        {
            //SpriteRenderer lost = loss.GetComponent<SpriteRenderer>();
            _loss.enabled = true;
            _blur.enabled = true;
        }
    }
}