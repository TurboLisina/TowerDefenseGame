// Г
using UnityEngine;

public static class TowerGenerator
{
    public static TowerStats Generate()
    {
        TowerStats newTower = new TowerStats();

        int kind = Random.Range(0, 6);

        if (kind == 0)
        {

            newTower.title = "Пулемётный";
            newTower.fireRate = Random.Range(3f, 6f);
            newTower.damage = Random.Range(1, 7);
            newTower.range = Random.Range(2f, 3.4f);
            newTower.element = Element.None;
            newTower.color = new Color(0.2f, 0.2f, 0.2f);
        }
        else if (kind == 1)
        {
            newTower.title = "Пушечный";
            newTower.fireRate = Random.Range(0.5f, 1f);
            newTower.damage = Random.Range(30, 160);
            newTower.range = Random.Range(2.5f, 4.8f);
            newTower.element = Element.None;
            newTower.color = new Color(1f, 0.8f, 0.5f);
        }
        else if (kind == 2)
        {
            newTower.title = "Огненный";
            newTower.fireRate = Random.Range(1.4f, 2.6f);
            newTower.damage = Random.Range(30, 80);
            newTower.range = Random.Range(3.1f, 4f);
            newTower.element = Element.Fire;
            newTower.color = ElementColor(newTower.element);
        }
        else if (kind == 3)
        {
            newTower.title = "Ледовый";
            newTower.fireRate = Random.Range(1.4f, 2.6f);
            newTower.damage = Random.Range(30, 80);
            newTower.range = Random.Range(3.1f, 4f);
            newTower.element = Element.Ice;
            newTower.color = ElementColor(newTower.element);
        }
        else if (kind == 4)
        {
            newTower.title = "Водный";
            newTower.fireRate = Random.Range(1.4f, 2.6f);
            newTower.damage = Random.Range(30, 80);
            newTower.range = Random.Range(3.1f, 4f);
            newTower.element = Element.Water;
            newTower.color = ElementColor(newTower.element);
        }
        else if (kind ==5)
        {
            newTower.title = "Электрический";
            newTower.fireRate = Random.Range(1.4f, 2.6f);
            newTower.damage = Random.Range(30, 80);
            newTower.range = Random.Range(3.1f, 4f);
            newTower.element = Element.Electricity;
            newTower.color = ElementColor(newTower.element);
        }
        return newTower;
    }

    static Color ElementColor(Element ElementTower)
    {
        switch (ElementTower)
        {
            case Element.Fire: return new Color(1f, 0.4f, 0.2f);
            case Element.Water: return new Color(0.3f, 0.6f, 1f);
            case Element.Ice: return new Color(0.6f, 0.9f, 1f);
            case Element.Electricity: return new Color(1f, 1f, 0.3f);
            default: return Color.white;
        }
    }
}