using UnityEngine;

public class DemoScript : MonoBehaviour
{
    [SerializeField] 
    private InventoryManager inventoryManager;
    [SerializeField] 
    private SpetificationsItem[] itemToPickup;

    public void PickupItem(int id)
    {
       bool result = inventoryManager.AddItem(itemToPickup[id]);
       if (result) Debug.Log("Added");
       else Debug.Log("Not Added");
    }

    public void UseSelectedItem()
    {
        SpetificationsItem receivedSpetificationsItem = inventoryManager.GetSelectedItem(true);
        if (receivedSpetificationsItem != null)
        {
            Debug.Log("used Item" + receivedSpetificationsItem);
        }
        else
        {
            Debug.Log("No item");
        }
    }
}
