using UnityEngine;
using System.Collections;

public class levelSelect : MonoBehaviour {
    public Transform mainMenu, levelSelectMenu, settingsMenu, newGame, newGameBar, settingsBar, settings, level,levelBar; 

    public void LevelMenu(bool click)
    {
        if(click == true)
        {
            levelSelectMenu.gameObject.SetActive(click);    
            mainMenu.gameObject.SetActive(false);
            newGameBar.gameObject.SetActive(true);
            newGame.gameObject.SetActive(false);
            settings.gameObject.SetActive(false);
            settingsBar.gameObject.SetActive(true);
        }
        else
        {
            levelSelectMenu.gameObject.SetActive(click);
            mainMenu.gameObject.SetActive(true);
            newGameBar.gameObject.SetActive(false);
            newGame.gameObject.SetActive(true);
            settings.gameObject.SetActive(true);
            settingsBar.gameObject.SetActive(false);
        }
    }

    public void SettingsMenu(bool click)
    {
        if(click == true)
        {
            settingsMenu.gameObject.SetActive(click);
            mainMenu.gameObject.SetActive(false);
            newGame.gameObject.SetActive(false);
            newGameBar.gameObject.SetActive(true);
            level.gameObject.SetActive(false);
            levelBar.gameObject.SetActive(true);
        }
        else
        {
            settingsMenu.gameObject.SetActive(click);
            mainMenu.gameObject.SetActive(true);
            newGame.gameObject.SetActive(true);
            newGameBar.gameObject.SetActive(false);
            level.gameObject.SetActive(true);
            levelBar.gameObject.SetActive(false);
        }
    }
}
