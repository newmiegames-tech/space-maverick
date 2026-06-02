using UnityEngine;

public class SimpleEnemyController : EnemyController
{
    // POLYMORPHISM: Each enemy has a unique controller that implements its unique movement
    protected override void ApplyMovement()
    {
        Vector3 forwardMovement = transform.rotation * (Vector3.up * _moveSpeed);
        _rb.Move(transform.position + forwardMovement, transform.rotation);
    }

    protected override void FireProjectile()
    {
        Instantiate(_projectilePrefab, transform.position, transform.rotation);
    }
}
