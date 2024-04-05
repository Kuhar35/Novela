using System.Collections;
using UnityEngine;

namespace Assets.Script
{
    public class FightAvtoRun : MonoBehaviour
    {
        [SerializeField] public SpriteAnimation _spriteAnimation;
        public bool onAvto = false;
        public bool pause = true;
        private void OnMouseDown()
        {
           
            if (Input.GetMouseButtonDown(0))
            {
                onAvto = !onAvto;
                        SpriteAnimation AnimationPause = _spriteAnimation.GetComponent<SpriteAnimation>();
                if (pause)
                {
                    AnimationPause!.SetClip("Pause");
                    pause = false;

                }
                else
                {
                    AnimationPause!.SetClip("FlashingIdle");
                    pause = true;
                }
            }
        }
    }
}