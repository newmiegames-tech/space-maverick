using UnityEngine;

public class TripleShotEnemyController : EnemyController
{
    protected override void ApplyMovement()
    {
        _rb.Move(transform.position + Vector3.down * _moveSpeed * Time.deltaTime, transform.rotation);
    }

    protected override void FireProjectile()
    {
        Instantiate(_projectilePrefab, transform.position, transform.rotation);
        Instantiate(_projectilePrefab, transform.position, transform.rotation * Quaternion.Euler(0, 0, 45f));
        Instantiate(_projectilePrefab, transform.position, transform.rotation * Quaternion.Euler(0, 0, -45f));
    }
}
