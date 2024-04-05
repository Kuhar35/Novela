using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
//using static UnityEditor.Rendering.FilterWindow;

namespace Assets.Script
{
    
    public class SwitchBeetwenPlayers : MonoBehaviour
    {
        [SerializeField] public Image _frame;   
        [SerializeField] public GameObject LoyoutGrop;
        [SerializeField] public float FrameShiftSize=50f;

        [SerializeField] public GameObject Player;
        [SerializeField] public GameObject ObjectDeath;
        [SerializeField] public SpriteRenderer SpriteRend;
        [SerializeField] public SpriteAnimation SpriteAnimation;
       
        [SerializeField] public float SpeedAnimation=10f;
        [SerializeField] public TextMeshProUGUI _editHealth;
        [SerializeField] public TextMeshProUGUI _priority;

        
        private SpriteRenderer SpriteOther;
        private IntarectableWithGame PlayerSwitch;
        private SpriteRenderer SpriteHeroi;
        private SpriteAnimation SetAnimation;

        [HideInInspector] public HealthComponent HPPlayer;
        [HideInInspector] public bool movedFrame = true;
        [HideInInspector] public string GeneralHP;
        [HideInInspector] public bool codeExecuted = false;
        private SwitchBeetwenPlayers[] movedFrameCheck;
        [SerializeField] public GameObject[] objectsAttack;
      //  [SerializeField] public GameObject[] SpriteAnimation;


        [Header("Parametrs Attack")]
        [SerializeField] public int damage;
        [SerializeField] public int damageProcent;

        [SerializeField] public bool ProtectionVSattack = false;
        [SerializeField] public int contrAttack;
        [SerializeField] public int quantityAbility;
        [SerializeField] public SpriteRenderer spriteAbility;

        private void Start()
        {
            if (gameObject.CompareTag("Frame"))
            {
                HPPlayer = GetComponent<HealthComponent>();
                GeneralHP =HPPlayer._health.ToString();
                PlayerSwitch.GeneralHP = GeneralHP;
                _editHealth.text = HPPlayer._health.ToString()+"/" + GeneralHP;               
                 StartCoroutine(ExecuteLate());

                movedFrameCheck = LoyoutGrop.GetComponentsInChildren<SwitchBeetwenPlayers>();
                PlayerSwitch._editHealth = movedFrameCheck[0]._editHealth;
                PlayerSwitch._ObjectDeath = movedFrameCheck[0].ObjectDeath;
                PlayerSwitch._health= movedFrameCheck[0].GetComponent<HealthComponent>();

                SetAnimation._clips = movedFrameCheck[0].SpriteAnimation._clips;
                //////

                //if (movedFrameCheck != null && movedFrameCheck.Length > 0)
                //{
                //    // Проверка, что у нулевого элемента есть корректное значение objectsAttack
                //    if (movedFrameCheck[0] != null && movedFrameCheck[0].objectsAttack != null)
                //    {
                        PlayerSwitch.objectsAttack = movedFrameCheck[0].objectsAttack;
                //    }
                //}
                // SetAnimation._clips = SpriteAnimation!._clips;

                //CHeck this kod

                //get parametr attack
                damage = objectsAttack[0].GetComponent<ActionAttackInfo>().damage;
                damageProcent = objectsAttack[0].GetComponent<ActionAttackInfo>().damageProcent;
                ProtectionVSattack = objectsAttack[0].GetComponent<ActionAttackInfo>().ProtectionVSattack;
                contrAttack = objectsAttack[0].GetComponent<ActionAttackInfo>().contrAttack;
                quantityAbility = objectsAttack[0].GetComponent<ActionAttackInfo>().quantityAbility;
                //
                //set parametr attack in object Heroi_image
                PlayerSwitch.damage= movedFrameCheck[0].damage;
                PlayerSwitch.damageProcent = movedFrameCheck[0].damageProcent;
                PlayerSwitch.ProtectionVSattack = movedFrameCheck[0].ProtectionVSattack;
                PlayerSwitch.contrAttack = movedFrameCheck[0].contrAttack;
                PlayerSwitch.quantityAbility = movedFrameCheck[0].quantityAbility;
                spriteAbility.sprite = objectsAttack[0].GetComponent<SpriteRenderer>().sprite;
                //

                ///Делаем бой

            }
        }
        private void Awake()
        {
            // GameObject.FindGameObjectWithTag("Player").GetComponent<IntarectableWithGame>();
            HPPlayer = GetComponent<HealthComponent>();

            SpriteOther = SpriteRend.GetComponent<SpriteRenderer>();
            PlayerSwitch = Player.GetComponent<IntarectableWithGame>();
            SpriteHeroi = PlayerSwitch.GetComponent<SpriteRenderer>();

            SetAnimation = Player!.GetComponent<SpriteAnimation>();
            SetAnimation._clips = SpriteAnimation!._clips;          

            //SpriteHeroi.sprite = SpriteOther.sprite;

        }
        IEnumerator ExecuteLate()
        {
            yield return new WaitForEndOfFrame(); // Ждем конца текущего кадра

            if (!codeExecuted)
            {
                Image[] images = LoyoutGrop.GetComponentsInChildren<Image>();
                RectTransform rectOther = images[0]!.GetComponent<RectTransform>();
                images[0].rectTransform.anchoredPosition = new Vector2(rectOther.anchoredPosition.x, rectOther.anchoredPosition.y + FrameShiftSize);
       
                movedFrameCheck = LoyoutGrop.GetComponentsInChildren<SwitchBeetwenPlayers>();
                movedFrameCheck[0].movedFrame = false;
                for (int i = 0; i < movedFrameCheck.Length; i++)
                {
                    movedFrameCheck[i].codeExecuted = true;
                }
            }
        }
        private void Update()
        {
           // Debug.Log(this.gameObject.name+" "+ movedFrame);
        }


        //public int indexPlayerWork = 0;
        //public string namePlayerWork;
        private bool isCoroutineRunning = false;
        public void Action()
        {
            if (movedFrame && !isCoroutineRunning)
            {
               // movedFrameCheck = LoyoutGrop.GetComponentsInChildren<SwitchBeetwenPlayers>();
               
                for (int i = 0; i < movedFrameCheck.Length; i++)
                {
                    movedFrameCheck[i].isCoroutineRunning = true;//on corotine
                    BoxCollider2D ColiderFrame = movedFrameCheck[i].GetComponent<BoxCollider2D>();
                    if (ColiderFrame)
                    {
                        // movedFrame = false;
                        movedFrameCheck[i].movedFrame = true;   //was only line                      
                    }
                    else { movedFrame = false; }
                    //Debug.Log(movedFrameCheck[i].movedFrame+" "+i);
                }
                movedFrame = false;// Определяет кто выбран
                                

                //set parametr attack in object Heroi_image switch frame
                PlayerSwitch.damage = damage;
                PlayerSwitch.damageProcent = damageProcent;
                PlayerSwitch.ProtectionVSattack = ProtectionVSattack;
                PlayerSwitch.contrAttack = contrAttack;
                PlayerSwitch.quantityAbility = quantityAbility;
                //еще нужно передать спрайт

                PlayerSwitch._health = GetComponent<HealthComponent>();//set on  heroi_image

                PlayerSwitch._editHealth = _editHealth;//set assigment TextHP //here problem

                PlayerSwitch._ObjectDeath = ObjectDeath;//set assigment Death

                SetAnimation._clips = SpriteAnimation!._clips;//set assigment Animation

                PlayerSwitch.objectsAttack = objectsAttack;//set assigment attack

            //    objectsAttack[0].GetComponent<ActionAttackInfo>().quantityAbility

              //  isCoroutineRunning = true;
                StartCoroutine(MoveImageCoroutine());            
            }

          
            SpriteHeroi.sprite = SpriteOther.sprite;            
        }
        public void OnMouseDown()
        {           
            if (Input.GetMouseButtonDown(0))
            {
                Action();
            }
        }
        private IEnumerator MoveImageCoroutine()
        {           
            RectTransform rect = _frame.GetComponent<RectTransform>();
            Vector2 targetPositionUP = new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + FrameShiftSize);

            float FixPosition = rect.anchoredPosition.y;
  
            Image[] images = LoyoutGrop.GetComponentsInChildren<Image>();         

            for (int i = 0; i < images.Length; i++)
            {
                RectTransform rectOther = images[i]!.GetComponent<RectTransform>();
                Vector2 targetPositionDown = new Vector2(rectOther.anchoredPosition.x, rectOther.anchoredPosition.y - FrameShiftSize);
                Vector2 startPositionOther = rectOther.anchoredPosition;
                float elapsedTime = 0f;
                float animationDuration = 0.5f;

                if (_frame.name == images[i].name)
                {
                    Vector2 startPosition = _frame.rectTransform.anchoredPosition;
                    while (elapsedTime < animationDuration)
                    {
                        float t = Mathf.SmoothStep(0, 1, elapsedTime / animationDuration);
                        _frame.rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPositionUP, t);
                        elapsedTime += Time.deltaTime * 12f;
                        yield return null;
                    }
                    _frame.rectTransform.anchoredPosition = targetPositionUP;
                }
                else {
                    if (FixPosition < startPositionOther.y|| FrameShiftSize<0 && FixPosition > startPositionOther.y)//здесь проблема
                    {
                        while (elapsedTime < animationDuration)
                        {
                            float t = Mathf.SmoothStep(0, 1, elapsedTime / animationDuration);
                            images[i].rectTransform.anchoredPosition = Vector2.Lerp(startPositionOther, targetPositionDown, t);
                            elapsedTime += Time.deltaTime * SpeedAnimation;
                            yield return null;
                        }
                        images[i].rectTransform.anchoredPosition = targetPositionDown;                   
                    }
                }
            }
            foreach (SwitchBeetwenPlayers offCorotine in movedFrameCheck)
            {
                offCorotine.isCoroutineRunning = false;
            }
        }    
    }
}