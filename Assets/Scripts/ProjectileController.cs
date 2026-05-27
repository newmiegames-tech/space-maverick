using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private Vector2 _direction;
    [SerializeField] private float _speed;

    void Update()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        transform.Translate(_direction.normalized * _speed * Time.deltaTime);
    }
}
