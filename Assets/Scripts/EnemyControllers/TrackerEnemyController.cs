using UnityEngine;

public class TrackerEnemyController : EnemyController
{
    [SerializeField] private float trackSpeed;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected override void ApplyMovement()
    {
        Vector3 trackMovement = Vector3.zero;        
        if (player.transform.position.x > transform.position.x)
        {
            trackMovement = Vector3.right * trackSpeed * Time.deltaTime;
        }
        else if (player.transform.position.x < transform.position.x)
        {
            trackMovement = Vector3.left * trackSpeed * Time.deltaTime;
        }

        Vector3 forwardMovement = transform.rotation * (Vector3.up * _moveSpeed * Time.deltaTime);
        _rb.Move(transform.position + forwardMovement + trackMovement, transform.rotation);
    }

    protected override void FireProjectile()
    {
        Instantiate(_projectilePrefab, transform.position, transform.rotation);
    }
}
