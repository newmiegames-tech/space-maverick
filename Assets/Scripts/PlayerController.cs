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
    [SerializeField] float _attackCooldown;
    [SerializeField] GameObject _projectilePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
        transform.Translate(movement.normalized * _moveSpeed * Time.deltaTime);
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
}
