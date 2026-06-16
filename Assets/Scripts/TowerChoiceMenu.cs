using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TowerChoiceMenu : MonoBehaviour
{
    public static TowerChoiceMenu Instance;

    [Header("UI")]
    public GameObject panel;        
    public Button[] buttons;       
    public TMP_Text[] buttonTexts;  

    private TowerStats[] options = new TowerStats[3];
    private BuildSlot currentSlot;
    private float openedTime;

    public bool IsOpen => panel != null && panel.activeSelf;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (panel) panel.SetActive(false);
    }

    public void Open(BuildSlot slot)
    {
        currentSlot = slot;
        openedTime = Time.unscaledTime;

        for (int i = 0; i < options.Length; i++)
        {
            options[i] = TowerGenerator.Generate();

            if (i < buttonTexts.Length && buttonTexts[i] != null)
                buttonTexts[i].text = options[i].Describe();
        }

        if (panel) panel.SetActive(true);
    }

    public void Choose(int index)
    {
        if (Time.unscaledTime - openedTime < 0.2f) return;
        if (currentSlot == null) return;
        if (index < 0 || index >= options.Length) return;

        currentSlot.Build(options[index]);
        Close();
    }

    public void Close()
    {
        if (panel) panel.SetActive(false);
        currentSlot = null;
    }
}