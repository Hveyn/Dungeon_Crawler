using UnityEngine;

public class DemoScript : MonoBehaviour
{
    [SerializeField] 
    private InventoryManager inventoryManager;
    [SerializeField] 
    private Item[] itemToPickup;

    public void PickupItem(int id)
    {
       bool result = inventoryManager.AddItem(itemToPickup[id]);
       if (result) Debug.Log("Added");
       else Debug.Log("Not Added");
    }

    public void UseSelectedItem()
    {
        Item receivedItem = inventoryManager.GetSelectedItem(true);
        if (receivedItem != null)
        {
            Debug.Log("used Item" + receivedItem);
        }
        else
        {
            Debug.Log("No item");
        }
    }
}
