using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Script
{
    public class ReloadLevel : MonoBehaviour
    {


        private void OnMouseDown()
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        
    }
}