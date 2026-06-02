using System.Collections.Generic;
using UnityEngine;

public enum SfxType
{
    EnemyShoot,
    EnemyHit,
    EnemyExplode,
    PlayerShoot,
    PlayerHit,
    PlayerExplode
}

public class SfxManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> _enemyShootFx;
    [SerializeField] List<AudioClip> _enemyHitFx;
    [SerializeField] List<AudioClip> _enemyExplodeFx;
    [SerializeField] List<AudioClip> _playerShootFx;
    [SerializeField] List<AudioClip> _playerHitFx;
    [SerializeField] List<AudioClip> _playerExplodeFx;
    [SerializeField] AudioSource _audioSource;

    public void Play(SfxType sfx)
    {
        List<AudioClip> listSfx;
        if (sfx == SfxType.EnemyShoot)
        {
            listSfx = _enemyShootFx;
        }
        else if (sfx == SfxType.EnemyHit)
        {
            listSfx = _enemyHitFx;
        }
        else if (sfx == SfxType.EnemyExplode)
        {
            listSfx = _enemyExplodeFx;
        }
        else if (sfx == SfxType.PlayerShoot)
        {
            listSfx = _playerShootFx;
        }
        else if (sfx == SfxType.PlayerHit)
        {
            listSfx = _playerHitFx;
        }
        else //if (sfx == SfxType.PlayerExplode)
        {
            listSfx = _playerExplodeFx;
        }

        int index = Random.Range(0, listSfx.Count);
        _audioSource.PlayOneShot(listSfx[index]);
    }
}
