using UnityEngine;

public class TripleShotEnemyController : EnemyController
{
    protected override void ApplyMovement()
    {
        Vector3 forwardMovement = transform.rotation * (Vector3.up * _moveSpeed);
        _rb.Move(transform.position + forwardMovement, transform.rotation);
    }

    protected override void FireProjectile()
    {
        Instantiate(_projectilePrefab, transform.position, transform.rotation);
        Instantiate(_projectilePrefab, transform.position, transform.rotation * Quaternion.Euler(0, 0, 45f));
        Instantiate(_projectilePrefab, transform.position, transform.rotation * Quaternion.Euler(0, 0, -45f));
    }
}
