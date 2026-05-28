using UnityEngine;

public class ProjectileController : MonoBehaviour
{
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
            transform.position + transform.rotation * (Vector3.up * _speed * Time.deltaTime),
            transform.rotation
        );
    }
}
