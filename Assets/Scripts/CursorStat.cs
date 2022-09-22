using UnityEngine;

public class CursorStat : MonoBehaviour
{
    [SerializeField] private Sprite[] cursors;
    [SerializeField] private int selectedCursor;


    private Vector2 _cursorPos;
    private SpriteRenderer _rend;

    void Start()
    {
        Cursor.visible = false;
        _rend = GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        // Position cursor is mousePosition
        _cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = _cursorPos;
        _rend.sprite = cursors[selectedCursor];
        
    }
}
