using System;
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
    private SpetificationsItem _oldSpetsItem;
    
    private void FixedUpdate()
    {
        SpetificationsItem newItem = InventoryManager.Instance.GetSelectedItem(false);
        
        if (newItem != null)
        {
            if (_oldSpetsItem != newItem)
            {
                CreateItem(newItem);
                _oldSpetsItem = newItem;
            }
        }
        else
        {
            _oldSpetsItem = null;
            Destroy(_newTool);
        }
        
        if (_oldSpetsItem == null)
        {
            InventoryManager.Instance.ChangedSelectedSlot(0);
        }
        
        if (Input.GetMouseButtonDown(0))
        {
            if (_oldSpetsItem.stackable)
            {
                InventoryManager.Instance.GetSelectedItem(true);
            }
        }
    }

    private void CreateItem(SpetificationsItem statsItem)
    {
        if (_newTool != null)
        {
            Destroy(_newTool);
        }
        
        
        if (statsItem.Iname == SpetificationsItem.ItemName.Sword)
        {
            _newTool = Instantiate(items[0], pointRotation.transform);
            _newTool.GetComponent<ItemRotation>().player = player;
            _newTool.GetComponent<ItemRotation>().pointRotation = pointRotation;
        }
        else if (statsItem.Iname == SpetificationsItem.ItemName.Bomb)
        {
            _newTool = Instantiate(items[1], pointRotation.transform);
            _newTool.GetComponent<ItemRotation>().player = player;
            _newTool.GetComponent<ItemRotation>().pointRotation = pointRotation;
        }
        else if (statsItem.Iname == SpetificationsItem.ItemName.FlaskGreen)
        {
            _newTool = Instantiate(items[2], pointRotation.transform);
            _newTool.GetComponent<ItemRotation>().player = player;
            _newTool.GetComponent<ItemRotation>().pointRotation = pointRotation;
        }
        else if (statsItem.Iname == SpetificationsItem.ItemName.FlaskRed)
        {
            _newTool = Instantiate(items[3], pointRotation.transform);
            _newTool.GetComponent<ItemRotation>().player = player;
            _newTool.GetComponent<ItemRotation>().pointRotation = pointRotation;
        }
        else if (statsItem.Iname == SpetificationsItem.ItemName.FlaskYellow)
        {
            _newTool = Instantiate(items[4], pointRotation.transform);
            _newTool.GetComponent<ItemRotation>().player = player;
            _newTool.GetComponent<ItemRotation>().pointRotation = pointRotation;
        }
        
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        GameObject newObject = col.gameObject;
        Debug.Log(newObject.name);
        if (newObject.GetComponent<Item>())
        {
            var newItem = newObject.GetComponent<Item>().SpetsItem;
            InventoryManager.Instance.AddItem(newItem);
            Destroy(newObject);
        }
    }
}
