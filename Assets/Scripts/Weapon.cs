using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject pointRotation;

    private Vector2 _mousePosition;
    private bool _isFlipWeapon = false;
    private SpriteRenderer _spriteWeapon;

    private Rigidbody2D _rbPlayer;
    private Animator _weaponAnimator;
    void Start()
    {
        _rbPlayer = player.GetComponent<Rigidbody2D>();
        _spriteWeapon = GetComponent<SpriteRenderer>();
        _weaponAnimator = GetComponent<Animator>();
    }
    
    void Update()
    {
        RotateWeapon();

        if (Input.GetMouseButtonDown(0))
        {
            _weaponAnimator.SetTrigger("swing");
        }
    }

    private void RotateWeapon()
    {
        //Rotate weapon around player
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
        Vector2 aimDirection = _mousePosition - _rbPlayer.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;

        pointRotation.transform.localRotation = Quaternion.Euler(0, 0, aimAngle);
        float anglePointRotation = pointRotation.transform.eulerAngles.z;
        if (_isFlipWeapon)
        {
            if (!player.GetComponent<SpriteRenderer>().flipX)
            {
                _isFlipWeapon = false;
            }
        }
        else
        {
            if (player.GetComponent<SpriteRenderer>().flipX)
            {
                _isFlipWeapon = true;
            }
        }

        FlipWeapon(_isFlipWeapon);
        
        if ((anglePointRotation <= 70f && anglePointRotation >= 0f) ||
            (anglePointRotation <= 360f && anglePointRotation >= 250f))
        {
            _spriteWeapon.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder - 1;
        }
        else
        {
            _spriteWeapon.sortingOrder = player.GetComponent<SpriteRenderer>().sortingOrder + 1;
        }
    }
    private void FlipWeapon(bool flip)
    {
        if (flip)
        {
            _weaponAnimator.SetBool("isFlip", true);
            _weaponAnimator.SetBool("leftSide", true);
            _spriteWeapon.flipX = true;
        }
        else
        {
            _weaponAnimator.SetBool("isFlip", false);
            _weaponAnimator.SetBool("leftSide", false);
            _spriteWeapon.flipX = false;
        }
    }
    
}
