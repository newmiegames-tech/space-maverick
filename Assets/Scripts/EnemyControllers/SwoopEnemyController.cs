using UnityEngine;

enum SwoopState
{
    None,
    Swooping,
    Finished
}

public class SwoopEnemyController : EnemyController
{
    [SerializeField] private float _swoopDuration;
    private SwoopState _swoopState;
    private float _swoopStartTime;
    private Vector3 _bezierStart;
    private Vector3 _bezierControl;
    private Vector3 _bezierEnd;

    private void Awake()
    {
        _swoopState = SwoopState.None;
    }

    protected override void ApplyMovement()
    {
        // Move normally starting swoop
        if (_swoopState != SwoopState.Swooping)
        {
            Vector3 forwardMovement = transform.rotation * (Vector3.up * _moveSpeed * Time.deltaTime);
            _rb.Move(transform.position + forwardMovement, transform.rotation);
        }
        else
        {
            // During swoop bezier curve controls movement
            float swoopTime = Time.time - _swoopStartTime;
            if (swoopTime < _swoopDuration)
            {
                _rb.Move(CalculateSwoopPosition(swoopTime), transform.rotation);
            }
            // Swoop concludes after specified duration
            else
            {
                _swoopState = SwoopState.Finished;
            }
        }

    }

    protected override void FireProjectile()
    {
        Instantiate(_projectilePrefab, transform.position, transform.rotation);

        
        // Swoop after first projectile fired
        if (_swoopState == SwoopState.None)
        {
            _swoopState= SwoopState.Swooping;

            SetupSwoop();
        }
    }

    private void SetupSwoop()
    {
        _swoopStartTime = Time.time;

        // Initialize bezier curve points
        // Start point is current position
        _bezierStart = transform.position;
        // Control point is start point projected forward for swoop duration
        Vector3 forwardProjection = transform.position + transform.rotation * (Vector3.up * _moveSpeed * _swoopDuration);
        _bezierControl = forwardProjection;
        // End point is start point projected forward and x co-ordinated negated
        forwardProjection.x = -forwardProjection.x;
        _bezierEnd = forwardProjection;
        //Debug.Log("p0: " + _bezierStart + ", p1: " + _bezierControl + ", p2: " + _bezierEnd);
    }

    private Vector3 CalculateSwoopPosition(float swoopTime)
    {
        // Bezier curve quadratic interpoliation formula
        Vector3 p0 = _bezierStart;
        Vector3 p1 = _bezierControl;
        Vector3 p2 = _bezierEnd;
        float t = swoopTime / _swoopDuration;
        Vector3 p = ((1 - t) * (1 - t) * p0) + (2 * (1 - t) * t * p1) + (t * t * p2);
        return p;
    }
}
