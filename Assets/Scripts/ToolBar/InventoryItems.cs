using UnityEngine;
using UnityEngine.UI;

public class InventoryItems : MonoBehaviour
{
    [SerializeField] 
    private SpetificationsItem spetificationsItem;

    public SpetificationsItem SpetificationsItem => spetificationsItem;
    
    [Header("Ui")]
    [SerializeField] 
    private Text countText;
    
    public int count = 1;

    public void InitializeItem(SpetificationsItem newSpetificationsItem)
    {
        spetificationsItem = newSpetificationsItem;
        gameObject.GetComponent<Image>().sprite = newSpetificationsItem.image;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool textActive = count > 1;
        countText.gameObject.SetActive(textActive);
    }
}
