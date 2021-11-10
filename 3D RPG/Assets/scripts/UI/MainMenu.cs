using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class MainMenu : MonoBehaviour
{
    Button newGameButton;
    Button continueButton;
    Button quitButton;

    PlayableDirector director;

    void Awake()
    {
        newGameButton = transform.GetChild(1).GetComponent<Button>();
        continueButton = transform.GetChild(2).GetComponent<Button>();
        quitButton = transform.GetChild(3).GetComponent<Button>();

        newGameButton.onClick.AddListener(PlayTimeline);
        continueButton.onClick.AddListener(ContinueGame);
        quitButton.onClick.AddListener(QuitGame);

        director = FindObjectOfType<PlayableDirector>();
        director.stopped += NewGame;
    }

    void PlayTimeline()
    {
        director.Play();
    }

    void NewGame(PlayableDirector obj)
    {
        PlayerPrefs.DeleteAll();
        // 转换场景
        SceneController.Instance.TransitionToFirstLevel();
    }

    void ContinueGame()
    {
        // 转换场景，读取进度
        SceneController.Instance.TransitionToLoadGame();
    }

    void QuitGame()
    {
        Application.Quit();
    }
}
