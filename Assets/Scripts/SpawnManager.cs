using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

enum ShipType
{
    Simple,
    TripleShot,
    Swoop,
    Tracker,
    DualShot,
    ScatterShot
}

[Serializable]
class SpawnShip
{
    public Vector3 Position;
    public ShipType Type;
}

[Serializable]
class SpawnWave
{
    public List<SpawnShip> Ships;
    public float Delay;
}

[Serializable]
class SpawnWaveList
{
    public List<SpawnWave> Waves;
}


public class SpawnManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> _shipPrefabs;
    [SerializeField] private string _wavesFile;
    [SerializeField] private int _waveIndex;
    private float _enemiesRemainCheckDelay;
    private SpawnWaveList _waves;
    [SerializeField] private GameObject _victoryDisplay;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadWaves();
        StartCoroutine(nameof(DoSpawnWaves));
    }

    private void LoadWaves()
    {
        // Load the waves json file into the in memory waves object
        string path = Path.Combine(Application.streamingAssetsPath, _wavesFile);
        string json = File.ReadAllText(path);
        _waves = JsonUtility.FromJson<SpawnWaveList>(json);
    }

    IEnumerator DoSpawnWaves()
    {
        // Spawn waves until no more
        while (_waveIndex < _waves.Waves.Count)
        {
            // Spawn the wave
            SpawnWave wave = _waves.Waves[_waveIndex];
            DoSpawnWave(wave);

            // Wait for wave delay
            yield return new WaitForSeconds(wave.Delay);

            // Move on to next wave
            _waveIndex++;
        }

        // Wait until no more enemies remain
        bool doEnemiesRemain = true;
        while (doEnemiesRemain)
        {
            doEnemiesRemain = (GameObject.FindGameObjectWithTag("Enemy") != null);
            yield return new WaitForSeconds(0.5f);
        }

        // End game, victory
        GameManager.Instance.EndGame();
        _victoryDisplay.SetActive(true);
    }

    private void DoSpawnWave(SpawnWave wave)
    {
        foreach (var item in wave.Ships)
        {
            // Spawn specified ship at the specified position
            GameObject shipPrefab = _shipPrefabs[(int)item.Type];
            Instantiate(shipPrefab, item.Position, shipPrefab.transform.rotation);
        }
    }
}
