using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
    public Texture backgroundTexture;

    public Texture newGame;
    public Texture levelSelect;
    public Texture settings;
    public Texture noSource;

    public float guiPlacementX;
    public float guiPlacementX1;
    public float guiPlacementX2;
    public float guiPlacementX3;

    public float guiPlacementY;
    public float guiPlacementY1;
    public float guiPlacementY2;
    public float guiPlacementY3;

    void OnGUI()
    {
        //Display background texture
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), backgroundTexture);
        GUI.DrawTexture(new Rect(Screen.width * guiPlacementX3, Screen.height * guiPlacementY3, Screen.width * .4f, Screen.height * .4f), noSource);

        //Display buttons without GUI outline
        if (GUI.Button(new Rect(Screen.width * guiPlacementX, Screen.height * guiPlacementY, Screen.width * .25f, Screen.height * .1f),newGame, ""))
        {
            //Application.LoadLevel("Level 1");
        }
        if (GUI.Button(new Rect(Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * .25f, Screen.height * .1f),levelSelect, ""))
        {
            print("Insert level select");
            GUI.Button(new Rect(Screen.width * .25f, Screen.height * .8f, Screen.width * .1f, Screen.height * .1f),"Level 1");
        }
        if (GUI.Button(new Rect(Screen.width * guiPlacementX2, Screen.height * guiPlacementY2, Screen.width * .25f, Screen.height * .1f),settings, ""))
        {
            print("Insert options here");
        }
    }
}
