using UnityEngine;

public class TowerStats
{
    public string title;
    public float range;
    public float fireRate;
    public int damage;
    public Element element;
    public Color color;     


    public string Describe()
    {
        string typeName = (element != Element.None) ? ElementName(element) : title;
        return "Тип: " + typeName
             + "\nУрон: " + damage
             + "\nСкорость: " + fireRate.ToString("0.0") + " выстр/с"
             + "\nРадиус: " + range.ToString("0.0");
    }

    public static string ElementName(Element e)
    {
        switch (e)
        {
            case Element.Fire: return "Огонь";
            case Element.Water: return "Вода";
            case Element.Ice: return "Лёд";
            case Element.Electricity: return "Электричество";
            default: return "нет";
        }
    }
}