using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
   [SerializeField] private Image image;

   [SerializeField] 
   private Color selectedColor, deselectedColor;
   

   private void Awake()
   {
      Deselect();
   }
   
   public void Select()
   {
      image.color = selectedColor;
   }
   public void Deselect()
   {
      image.color = deselectedColor;
   }
}
