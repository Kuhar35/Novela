using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.DebugUI;

namespace Assets.Script
{


    public class IntarectableWithGame : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] public TextMeshProUGUI _editHealth;
        [SerializeField] public TextMeshProUGUI _damageHealth;
        [SerializeField] public HealthComponent _health;
        [SerializeField] public GameObject _ObjectDeath;
        [SerializeField] public GameObject _LayoutGroupHeroi;
        [SerializeField] public GameObject _LayoutGroupEnemy;

        [SerializeField] public UnityEvent _statusTheEnd;


        [HideInInspector] public string GeneralHP;
        [HideInInspector] private bool RoundStep = true;
        private SpriteRenderer _spriteRenderer;

        private SpriteRenderer RenderEnemy;
        private HealthComponent HPEnemy;
        private string GeneralEnemyHP;
        private BoxCollider2D ColiderpEnemy;
        private IntarectableWithGame stepPC;
        private GameObject Enemy;

        private SpriteRenderer RenderPlayer;
        private HealthComponent HPplayer;
        private string GeneralHlayerHP;

        //  private HealthComponent HPenemy;
        private BoxCollider2D ColiderPlayer;
        private IntarectableWithGame stepPlayer;
        private GameObject player;
        private SwitchBeetwenPlayers[] groupEnemies;
        private SwitchBeetwenPlayers[] groupPlayers;
        private bool died = true;
        [SerializeField] public float SpeedAlfaHide = 0.7f;
        [SerializeField] public float SpeedUpMoovDamage = 7f;

        [SerializeField] public MenuAction MenuAction;
        [SerializeField] public GameObject[] objectsAttack;
        [SerializeField] public GameObject[] Sprite;

        SpriteAnimation AnimationPlayer;
        SpriteAnimation AnimationEnemy;
        private int damageEnemy;
        private int damageEnemyProcent;

      //  [Space(10)]
        [Header("Parametrs Attack")]
        [SerializeField] public int damage;
        [SerializeField] public int damageProcent;

        [SerializeField] public bool ProtectionVSattack = false;
        [SerializeField] public int contrAttack;
        [SerializeField] public int quantityAbility;
        Dictionary<int, SwitchBeetwenPlayers> ListPlayerDistribution;//List sequencz turn
        private int _counterAlivePlayer;




        private void Awake()
        {  
            _damageHealth.color = new Color(_damageHealth.color.r, _damageHealth.color.g, _damageHealth.color.b, 0f);
             //player move
            Enemy = GameObject.FindGameObjectWithTag("Enemy");
            RenderEnemy = Enemy!.GetComponent<SpriteRenderer>();
            HPEnemy = Enemy!.GetComponent<HealthComponent>();//здесь ошибка
            ColiderpEnemy = Enemy!.GetComponent<BoxCollider2D>();
            stepPC = Enemy!.GetComponent<IntarectableWithGame>();
          //  HPplayer = stepPC._health;

            //player PC
            player = GameObject.FindGameObjectWithTag("Player");
            RenderPlayer = player!.GetComponent<SpriteRenderer>();
            ColiderPlayer = player!.GetComponent<BoxCollider2D>();
            stepPlayer = player!.GetComponent<IntarectableWithGame>();
            HPplayer = player!.GetComponent<HealthComponent>();

            groupEnemies = _LayoutGroupEnemy!.GetComponentsInChildren<SwitchBeetwenPlayers>();
            groupPlayers = _LayoutGroupHeroi!.GetComponentsInChildren<SwitchBeetwenPlayers>();

            _counterAlivePlayer= groupEnemies.Length+groupPlayers.Length;
       
           
        }
        private void Start()
        {
            GeneralHP = _health._health.ToString();

            if (gameObject.CompareTag("Enemy"))// this kod is bad if show error, you can use Player, because we have two object method Start()
            {
                DistributionOfGameMoves();
                DetermineWhoWalksText();
            }
            //CheckAlivePlayer();


        }

        private void OnMouseDown()
        {
            if (Input.GetMouseButtonDown(0) && gameObject.CompareTag("Enemy") == true && RoundStep && stepPlayer.died && stepPC.died)
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);
                if (hit.collider != null && hit.collider.CompareTag(_tag) && died)
                {
                    StartCoroutine(Run());
                }
            }
           
            //////////////////////////////////////////////////////////////////////
            if (Input.GetMouseButtonDown(0) && gameObject.CompareTag("Player") == true)//menu action
            {
             
                BoxCollider2D ImageHeroi = this.gameObject.GetComponent<BoxCollider2D>();
                ImageHeroi.enabled = false;
               // GameObject menuAction = GetComponent<GameObject>();
                ///
                MenuAction.objectsList.Clear();
                for (int i = 0; i < objectsAttack.Length; i++)
                {
                    //ActionAttackInfo parametrsAttack = objectsAttack[2].GetComponent<ActionAttackInfo>();
                    //stepPC.damageEnemy = parametrsAttack.damage;///this delete==----- damage
                    //HealthComponent[] movedFrameCheck = _LayoutGroupEnemy.GetComponentsInChildren<HealthComponent>();
                    //movedFrameCheck[0]._PrecentageChangeDamage = parametrsAttack.damageProcent;//это оставить 0 индекс нужен нам
                    //stepPC.damageEnemyProcent = parametrsAttack.damageProcent;
                    //HPEnemy._PrecentageChangeDamage = parametrsAttack.damageProcent;

                    if (objectsAttack[i] != null)
                    {
                        objectsAttack[i].GetComponent<SpriteRenderer>().enabled = true;
                        objectsAttack[i].GetComponent<CircleCollider2D>().enabled = true;
                      //  objectsAttack[i].GetComponent<MenuAction>().HeroiImage = menuAction;
                        MenuAction.objectsList.Add(objectsAttack[i]);
                    }                    
                }

                MenuAction.PlaceObjectsInCircle(MenuAction.objectsList.Count);   
               
            }
            /////////////////////////////////////////////////////////////////////////////
        }
        public void WriteHPtext() //this is kod is ready
        {
                    
            foreach (SwitchBeetwenPlayers CheckEnemyHP in groupEnemies)
            {
                if (!CheckEnemyHP!.movedFrame)
                {
                    HPEnemy = CheckEnemyHP.HPPlayer;
                    GeneralEnemyHP = CheckEnemyHP.GeneralHP;//
                }               
            }  
            foreach (SwitchBeetwenPlayers CheckEnemPlayerHP in groupPlayers)
            {
                if (!CheckEnemPlayerHP!.movedFrame)
                {
                    HPplayer = CheckEnemPlayerHP.HPPlayer;
                    GeneralHlayerHP = CheckEnemPlayerHP.GeneralHP;//
                }
            } 
        }


        private void Update()
        {
            //Avto player move
            //bool avtoFight = FindObjectOfType<AvtoFight>().onFight;

            //if (avtoFight && RoundStep && gameObject.CompareTag("Enemy") == true && stepPlayer.died && stepPC.died)
            //{          фи      
            //    StartCoroutine(Run());
            //}

            bool onAvto = FindObjectOfType<FightAvtoRun>().onAvto;

            if (onAvto && gameObject.CompareTag("Enemy"))
            {
                StartCoroutine(stepPC.Fight());
                FindObjectOfType<FightAvtoRun>().onAvto = false;

                _pauseStepPlayer = !_pauseStepPlayer;///здесь проблема
              
            }

           

        }
        public IEnumerator Run()
        {
            // damage;         ready
            // damageProcent;  ready
            // contrAttack;это можно убрать
            //ProtectionVSattack=false;  ready
            //quantityAbility это делаем еще и контр отаку делаем


            //  stepPlayer stepPC

            //Нужно разбить Player and Enemy
          //  Fight();
            RoundStep = false;
            WriteHPtext();      
            //Делаем бой
            if (HPplayer._health > 0 && ColiderpEnemy.enabled )
            { 
                AnimationPlayer = player.GetComponent<SpriteAnimation>();
                AnimationPlayer.SetClip("Attack");//Animation player attack vs enemy

                if (!stepPC.ProtectionVSattack)
                {
                    stepPlayer.quantityAbility--;//дописать если меньше 0
                    yield return new WaitForSeconds(1f);
                    HPEnemy.ApplyDamage(-stepPlayer.damage, stepPlayer.damageProcent);
                    yield return StartCoroutine(stepPC.DamageAnimation(groupEnemies, GeneralEnemyHP, HPEnemy, RenderEnemy, ColiderpEnemy, Enemy.transform));
                }
                else
                {
                    yield return new WaitForSeconds(2f);
                    Debug.Log("animation protection Enemy");
                    //Here paste animation protection Enemy 
                }
            }
       
            //Step Enemy
            if (HPEnemy._health > 0 && ColiderPlayer.enabled)
            {
                stepPC.quantityAbility--;
                AnimationEnemy = Enemy.GetComponent<SpriteAnimation>();
                AnimationEnemy.SetClip("Attack");//Animation enemy attack vs player

                if (!stepPlayer.ProtectionVSattack)
                {
                    yield return new WaitForSeconds(1f);
                    HPplayer.ApplyDamage(-stepPC.damage, stepPC.damageProcent);
                    yield return StartCoroutine(stepPlayer.DamageAnimation(groupPlayers, GeneralHlayerHP, HPplayer, RenderPlayer, ColiderPlayer, player.transform));
                }
                else
                {
                    Debug.Log("animation protection Player");
                //Here paste animation protection Player 
                }
            }
            RoundStep = true;
        }
      

        private void  DetermineWhoWalksText()
        {          
            for (int i = 0; i < ListPlayerDistribution.Count; i++)
            {
                if (ListPlayerDistribution[i].GetComponent<HealthComponent>()._health > 0)
                {
                       if (!listEnemy.ContainsKey(i))
                    {
                        groupPlayers[listPlayer[i]]._priority.text = (1 + ListPlayerDistribution.FirstOrDefault(x => x.Value == groupPlayers[listPlayer[i]]).Key).ToString();
                    }
                    else
                    {
                        groupEnemies[listEnemy[i]]._priority.text = (1 + ListPlayerDistribution.FirstOrDefault(x => x.Value == groupEnemies[listEnemy[i]]).Key).ToString();
                    }
                }
            }           
        }
        private int _indexStepPause = 0;
        private bool _pauseStepPlayer = true;
        public IEnumerator Fight()
        {
            bool tagPlayer = false;
            bool tagEnemy = false;
            for (int i = _indexStepPause; i < ListPlayerDistribution.Count; i++)
            {

                HealthComponent HP = ListPlayerDistribution[i].GetComponent<HealthComponent>();
                if (HP._health >= 0)
                {
                    if (!listEnemy.ContainsKey(i))//define who walk
                    {
                        tagPlayer = true;

                    }
                    else if (!listPlayer.ContainsKey(i))
                    {
                        tagEnemy = true;
                    }
                    ListPlayerDistribution[i].Action();
                    RoundStep = false;
                    WriteHPtext();
                    StartCoroutine(AnimationPriorityText(ListPlayerDistribution[i]._priority));

                    ListPlayerDistribution[i].objectsAttack[0].GetComponent<ActionAttackInfo>().quantityAbility = 0;

                    if (tagPlayer)
                    {
                        yield return StartCoroutine(StepPlayer());
                    }
                    else
                    if (tagEnemy)
                    {
                        yield return StartCoroutine(StepPC());
                    }
                    RoundStep = true;

                    tagPlayer = false;
                    tagEnemy = false;
                }
                yield return new WaitForSeconds(0.1f);

                if (_pauseStepPlayer) //
                {
                    _indexStepPause = i + 1;///work
                    _counterAlivePlayer++;
                    StopAllCoroutines();
                    yield break;

                }
                else
                {
                    if (ListPlayerDistribution.Count == _indexStepPause + 1) _indexStepPause = 0;
                }
            }

            CheckAlivePlayer();
            DistributionOfGameMoves();
            DetermineWhoWalksText();
            _pauseStepPlayer = true;
            OnAnimationIconAvto();


            //for (int i = 0; i < groupEnemies.Length; i++)
            //{
            //    groupEnemies[i].objectsAttack[1].GetComponent<ActionAttackInfo>().quantityAbility = 0;//в этой точке нельзя менять данные 
            //}
        }
        private void OnAnimationIconAvto()
        {
            FightAvtoRun onAvto = FindObjectOfType<FightAvtoRun>();
            SpriteAnimation PauseOn = onAvto._spriteAnimation.GetComponent<SpriteAnimation>();
            PauseOn.SetClip("FlashingIdle");
            onAvto.pause = true;

        }
        private void CheckAlivePlayer() 
        {
            _counterAlivePlayer = 0;
            for (int i = 0; i < ListPlayerDistribution.Count; i++)
            {     
                HealthComponent counterHP = ListPlayerDistribution[i].GetComponent<HealthComponent>();
                    if (counterHP._health >= 0)
                    {
                        ListPlayerDistribution[i]._priority.GetComponent<TextMeshProUGUI>().enabled = true;
                        _counterAlivePlayer++;
                    }
                    else
                    {
                        ListPlayerDistribution[i]._priority.GetComponent<TextMeshProUGUI>().enabled = false;

                    }
                
            }
        }
        private IEnumerator AnimationPriorityText(TextMeshProUGUI animationText)
        {
            Color ColorBasic = animationText.color;
            float textPriority = animationText.fontSize;
            animationText.fontSize = animationText.fontSize + 40;
            yield return animationText.color = Color.red;

            while (animationText.fontSize  >= textPriority)
            {
                animationText.fontSize -= 2;
                yield return new WaitForSeconds(0.05f);
            }
            animationText.fontSize = textPriority;
            yield return animationText.color = ColorBasic;

        }
        private IEnumerator StepPlayer()
        {
            if (HPplayer._health > 0 && ColiderpEnemy.enabled)
            {
                AnimationPlayer = player!.GetComponent<SpriteAnimation>();
                AnimationPlayer!.SetClip("Attack");//Animation player attack vs enemy

                if (!stepPC.ProtectionVSattack)
                {


                    //groupPlayers[0].quantityAbility--;
                    //groupPlayers

                    //    objectsAttack=





                    stepPlayer.quantityAbility--;//дописать если меньше 0
                    yield return new WaitForSeconds(1f);
                    HPEnemy.ApplyDamage(-stepPlayer.damage, stepPlayer.damageProcent);
                    yield return StartCoroutine(stepPC.DamageAnimation(groupEnemies, GeneralEnemyHP, HPEnemy, RenderEnemy, ColiderpEnemy, Enemy.transform));
                }
                else
                {
                    yield return new WaitForSeconds(2f);
                    Debug.Log("animation protection Enemy");
                    //Here paste animation protection Enemy 
                }
                if (stepPlayer.quantityAbility <= 0)
                {
                    //groupPlayers[0].Action();
                    //groupPlayers[0].objectsAttack[0].GetComponent<ActionAttackInfo>().quantityAbility = 0;
                }

                groupPlayers[0].objectsAttack[0].GetComponent<ActionAttackInfo>().quantityAbility = 0;
            }

        }
        private IEnumerator StepPC()
        {          
                if (HPEnemy._health > 0 && ColiderPlayer.enabled)
                {
                    stepPC.quantityAbility--;
                    AnimationEnemy = Enemy!.GetComponent<SpriteAnimation>();
                    AnimationEnemy!.SetClip("Attack");//Animation enemy attack vs player

                    if (!stepPlayer.ProtectionVSattack)
                    {
                        yield return new WaitForSeconds(1f);
                        HPplayer.ApplyDamage(-stepPC.damage, stepPC.damageProcent);
                        yield return StartCoroutine(stepPlayer.DamageAnimation(groupPlayers, GeneralHlayerHP, HPplayer, RenderPlayer, ColiderPlayer, player.transform));
                    }
                    else
                    {
                        Debug.Log("animation protection Player");
                        //Here paste animation protection Player 
                    }
                }
        }

        Dictionary<int,int> listEnemy;
        Dictionary<int,int> listPlayer;
   

        private void DistributionOfGameMoves()
        {
            
            System.Random random = new System.Random();
            int key;
            List<int> usedKeys = new List<int>();
            ListPlayerDistribution = new Dictionary<int, SwitchBeetwenPlayers>();
            listEnemy = new Dictionary<int, int>();
            listPlayer = new Dictionary<int, int>();
            for (int i = 0; i < groupEnemies.Length; i++)
            {
                if (groupEnemies[i].GetComponent<HealthComponent>()._health > 0)
                {
                    while (ListPlayerDistribution.Count < 8)
                    {
                        key = random.Next(0, _counterAlivePlayer);
                        if (!usedKeys.Contains(key))
                        {
                            usedKeys.Add(key);
                            ListPlayerDistribution.Add(key, groupEnemies[i]);
                            listEnemy.Add(key, i);
                            break;
                        }
                    }
                }
            }
            for(int i = 0; i < groupPlayers.Length; i++)
            {
                if (groupPlayers[i].GetComponent<HealthComponent>()._health > 0)
                {
                    while (ListPlayerDistribution.Count < 8)
                    {
                        key = random.Next(0, _counterAlivePlayer);
                        if (!usedKeys.Contains(key))
                        {
                            usedKeys.Add(key);
                            ListPlayerDistribution.Add(key, groupPlayers[i]);
                            listPlayer.Add(key, i);
                            break;
                        }
                    }
                }
            }
            Debug.Log("lllllllllll  " + ListPlayerDistribution);
            foreach (var pair in ListPlayerDistribution)
            {
                Debug.Log("Ключ: " + pair.Key + ", Значение: " + pair.Value);
            }
        }
        List<BoxCollider2D> collidersDeath;
        int deathIndex;
        private IEnumerator    DamageAnimation(SwitchBeetwenPlayers[] CheckHPs, string GeneralHP8, HealthComponent HPplayer, SpriteRenderer RenderPlayer, BoxCollider2D Colider,Transform transform)
        {
            Vector2 mousePosition = transform.position;        
            var flyDamage = mousePosition.y;
            var endFly = flyDamage + 5f;
            var changeColorA=1f;         

            _editHealth.text = HPplayer._health.ToString() + "/" + GeneralHP8;

            _damageHealth.text = HPplayer.damage.ToString();
            while (flyDamage< endFly) //animation damage
            {
                changeColorA -= SpeedAlfaHide * Time.deltaTime;
                flyDamage += Time.deltaTime * SpeedUpMoovDamage;
                _damageHealth.transform.position = new Vector2(mousePosition.x, flyDamage);
                _damageHealth.color = new Color(_damageHealth.color.r, _damageHealth.color.g, _damageHealth.color.b, changeColorA);              
                yield return null;
            }
            _damageHealth.color = new Color(_damageHealth.color.r, _damageHealth.color.g, _damageHealth.color.b, 0);

             collidersDeath = new List<BoxCollider2D>();//Switch frame
            for (int i = 0; i < CheckHPs.Length; i++)
            {
                collidersDeath.Add(CheckHPs[i].GetComponent<BoxCollider2D>());
            }
            for (int i = 0; i < CheckHPs.Length; i++)
            {
                if (!CheckHPs[i]!.movedFrame && HPplayer._health <= 0)
                {
                    collidersDeath[i].enabled = false;//player died
                    deathIndex = i;

                  //  int lost = 0;
                    for (int j = 0; j < collidersDeath.Count; j++)
                    {
                        if (collidersDeath[j].enabled)
                        {
                            CheckHPs[j].Action();
                            break;
                        }                     
                    }
                    break;
                }
            }

            int lost = 0;//the end game
            for (int i = 0; i < CheckHPs.Length; i++)
            {
                if (!collidersDeath[i].enabled) 
                {
                    lost++;                    
                }
                if (lost == CheckHPs.Length)
                {
                    died = false;
                    RenderPlayer.enabled = false;//maybe to do event
                    _statusTheEnd.Invoke();
                }

            }               
         }
     
        public void death()//it doesn't work until the end  
        {
            
            _editHealth.text = "888";
            _spriteRenderer = _ObjectDeath.GetComponent<SpriteRenderer>();//Привязать смерть
            _spriteRenderer.enabled = true;
          //  _spriteRenderer1 = _Heroi.GetComponent<SpriteRenderer>();
          //  _spriteRenderer1.enabled = true;
        }
     
    }
   
}