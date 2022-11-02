using UnityEngine;

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
    
    private int _selectedSlot = -1;

    private void Awake()
    {
        Instance = this; 
    }

    private void Start()
    {
        ChangedSelectedSlot(0);
        GetSelectedItem(false);
    }

    private void Update()
    {
        if (Input.inputString != null)
        {
            bool isNumber = int.TryParse(Input.inputString, out int number);
            if (isNumber && number is > 0 and < 6)
            {
                ChangedSelectedSlot(number-1);

                if (number != 2)
                {
                    GetSelectedItem(true);
                    ChangedSelectedSlot(0);
                }
                else
                {
                    GetSelectedItem(false);
                }
            }
        }
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
    
    public bool AddItem(Item item)
    {
        
        foreach (var slot in inventorySlots)
        {
            InventoryItems itemInSlot = slot.GetComponentInChildren<InventoryItems>();
            if (itemInSlot != null && itemInSlot.Item == item && itemInSlot.count < maxStackedItems)
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
            if (item.Iname == Item.ItemName.Bomb)
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
                SpawnNewItem(item, slot);
                return true;
            }
            
        }

        return false;
    }

    private void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGo;
        if (item.Iname == Item.ItemName.Bomb)
        {
            newItemGo = Instantiate(bombItemPrefab, slot.transform);
        }
        else
        {
            newItemGo = Instantiate(inventoryItemPrefab, slot.transform);
        }
        
        InventoryItems inventoryItem = newItemGo.GetComponent<InventoryItems>();
        inventoryItem.InitializeItem(item);
    }

    public Item GetSelectedItem(bool use)
    {
        InventorySlot slot = inventorySlots[_selectedSlot];
        InventoryItems itemInSlot = slot.GetComponentInChildren<InventoryItems>();
        if (itemInSlot != null)
        {

            Item item = itemInSlot.Item;
            if (use && item.Iname != Item.ItemName.Sword)
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
            return item;
        }
        
        return null;
    }
}
