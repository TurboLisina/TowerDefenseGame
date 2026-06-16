using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseMenu : MonoBehaviour
{
    [Header("Панели")]
    public GameObject pausePanel;        
    public GameObject instructionPanel;   

    [Header("Кнопка паузы")]
    public GameObject pauseButton;

    [Header("Громкость")]
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Сцена главного меню")]
    public string mainMenuScene = "MainMenu";

    private bool paused;

    void Start()
    {
        if (pausePanel) pausePanel.SetActive(false);
        if (instructionPanel) instructionPanel.SetActive(false);

        if (AudioManager.Instance != null)
        {
            if (musicSlider) musicSlider.value = AudioManager.Instance.MusicVolume;
            if (sfxSlider) sfxSlider.value = AudioManager.Instance.SfxVolume;
        }
        if (musicSlider) musicSlider.onValueChanged.AddListener(OnMusicChanged);
        if (sfxSlider) sfxSlider.onValueChanged.AddListener(OnSfxChanged);
    }

    void Update()
    {
        if (!paused) return;

        if (Input.GetMouseButtonDown(0) && !PointerOverPauseUI())
            Resume();
    }

    bool PointerOverPauseUI()
    {
        if (EventSystem.current == null) return false;

        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);

        foreach (var r in results)
        {
            if (BelongsTo(r.gameObject, pausePanel)) return true;
            if (BelongsTo(r.gameObject, instructionPanel)) return true;
            if (BelongsTo(r.gameObject, pauseButton)) return true;
        }
        return false;
    }

    bool BelongsTo(GameObject obj, GameObject root)
    {
        if (root == null) return false;
        return obj == root || obj.transform.IsChildOf(root.transform);
    }

    // Кнопка "Пауза"
    public void TogglePause()
    {
        paused = !paused;
        if (pausePanel) pausePanel.SetActive(paused);
        if (!paused && instructionPanel) instructionPanel.SetActive(false);
        Time.timeScale = paused ? 0f : 1f;   
    }

    public void Resume()
    {
        paused = false;
        if (pausePanel) pausePanel.SetActive(false);
        if (instructionPanel) instructionPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowInstructions()
    {
        if (instructionPanel) instructionPanel.SetActive(true);
    }

    public void HideInstructions()
    {
        if (instructionPanel) instructionPanel.SetActive(false);
    }

    void OnMusicChanged(float v)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SetMusicVolume(v);
    }

    void OnSfxChanged(float v)
    {
        if (AudioManager.Instance != null) AudioManager.Instance.SetSfxVolume(v);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}