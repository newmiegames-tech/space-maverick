using System.Collections;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    // Movement
    [SerializeField] protected float _moveSpeed;

    // Projectile
    private bool _isAttackCoolingDown;
    [SerializeField] private float _attackCooldown;
    [SerializeField] protected GameObject _projectilePrefab;
    [SerializeField] private Vector2 _xAttackBounds;
    [SerializeField] private Vector2 _yAttackBounds;

    // Hit Points
    [SerializeField] private int _hitPoints;

    // Explosion and destruction
    [SerializeField] private ParticleSystem _explosion;
    [SerializeField] private float _destroyDelay;

    // Score
    private ScoreboardController _scoreboardController;
    [SerializeField] private int _score;

    protected Rigidbody _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _scoreboardController = GameObject.Find("Scoreboard").GetComponent<ScoreboardController>();
        _rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMovement();
        CheckAttack();
    }

    protected abstract void ApplyMovement();

    private void CheckAttack()
    {
        // Cooling down
        if (_isAttackCoolingDown)
            return;

        // Outside attack boundary
        if (transform.position.x < _xAttackBounds.x)
            return;

        if (transform.position.x > _xAttackBounds.y)
            return;

        if (transform.position.y < _yAttackBounds.x)
            return;

        if (transform.position.y > _yAttackBounds.y)
            return;

        _isAttackCoolingDown = true;

        FireProjectile();

        StartCoroutine(nameof(WaitAttackCooldown));
    }

    IEnumerator WaitAttackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);

        _isAttackCoolingDown = false;
    }

    protected abstract void FireProjectile();

    private void OnTriggerEnter(Collider other)
    {
        bool tookHit = false;

        // Hit by projectile
        if (other.name.StartsWith("Projectile"))
        {
            tookHit = true;

            // Destroy projectile
            Destroy(other.gameObject);
        }

        // Hit by player ship
        if (other.name.StartsWith("Player"))
        {
            tookHit = true;
        }

        // Lose a hitpoint
        if (tookHit && _hitPoints > 0)
        {
            _hitPoints -= 1;

            // Destroy the ship if hit points exhausted
            if (_hitPoints <= 0)
            {
                // Add to scoreboard
                _scoreboardController.AddScore(_score);

                // Play an explosion
                _explosion.Play();

                // Destroy the object after letting explosion effect play for a bit
                StartCoroutine(nameof(WaitDestroyDelay));
            }
        }
    }

    IEnumerator WaitDestroyDelay()
    {
        yield return new WaitForSeconds(_destroyDelay);

        Destroy(gameObject);
    }
}
