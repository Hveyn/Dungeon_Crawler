using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;
    
    [SerializeField] 
    private int maxStackedItems = 3;
    [SerializeField] 
    private InventorySlot[] inventorySlots;
    [SerializeField] 
    private GameObject inventoryItemPrefab;
    
    [SerializeField] 
    private GameObject bombItemPrefab;
    
    [Header("UI")]
    [SerializeField] 
    private Text uiCountCoins;
    [SerializeField]
    private int minCoinsInBag;
    [SerializeField]
    private int maxCoinsInBag;
    [SerializeField] 
    private Text uiCountKeys;
    [SerializeField] 
    private Text uiCountEnemy;
   
    private int _countCoins = 0;
    private int _countKeys = 0;

    private int _selectedSlot = -1;
    
    private DungeonData _dungeonData;
    
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _dungeonData = FindObjectOfType<DungeonData>();
        ChangedSelectedSlot(0);
        Debug.Log("sel 0");
        GetSelectedItem(false);
        
        uiCountCoins.text = "0";
        uiCountKeys.text = "0";
    }

    private void Update()
    {
        if (_dungeonData == null) _dungeonData = FindObjectOfType<DungeonData>();
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number is > 0 and < 6)
            {
                ChangedSelectedSlot(number-1);

                if (number != 2)
                {
                    SpetificationsItem item = GetSelectedItem(true);
                    
                    if(item != null) FlasksEffects.Instance.SelectedEffect(item);
                    
                    ChangedSelectedSlot(0);
                }
                else
                {
                    GetSelectedItem(false);
                }
            }
        }
        UpdateUiCount(uiCountEnemy,_dungeonData.countEnemy);
    }

    public void ChangedSelectedSlot(int newValue)
    {
        if (_selectedSlot >= 0)
        {
            inventorySlots[_selectedSlot].Deselect();
        }
        
        inventorySlots[newValue].Select();
        _selectedSlot = newValue;
    }
    
    public bool AddItem(SpetificationsItem spetsItem)
    {
        if (spetsItem.Type == SpetificationsItem.ItemType.Story)
        {
            if (spetsItem.Iname == SpetificationsItem.ItemName.Coin)
            {
                _countCoins += Random.Range(minCoinsInBag,maxCoinsInBag);
                UpdateUiCount(uiCountCoins, _countCoins);
                return false;
            }
            else if (spetsItem.Iname == SpetificationsItem.ItemName.Key)
            {
                _countKeys += 1;
                UpdateUiCount(uiCountKeys, _countKeys);
                return false;
            }
        }
        
        foreach (var slot in inventorySlots)
        {
            InventoryItems itemInSlot = slot.GetComponentInChildren<InventoryItems>();
            if (itemInSlot != null && itemInSlot.SpetificationsItem == spetsItem && itemInSlot.count < maxStackedItems)
            {

                itemInSlot.count++;
                itemInSlot.RefreshCount();
                return true;
            }
        }
        
        // Find any empty slot
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            InventorySlot slot;
            if ((int) spetsItem.Iname == 1)
            {
                slot = inventorySlots[1];
            }
            else
            {
                if (i == 1)
                {
                    continue;
                }
                slot = inventorySlots[i];
            }
            
            InventoryItems itemInSlot = slot.GetComponentInChildren<InventoryItems>();
            if (itemInSlot == null)
            {
                SpawnNewItem(spetsItem, slot);
                return true;
            }
            
        }

        return false;
    }

    private void SpawnNewItem(SpetificationsItem spetificationsItem, InventorySlot slot)
    {
        GameObject newItemGo;
        if (spetificationsItem.Iname == SpetificationsItem.ItemName.Bomb)
        {
            newItemGo = Instantiate(bombItemPrefab, slot.transform);
        }
        else
        {
            newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        }
        
        InventoryItems inventoryItem = newItemGo.GetComponent<InventoryItems>();
        inventoryItem.InitializeItem(spetificationsItem);
    }

    public SpetificationsItem GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[_selectedSlot];
        InventoryItems itemInSlot = slot.GetComponentInChildren<InventoryItems>();
        if (itemInSlot != null)
        {

            SpetificationsItem spetificationsItem = itemInSlot.SpetificationsItem;
            if (use && spetificationsItem.Iname != SpetificationsItem.ItemName.Sword)
            {
                itemInSlot.count--;
                if (itemInSlot.count <= 0)
                {
                    Destroy(itemInSlot.gameObject);
                }
                else
                {
                    itemInSlot.RefreshCount();
                }
            }
            return spetificationsItem;
        }
        
        return null;
    }

    private void UpdateUiCount(Text text, int count)
    {
        text.text = count.ToString();
    }
}
