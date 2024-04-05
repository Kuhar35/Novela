using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Script
{
   
    public class MenuAction : MonoBehaviour
    {

        
        [SerializeField] public SpriteRenderer RendererBlur;
        //[SerializeField] public GameObject Attack1;
        //[SerializeField] public GameObject Attack2; 
        //[SerializeField] public GameObject Attack3; 
        //[SerializeField] public GameObject Attack4; 
        //[SerializeField] public GameObject Attack5; 

        public int countObjectsAttack = 5; // количество объектов
        public float circleRadius = 2f; // радиус круга, по которому нужно разместить объекты
        public float startAngle = 4.72f;
        public float positionX =-2.1f;
        public float positionY = -12.6f;


        public float startRadius = 0.3f; // Начальный радиус
        public float endRadius = 2f; // Конечный радиус
        public float durationAnimationCircle = 1f;
        public bool MenuActive=false;

       // public GameObject HeroiImage;
       

        public List<GameObject> objectsList = new List<GameObject>();
        [ContextMenu("Spawn")]
        private void Spawn()
        {
            PlaceObjectsInCircle(countObjectsAttack);
        }

       
        public void PlaceObjectsInCircle(int countObjectsAttack)
        {
            this.countObjectsAttack = countObjectsAttack;
            RendererBlur.enabled = true;
            
            
            for (int i = 0; i < countObjectsAttack; i++)
            {
                ActionAttackInfo QuantityAbility = objectsList[i].GetComponent<ActionAttackInfo>();
                if (QuantityAbility.quantityAbility==0)
                {
                   objectsList[i].GetComponent<SpriteRenderer>().color= new Color(0.38f, 0.32f, 0.32f, 1f);
                   objectsList[i].GetComponent<CircleCollider2D>().enabled = false;
                
                }
       
            }

            startAngle = 0;
            StartCoroutine(AnimationBlur());
            StartCoroutine(ExpandCreatedObjects(startRadius, endRadius, durationAnimationCircle));
        }

        [ContextMenu("CloseMenu")]
        public void CloseMenu()
        {
            //поиск по тэгу Player;
            GameObject.FindGameObjectWithTag("Player").GetComponent<BoxCollider2D>().enabled=true;
            RendererBlur.enabled = false;
            StartCoroutine(ExpandCreatedObjects(2,0,0.5f));
         
            
            //  HeroiImage.GetComponent<BoxCollider2D>().enabled = true;
            //this.gameObject.SetActive(false);
        }
        private IEnumerator ExpandCreatedObjects(float startRadius, float endRadius,float durationAnimationCircle)
        {   
            float elapsed = 0f; 
            float fixAngel= (float)System.Math.Round(startAngle, 2);
            bool checkEndSecond = true;
            bool checkEndFirst = true;
            float currentRadius=0;
            while (fixAngel < 4.72f && checkEndSecond)
            {
                if (elapsed < durationAnimationCircle)
                {
                    elapsed += Time.deltaTime*2;
                } 
                currentRadius = Mathf.Lerp(startRadius, endRadius, elapsed / durationAnimationCircle);               
                if (startAngle > 6|| !checkEndFirst)//Revers rotetion
                {
                    checkEndFirst = false;
                    startAngle -= 0.05f;
                    if (startAngle<4.73) checkEndSecond = false;
                }
                else {
                    startAngle += 0.2f;                 
                   
                }

                // Перемещаем объекты
                float angleIncrement = 2f * Mathf.PI / countObjectsAttack;
                Vector2 currentPosition = transform.position - new Vector3(positionX, positionY, 0);
                for (int i = 0; i < countObjectsAttack; i++)  ///objectsList.Count
                {
                    float angle = startAngle + i * angleIncrement;
                    Vector2 offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * currentRadius;
                    Vector2 newPos = currentPosition - offset;
                    objectsList[i].transform.position = newPos; // Изменяем позицию объекта
                }
               // Debug.Log(currentRadius);               
                MenuActive = true;
                yield return null; 
            }

            if (currentRadius <= 0.5)
            { //close menu
                for (int i = 0; i < countObjectsAttack; i++)
                {
                    objectsList[i].GetComponent<SpriteRenderer>().enabled = false;
                    objectsList[i].GetComponent<CircleCollider2D>().enabled = false;
                    MenuActive = false;
                }
            }
        }
        private IEnumerator AnimationBlur()
        {
            RendererBlur.enabled = true;
            bool isIncreasing = true;
            while (RendererBlur.enabled)
            {
                if (isIncreasing)
                {
                    RendererBlur.transform.localScale += new Vector3(0.1f, 0.1f, 0f); 
                    if (RendererBlur.transform.localScale.x >= 8)
                    {
                        isIncreasing = false; 
                    }
                }
                else
                {
                    RendererBlur.transform.localScale -= new Vector3(0.1f, 0.1f, 0f); 
                    if (RendererBlur.transform.localScale.x <= 4)
                    {
                        isIncreasing = true; 
                    }
                }

                yield return new WaitForSeconds(0.03f); 
            }
        }

          
        

    }
}