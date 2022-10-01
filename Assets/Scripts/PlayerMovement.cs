using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private GameObject playerCursor;
    [SerializeField] private GameObject pointRotation;
    [SerializeField] private GameObject weapon;
    
    private Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private SpriteRenderer _rend;
    private Vector2 _mousePosition;
    private bool _flipWeapon = false;
    private bool _isRun;
    private Animator _animator;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _rend = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        InputProcess();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void InputProcess()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");
        float verticalMove = Input.GetAxisRaw("Vertical");

        _animator.SetFloat("speed", math.abs(horizontalMove)+math.abs(verticalMove));

        _moveDirection = new Vector2(horizontalMove, verticalMove).normalized;
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        
       
    }

    // ReSharper disable Unity.PerformanceAnalysis
    private void Move()
    {
        _rb.velocity = new Vector2(_moveDirection.x * moveSpeed, _moveDirection.y * moveSpeed);
        
        
        //Rotate sprite character
        if (_mousePosition.x < _rb.position.x)
        {
            _rend.flipX = true;
        }
        else
        {
            _rend.flipX = false;
        }

        //Rotate weapon around player
        Vector2 aimDirection = _mousePosition - _rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;

        pointRotation.transform.localRotation = Quaternion.Euler(0, 0, aimAngle);
        

        if (_flipWeapon)
        {
            if (pointRotation.transform.eulerAngles.z < 330 && pointRotation.transform.eulerAngles.z > 200)
            {
                _flipWeapon = false;
            }
        }
        else
        {
            if (pointRotation.transform.eulerAngles.z > 20f && pointRotation.transform.eulerAngles.z < 160f)
            {
                _flipWeapon = true;
            }
        }

        FlipWeapon(_flipWeapon);
        
    }

    private void FlipWeapon(bool flip)
    {
        if (flip)
        {
            weapon.transform.localRotation = Quaternion.Euler(0, 0, -45);
            weapon.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            weapon.transform.localRotation = Quaternion.Euler(0, 0, 45);
            weapon.GetComponent<SpriteRenderer>().flipX = false;
        }
    }
}
