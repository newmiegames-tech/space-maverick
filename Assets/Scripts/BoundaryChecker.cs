using UnityEngine;

public enum CleanupAction
{
    Deactivate,
    Destroy
}

public class BoundaryChecker : MonoBehaviour
{
    [SerializeField] private Vector2 _xBounds;
    [SerializeField] private Vector2 _yBounds;
    [SerializeField] private CleanupAction _cleanupAction;

    void Update()
    {
        CheckBounds();
    }

    private void CheckBounds()
    {
        bool doCleanup = false;

        // Check horizontal bounds
        if (transform.position.x < _xBounds.x)
            doCleanup = true;
        else if (transform.position.x > _xBounds.y)
            doCleanup = true;

        // Check vertical bounds
        if (transform.position.y < _yBounds.x)
            doCleanup = true;
        else if (transform.position.y > _yBounds.y)
            doCleanup = true;

        if (doCleanup)
        {
            // Perform configured cleanup action
            if (_cleanupAction == CleanupAction.Deactivate)
            {
                gameObject.SetActive(false);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
