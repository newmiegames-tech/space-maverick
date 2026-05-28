using System.Collections.Generic;
using UnityEngine;

public class ScatterShotEnemyController : EnemyController
{
    private int _scatterIndex;
    [SerializeField] private List<float> _scatterAngles;

    private void Awake()
    {
        _scatterIndex = Random.Range(0, _scatterAngles.Count);
    }

    protected override void ApplyMovement()
    {
        Vector3 forwardMovement = transform.rotation * (Vector3.up * _moveSpeed * Time.deltaTime);
        _rb.Move(transform.position + forwardMovement, transform.rotation);
    }

    protected override void FireProjectile()
    {
        // Resolve firing angle
        float angle = _scatterAngles[_scatterIndex++];
        if (_scatterIndex >= _scatterAngles.Count)
            _scatterIndex = 0;

        // Fire projectile
        Instantiate(_projectilePrefab, transform.position, transform.rotation * Quaternion.Euler(0, 0, angle));
    }
}
