using System.Collections.Generic;
using UnityEngine;

public class HealthCounterController : MonoBehaviour
{
    public int Health {  get; private set; }
    [SerializeField] private List<GameObject> _healthCounters;
    [SerializeField] private GameObject _defeatDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManager.Instance != null)
        {
            Health = GameManager.Instance.StartingHealth;
        }
        else
        {
            // TODO: Throw exception, used for dev
            Health = 8;
        }

        // Initialize health counter
        AddHealth(0);
    }

    public void AddHealth(int value)
    {
        Health += value;

        // Health cannot go negative
        if (Health < 0)
            Health = 0;

        // Update health display
        for (int i = 0; i < _healthCounters.Count; i++)
        {
            _healthCounters[i].SetActive(i < Health);
        }
        
        // Health all gone
        if (Health == 0)
        {            
            // End game, defeat
            GameManager.Instance.EndGame();
            _defeatDisplay.SetActive(true);
        }
    }
}
