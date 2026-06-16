using UnityEngine;
using UnityEngine.SceneManagement;

// Положи на объект в сцене главного меню.
public class MainMenu : MonoBehaviour
{
    [Header("Имя игровой сцены")]
    public string gameScene = "SampleScene";

    [Header("Панель инструкции")]
    public GameObject instructionPanel;

    void Start()
    {
        if (instructionPanel) instructionPanel.SetActive(false);
    }

    public void PlayGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameScene);
    }

    public void ShowInstructions()
    {
        if (instructionPanel) instructionPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        if (instructionPanel) instructionPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}