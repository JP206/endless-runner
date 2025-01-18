using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] SceneInitializer _sceneInitializer;

    private void Update()
    {
        DetectInputs();
    }

    void DetectInputs()
    {
        //Jump Input
        if (Input.GetButtonDown("Jump"))
        {
            _sceneInitializer.OnJump();
        }

        //Proyectile Input
        if (Input.GetButtonDown("Shoot"))
        {
            _sceneInitializer.OnAttack();
        }
    }
}
