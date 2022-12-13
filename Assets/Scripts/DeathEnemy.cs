using UnityEngine;

public class DeathEnemy : MonoBehaviour
{

   private int _maxCountItemsSpawn;
   public void Death()
   {
      gameObject.GetComponent<DropItems>().Drop();
      Destroy(gameObject);
   }
}
