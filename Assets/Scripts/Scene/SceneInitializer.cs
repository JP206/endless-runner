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

    void Start()
    {
        _jump.InitializeReferences(_playerSpacialDetector, _playerMovement, _animator);
        _playerMovement.InitializeReferences(_playerSpacialDetector, _animator);
        _playerHealth.InitializeReferences(_animator);
        _player.InitializeReferences(_playerMovement, _playerHealth);
    }

    public void OnJump() {  _jump.OnJump(); }
    public void OnAttack()
    {
        ShootProyectile();
    }

    private void ShootProyectile()
    {
        GameObject proyectile = _proyectilePool.GetPooledObject();
        if (proyectile)
        {
            proyectile.transform.position = _player.transform.position;
        }
    }

}
