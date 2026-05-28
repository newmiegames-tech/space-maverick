using UnityEngine;

public class SimpleEnemyController : EnemyController
{
    protected override void ApplyMovement()
    {
        transform.Translate(Vector3.up * _moveSpeed * Time.deltaTime);
    }

    protected override void FireProjectile()
    {
        Instantiate(_projectilePrefab, transform.position, transform.rotation);
    }
}
