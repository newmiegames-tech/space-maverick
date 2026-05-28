using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private Vector3 _direction;
    [SerializeField] private float _speed;
    private Rigidbody _rb;

    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        _rb.Move(
            transform.position + _direction.normalized * _speed * Time.deltaTime,
            transform.rotation
        );
    }
}
