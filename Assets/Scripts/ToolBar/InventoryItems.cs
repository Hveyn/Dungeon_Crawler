using UnityEngine;
using UnityEngine.UI;

public class InventoryItems : MonoBehaviour
{
    [SerializeField] 
    private Item item;

    public Item Item => item;
    
    [Header("Ui")]
    [SerializeField] 
    private Text countText;
    
    public int count = 1;

    public void InitializeItem(Item newItem)
    {
        item = newItem;
        gameObject.GetComponent<Image>().sprite = newItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }
}
