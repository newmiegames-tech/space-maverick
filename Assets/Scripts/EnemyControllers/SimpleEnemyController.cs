using UnityEngine;

public class SimpleEnemyController : EnemyController
{
    protected override void ApplyMovement()
    {
        Vector3 forwardMovement = transform.rotation * (Vector3.up * _moveSpeed * Time.deltaTime);
        _rb.Move(transform.position + forwardMovement, transform.rotation);
    }

    protected override void FireProjectile()
    {
        Instantiate(_projectilePrefab, transform.position, transform.rotation);
    }
}
