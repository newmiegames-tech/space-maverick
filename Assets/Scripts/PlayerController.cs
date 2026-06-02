using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Movement
    private Vector2 _moveInput;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private Vector2 _xBounds;
    [SerializeField] private Vector2 _yBounds;

    // Projectile
    private bool _isAttackCoolingDown;
    [SerializeField] private float _attackCooldown;
    [SerializeField] private GameObject _projectilePrefab;

    // Health
    [SerializeField] private HealthCounterController _healthCounterController;
    [SerializeField] private float _invulnerabilityCooldown;
    [SerializeField] private GameObject _defeatDisplay;
    [SerializeField] private ParticleSystem _explosion;
    private bool isInvulnerable;

    private Rigidbody _rb;

    private SfxManager _sfxManager;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isInvulnerable = false;
        _rb = GetComponent<Rigidbody>();

        // SFX manager wont exist on title scene
        GameObject sfxOb = GameObject.Find("SfxManager");
        if (sfxOb != null )
        {
            _sfxManager = sfxOb.GetComponent<SfxManager>();
        }        
    }

    // Update is called once per frame
    void Update()
    {
        ApplyMovement();
        ApplyBounds();
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    public void OnAttack()
    {
        if (_isAttackCoolingDown)
            return;

        FireProjectile();

        _isAttackCoolingDown = true;
        StartCoroutine(nameof(StartAttackCooldown));
    }

    IEnumerator StartAttackCooldown()
    {
        yield return new WaitForSeconds(_attackCooldown);
        _isAttackCoolingDown = false;
    }

    private void ApplyMovement()
    {
        Vector3 movement = Vector3.right * _moveInput.x + Vector3.up * _moveInput.y;
        _rb.Move(transform.position + movement.normalized * _moveSpeed, transform.rotation);
    }

    private void ApplyBounds()
    {
        Vector3 position = transform.position;

        if (position.x < _xBounds.x)
            position.x = _xBounds.x;
        else if (position.x > _xBounds.y)
            position.x = _xBounds.y;

        if (position.y < _yBounds.x)
            position.y = _yBounds.x;
        else if (position.y > _yBounds.y)
            position.y = _yBounds.y;

        transform.position = position;
    }

    private void FireProjectile()
    {
        Instantiate(_projectilePrefab, transform.position, transform.rotation);

        // Play sound effect for player projectile
        _sfxManager.Play(SfxType.PlayerShoot);
    }

    private void OnTriggerEnter(Collider other)
    {
        bool tookHit = false;

        // Hit by projectile
        if (other.name.StartsWith("EnemyProjectile"))
        {
            tookHit = true;

            // Destroy projectile
            Destroy(other.gameObject);
        }

        // Hit by enemy ship
        if (other.CompareTag("Enemy"))
        {
            tookHit = true;
        }

        // Lose a hitpoint
        if (tookHit && !isInvulnerable)
        {
            _healthCounterController.AddHealth(-1);

            if (_healthCounterController.Health > 0)
            {
                // Play sound effect for getting hit
                _sfxManager.Play(SfxType.PlayerHit);
            }
            else
            {
                // Play explosion visual and sound effects
                _sfxManager.Play(SfxType.PlayerExplode);
                _explosion.Play();
            }

            // Enable invulnerability for a short period
            StartCoroutine(nameof(WaitInvulnerabilityCooldown));

            // Health all gone
            if (_healthCounterController.Health <= 0)
            {
                // End game, defeat                            
                gameObject.SetActive(false);
                _defeatDisplay.SetActive(true);
                GameManager.Instance.EndGame();
            }
        }
    }

    IEnumerator WaitInvulnerabilityCooldown()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(_invulnerabilityCooldown);
        isInvulnerable = false;
    }
}
