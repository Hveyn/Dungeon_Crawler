using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject pointRotation;
    [SerializeField] 
    private GameObject[] items;

    private GameObject _newTool;
    private Item _oldItem;

    void Update()
    {
        Item newItem = InventoryManager.Instance.GetSelectedItem(false);

        if (newItem != null)
        {
            if (_oldItem != newItem)
            {
                CreateItem(newItem);
                _oldItem = newItem;
            }
        }
        else
        {
            _oldItem = null;
            Destroy(_newTool);
        }
        
        if (_oldItem == null)
        {
            InventoryManager.Instance.ChangedSelectedSlot(0);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (_oldItem.stackable)
            {
                InventoryManager.Instance.GetSelectedItem(true);
            }
        }
    }

    private void CreateItem(Item item)
    {
        if (_newTool != null)
        {
            Destroy(_newTool);
        }
        
        if (item.Iname == Item.ItemName.Sword)
        {
            _newTool = Instantiate(items[0], pointRotation.transform);
            _newTool.GetComponent<ItemRotation>().player = player;
            _newTool.GetComponent<ItemRotation>().pointRotation = pointRotation;
        }
        else if (item.Iname == Item.ItemName.Bomb)
        {
            _newTool = Instantiate(items[1], pointRotation.transform);
            _newTool.GetComponent<ItemRotation>().player = player;
            _newTool.GetComponent<ItemRotation>().pointRotation = pointRotation;
        }
        else if (item.Iname == Item.ItemName.FlaskGreen)
        {
            _newTool = Instantiate(items[2], pointRotation.transform);
            _newTool.GetComponent<ItemRotation>().player = player;
            _newTool.GetComponent<ItemRotation>().pointRotation = pointRotation;
        }
        else if (item.Iname == Item.ItemName.FlaskRed)
        {
            _newTool = Instantiate(items[3], pointRotation.transform);
            _newTool.GetComponent<ItemRotation>().player = player;
            _newTool.GetComponent<ItemRotation>().pointRotation = pointRotation;
        }
        else if (item.Iname == Item.ItemName.FlaskYellow)
        {
            _newTool = Instantiate(items[4], pointRotation.transform);
            _newTool.GetComponent<ItemRotation>().player = player;
            _newTool.GetComponent<ItemRotation>().pointRotation = pointRotation;
        }
        
        
    }
}
