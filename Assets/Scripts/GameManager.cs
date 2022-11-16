using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public List<GameObject> levels;
    public static int levelPassed;
    public static int coinsCollected = 0;
    public GameObject arrowPrefab;
    private GameObject arrow;
    // Start is called before the first frame update
    void Start()
    {
        if(arrow != null){
            arrow = null;
        }
    }

    public static void passLevel(int level){
        Debug.Log("level pass");
        if(level >= GameManager.levelPassed){
            GameManager.levelPassed += 1;
        }
    }

    // Update level select scene
    void Update()
    {
        if(levels.Count >0 && SceneManager.GetActiveScene().name =="Level Selector"){
            for(int i = 0; i < levels.Count; i++){
                if(GameManager.levelPassed >= i){
                    levels[i].SetActive(true);
                    if(GameManager.levelPassed == i){
                        if(arrow != null){
                            arrow.transform.position = levels[i].transform.position;
                        }else{
                            if(arrowPrefab != null){
                                arrow = Instantiate(arrowPrefab,levels[i].transform);
                            }
                            
                        }
                    }
                }else{
                    levels[i].SetActive(false);
                }
            }
        }
    }
}
