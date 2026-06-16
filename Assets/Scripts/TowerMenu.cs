using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;


public class TowerMenu : MonoBehaviour
{
    public static TowerMenu Instance;

    [Header("UI")]
    public GameObject panel;          
    public Text statsText;            
    public Text sellButtonText;      
    public Text upgradeButtonText;   

    [Header("Смещение окна над башней (в пикселях экрана)")]
    public Vector2 screenOffset = new Vector2(0f, 90f);

    private Tower currentTower;
    private int openedFrame = -1;

    public bool IsOpen => panel != null && panel.activeSelf;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (panel) panel.SetActive(false);
    }

    void Update()
    {
        if (currentTower == null || panel == null || !panel.activeSelf) return;
        if (Time.frameCount == openedFrame) return;

        if (Input.GetMouseButtonDown(0))
        {

            if (!IsPointerOverMenu())
                Close();
        }
    }

    bool IsPointerOverMenu()
    {
        if (EventSystem.current == null) return false;

        PointerEventData ped = new PointerEventData(EventSystem.current);
        ped.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);

        foreach (var r in results)
        {
            if (r.gameObject == panel || r.gameObject.transform.IsChildOf(panel.transform))
                return true;
        }
        return false;
    }

    public void Open(Tower tower)
    {
        if (currentTower != null && currentTower != tower)
            currentTower.HideRange();  

        currentTower = tower;
        openedFrame = Time.frameCount;
        if (panel) panel.SetActive(true);
        currentTower.ShowRange();
        Refresh();
        PositionOverTower();
    }

    void Refresh()
    {
        if (currentTower == null) return;
        if (statsText) statsText.text = currentTower.Describe();
        if (sellButtonText) sellButtonText.text = "(+" + currentTower.SellValue + "$)";
        if (upgradeButtonText) upgradeButtonText.text = "(-" + currentTower.UpgradeCost + "$)";
    }

    void PositionOverTower()
    {
        if (currentTower == null || panel == null || Camera.main == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(currentTower.transform.position);
        RectTransform rt = panel.GetComponent<RectTransform>();
        if (rt != null)
            rt.position = screenPos + (Vector3)screenOffset;
    }

    public void Sell()
    {
        if (currentTower != null) currentTower.Sell();
        AudioManager.Sell();
        Close();
    }

    public void Upgrade()
    {
        if (currentTower == null) return;
        if (currentTower.Upgrade())
        {
            AudioManager.Upgrade();
            currentTower.ShowRange(); 
            Refresh();
        }
        else
        {
            AudioManager.NoMoney();
        }
    }

    public void Close()
    {
        if (currentTower != null) currentTower.HideRange();
        if (panel) panel.SetActive(false);
        currentTower = null;
    }
}