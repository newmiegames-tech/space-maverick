using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
    private bool isInvulnerable;

    private Rigidbody _rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        isInvulnerable = false;
        _rb = GetComponent<Rigidbody>();        
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
        _rb.Move(transform.position + movement.normalized * _moveSpeed * Time.deltaTime, transform.rotation);
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
            
            // Enable invulnerability for a short period
            StartCoroutine(nameof(WaitInvulnerabilityCooldown));
        }
    }

    IEnumerator WaitInvulnerabilityCooldown()
    {
        isInvulnerable = true;
        yield return new WaitForSeconds(_invulnerabilityCooldown);
        isInvulnerable = false;
    }
}
