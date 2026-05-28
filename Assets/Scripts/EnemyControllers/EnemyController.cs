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

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
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
}
