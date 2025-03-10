using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    [SerializeField] PlayerSpacialDetector _playerSpacialDetector;
    [SerializeField] PlayerMovement _playerMovement;
    [SerializeField] Player _player;
    [SerializeField] PlayerHealth _playerHealth;
    [SerializeField] Jump _jump;
    [SerializeField] ProyectilePool _proyectilePool;
    [SerializeField] ObstacleManager _obstacleManager;
    [SerializeField] Animator _animator;
    [SerializeField] Attack _attack;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip stepSounds;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip throwSound;

    void Start()
    {
        _jump.InitializeReferences(_playerSpacialDetector, _playerMovement, _animator, audioSource, jumpSound);
        _playerMovement.InitializeReferences(_playerSpacialDetector, _animator);
        _playerHealth.InitializeReferences(_animator);
        _player.InitializeReferences(_playerMovement, _playerHealth, stepSounds, audioSource);
        _attack.InitializeReferences(_animator, _playerMovement, _proyectilePool, audioSource, throwSound);
    }

    public void OnJump() {  _jump.OnJump(); }
    public void OnAttack() { _attack.PerformAttack(); }
}
