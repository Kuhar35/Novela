using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Script
{
    public class ActionAttackInfo : MonoBehaviour
    {
        [SerializeField] public int damage;
        [SerializeField] public int damageProcent;

        [SerializeField] public bool ProtectionVSattack = false;
        [SerializeField] public int contrAttack;
        [SerializeField] public int quantityAbility;
        [SerializeField] public GameObject LoyoutGroupHeroi;
        [SerializeField] public UnityEvent CloseMenu;
        
        //[SerializeField] public SwitchBeetwenPlayers LoyoutGroupEnemy;
        private SpriteRenderer SetSpriteAttack;
        private GameObject AbilitySprite;
        private void Start()
        {
            
        }

        private void OnMouseDown()
        {
            SwitchBeetwenPlayers[] movedFrameCheck = LoyoutGroupHeroi.GetComponentsInChildren<SwitchBeetwenPlayers>();
            IntarectableWithGame ParametrAttack = GameObject.FindGameObjectWithTag("Player").GetComponent<IntarectableWithGame>();

            if (Input.GetMouseButtonDown(0)&& gameObject.CompareTag("Gun") == true)
            {
                for (int i = 0; i < movedFrameCheck.Length; i++)
                {
                    if (movedFrameCheck[i].movedFrame != true)
                    {
                        AbilitySprite = movedFrameCheck[i]!.transform.Find("ability").gameObject;  //get frame attack                                      
                        SetSpriteAttack = AbilitySprite.GetComponent<SpriteRenderer>();                        
                        SetSpriteAttack.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;                       
                        //SetSpriteAttack.material.shader = Shader.Find("Sprites/Default");//Затемнение
                        //SetSpriteAttack.color = Color.grey;
                        CloseMenu.Invoke();
                        //Set parametr in GroupLayout
                        movedFrameCheck[i].damage = damage;
                        movedFrameCheck[i].damageProcent = damageProcent;
                        movedFrameCheck[i].ProtectionVSattack = ProtectionVSattack;
                        movedFrameCheck[i].contrAttack = contrAttack;
                        movedFrameCheck[i].quantityAbility = quantityAbility;
                        //
                        //Set parametr in Image_Player
                        ParametrAttack.damage = damage;
                        ParametrAttack.damageProcent = damageProcent;
                        ParametrAttack.ProtectionVSattack = ProtectionVSattack;
                        ParametrAttack.contrAttack = contrAttack;
                        ParametrAttack.quantityAbility = quantityAbility;
                        //

                        StartCoroutine(AnimationIcon(SetSpriteAttack));
                                            
                    }
                }
            }         
        }
        private IEnumerator AnimationIcon(SpriteRenderer AbilitySprite)
        {
            Vector3 beginSizeIcon = AbilitySprite.transform.localScale;
            AbilitySprite.transform.localScale += new Vector3(70f,70f,0);

            while (AbilitySprite.transform.localScale.x >= beginSizeIcon.x) 
            {
                AbilitySprite.transform.localScale -= new Vector3(5f, 5f, 0);
                yield return new WaitForSeconds(0.05f);
            }
            AbilitySprite.transform.localScale=beginSizeIcon;
        }
    }
}