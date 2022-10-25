using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] 
    private GameObject player;
    [SerializeField] 
    private GameObject pointRotation;

    private Vector2 _mousePosition;

    private Rigidbody2D _rbPlayer;

    void Start()
    {
        _rbPlayer = player.GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        RotateWeapon();
    }

    private void RotateWeapon()
    {
        //Rotate weapon around player
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDirection = _mousePosition - _rbPlayer.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;

        pointRotation.transform.localRotation = Quaternion.Euler(0, 0, aimAngle);


        if (!player.GetComponent<SpriteRenderer>().flipX)
        {
            FlipWeapon(false);
        }
        else if (player.GetComponent<SpriteRenderer>().flipX)
        {
            FlipWeapon(true);
        }
        

        
        
    }
    private void FlipWeapon(bool flip)
    {
        Vector2 scale = pointRotation.transform.localScale;
        if (flip)
        {
            scale.x = -1;
        }
        else
        {
            scale.x = 1;
        }

        pointRotation.transform.localScale = scale;
    }
    
}
