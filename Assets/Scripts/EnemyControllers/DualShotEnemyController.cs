using System.Collections.Generic;
using UnityEngine;

public class DualShotEnemyController : EnemyController
{
    // Flight path
    [SerializeField] private List<Vector3> _pathNodes;
    [SerializeField] private List<float> _pathNodeDurations;
    private int pathIndex;
    private float pathStartTime;

    // Projectiles
    [SerializeField] private Vector3 _leftProjectileOffset;
    [SerializeField] private Vector3 _rightProjectileOffset;

    private void Awake()
    {
        pathIndex = -1;
    }

    protected override void ApplyMovement()
    {
        // Start of first path node
        if (pathIndex == -1)
        {
            pathIndex = 0;
            pathStartTime = Time.time;
        }

        // No more path nodes to traverse to
        if (pathIndex >= _pathNodes.Count - 1 || pathIndex >= _pathNodeDurations.Count)
            return;

        float pathDuration = _pathNodeDurations[pathIndex];
        float pathTime = Time.time - pathStartTime;

        // Start of next path
        if (pathTime > pathDuration)
        {
            pathIndex++;
            pathStartTime = Time.time;
        }
        else
        {
            // Interpolate between path nodes to find position
            float t = pathTime / pathDuration;
            Vector3 p0 = _pathNodes[pathIndex];
            Vector3 p1 = _pathNodes[pathIndex + 1];
            Vector3 interpolated = (1 - t) * p0 + t * p1;
            _rb.Move(interpolated, transform.rotation);
        }
    }

    protected override void FireProjectile()
    {
        // Fire left and right projectiles simultaneously
        Instantiate(_projectilePrefab, transform.position + _leftProjectileOffset, transform.rotation);
        Instantiate(_projectilePrefab, transform.position + _rightProjectileOffset, transform.rotation);
    }
}
