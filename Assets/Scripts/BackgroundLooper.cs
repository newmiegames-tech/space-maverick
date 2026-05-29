using UnityEngine;

public class BackgroundLooper : MonoBehaviour
{
    private Vector3 _startPos;
    private float _repeatHeight;
    [SerializeField] private float _movementSpeed;

    void Start()
    {
        // Store initial position to loop back to
        _startPos = transform.position;

        // Background is tiled 3 high, so repeat after a third
        _repeatHeight = GetComponent<SpriteRenderer>().size.y / 3;
    }

    private void Update()
    {
        // Move background down
        transform.Translate(Vector3.down * _movementSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        // If background has moved below repeat point, loop back to starting position
        if (transform.position.y < _startPos.y - _repeatHeight)
        {
            Debug.Log("Looped");
            transform.position = _startPos;
        }
    }
}
