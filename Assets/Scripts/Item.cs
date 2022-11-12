using UnityEngine;

public class Item : MonoBehaviour
{
   [SerializeField]
   private SpetificationsItem spetsItem;

   public SpetificationsItem SpetsItem => spetsItem;
   
   private void Start()
   {
      gameObject.GetComponent<SpriteRenderer>().sprite = spetsItem.image;
   }
}
