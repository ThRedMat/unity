using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour
{
    public UnityEngine.UI.Button ButtonExit;

    void Update()
    {
        ButtonExit.onClick.AddListener(ButtonExitClicked);
    }

    void ButtonExitClicked()
    {
        Application.Quit();
    }
}
