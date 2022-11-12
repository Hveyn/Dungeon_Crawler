using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/Item")]
public class SpetificationsItem : ScriptableObject
{
    [Header("Only gameplay")] 
    [SerializeField]
    private ItemName iname;
    [SerializeField]
    private ItemType type;
    [SerializeField]
    private ActionType action;

    public ItemName Iname => iname;

    public ItemType Type => type;

    public ActionType Action => action;

    [Header("Only Ui")] 
    public bool stackable = true;
    
    [Header("Both")]
    public Sprite image;

    public enum ItemName
    {
        Sword,
        Bomb,
        FlaskGreen,
        FlaskRed,
        FlaskYellow,
        Key,
        Coin
    }
    
    public enum ItemType
    {
        Potion, 
        Weapon,
        Story
    }
    
    public enum ActionType
    {
        Attack, // предметы наносящие урон врагам
        Heal,   // лечящие предметы
        Poison, // отравляющие предметы
        Boost,  // ускоряющие предметы
        PowerUp, // усиляющие предметы
        OpenDoor,
        Rezult
    }
}
