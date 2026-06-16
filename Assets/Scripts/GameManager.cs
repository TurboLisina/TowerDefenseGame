using UnityEngine;
using UnityEngine.UI;            // обычный Text
using UnityEngine.SceneManagement;

// Главный «мозг» уровня: хранит золото, здоровье игрока,
// показывает победу/поражение. Существует в единственном экземпляре (Instance).
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Стартовые значения")]
    public int startGold = 200;
    public int startHealth = 20;

    [Header("UI")]
    public Text goldText;
    public Text healthText;
    public GameObject winPanel;     // панель «Победа» (выключена по умолчанию)
    public GameObject losePanel;    // панель «Поражение»
    public Text loseWaveText;       // текст «Пройдено волн» на LosePanel

    private int gold;
    private int health;
    private bool gameOver;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Time.timeScale = 1f;       
        gold = startGold;
        health = startHealth;
        if (winPanel) winPanel.SetActive(false);
        if (losePanel) losePanel.SetActive(false);
        UpdateUI();
    }

    public bool CanAfford(int cost) => gold >= cost;

    public void AddGold(int amount)
    {
        gold += amount;
        UpdateUI();
    }


    public bool SpendGold(int amount)
    {
        if (gold < amount) return false;
        gold -= amount;
        UpdateUI();
        return true;
    }

    public void TakeDamage(int amount)
    {
        if (gameOver) return;
        AudioManager.Damage();
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            Lose();
        }
        UpdateUI();
    }

    public void Win()
    {
        if (gameOver) return;
        gameOver = true;
        if (winPanel) winPanel.SetActive(true);
        Time.timeScale = 0f;       
    }

    void Lose()
    {
        gameOver = true;
        AudioManager.Lose();
        if (loseWaveText)
            loseWaveText.text = "Волна: " + Mathf.Max(WaveSpawner.CurrentWave - 1, 0);
        if (losePanel) losePanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public bool IsGameOver => gameOver;

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    void UpdateUI()
    {
        if (goldText) goldText.text = "" + gold;
        if (healthText) healthText.text = "" + health;
    }
}