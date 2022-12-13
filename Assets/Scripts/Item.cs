using UnityEngine;

public class Item : MonoBehaviour
{
   [SerializeField]
   private SpetificationsItem spetsItem;

   public SpetificationsItem SpetsItem => spetsItem;
   
   private void Awake()
   {
      gameObject.GetComponent<SpriteRenderer>().sprite = spetsItem.image;
   }

   public void SetItemType(SpetificationsItem newTypeItem)
   {
      spetsItem = newTypeItem;
   }
}
