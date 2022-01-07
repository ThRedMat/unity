using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string SceneName;
    public UnityEngine.UI.Button ButtonClicked;

    void Update()
    {
        ButtonClicked.onClick.AddListener(ButtonPressed);
    }

    void ButtonPressed()
    {
        SceneManager.LoadScene(sceneName: SceneName);
    }

}
