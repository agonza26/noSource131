using UnityEngine;
using System.Collections;

public class UIControl : MonoBehaviour {

    public void ChangeLevel(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }

    public void ChangeSettings(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }

    public void ReturnMain(string sceneName)
    {
        Application.LoadLevel(sceneName);
    }
}
