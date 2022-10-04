using Unity.Mathematics;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private Rigidbody2D _rb;
    private Vector2 _moveDirection;
    private SpriteRenderer _rend;
    private Vector2 _mousePosition;

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

    }
}
