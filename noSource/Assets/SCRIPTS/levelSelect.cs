using UnityEngine;
using System.Collections;

public class levelSelect : MonoBehaviour {
    public Texture box;

    public int levelCount;

    void OnGui()
    {
        if(levelCount == 1)
        {
            GUI.DrawTexture(new Rect(Screen.width * .1f, Screen.height * .1f, Screen.width * .25f, Screen.height * .1f),box);
        }
    }
}
