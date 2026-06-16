using UnityEngine;


public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    private GameObject towerToBuild;
    private int cost;

    void Awake()
    {
        Instance = this;
    }

    public bool HasTowerSelected() => towerToBuild != null;
    public GameObject GetSelectedTower() => towerToBuild;
    public int GetSelectedCost() => cost;

    public void SelectTowerToBuild(GameObject tower, int towerCost)
    {
        towerToBuild = tower;
        cost = towerCost;
    }

    public void ClearSelection()
    {
        towerToBuild = null;
        cost = 0;
    }
}
