using UnityEngine;
using System.Collections;


public class LockScreen : MonoBehaviour
{

    private int levelIndex;

    void Start()
    {
                CheckLockedLevels();
    }

    //Level to load on button click. Will be used for Level button click event 
    public void Selectlevel(string level)
    {
        Application.LoadLevel("Level" + level); //load the level
    }

    //uncomment the below code if you have a main menu scene to navigate to it on clicking escape when in World1 scene
    /*public void  Update (){
     if (Input.GetKeyDown(KeyCode.Escape) ){
      Application.LoadLevel("MainMenu");
     }   
    }*/

    //function to check for the levels locked
    void CheckLockedLevels()
    {
        //loop through the levels of a particular world
        for (int j = 1; j < LockLevel.levels; j++)
        {
            levelIndex = (j + 1);
            if ((PlayerPrefs.GetInt("level" + levelIndex.ToString())) == 1)
            {
                GameObject.Find("LockedLevel" + (levelIndex)).active = false;
                Debug.Log("Unlocked");
            }
        }
    }
}