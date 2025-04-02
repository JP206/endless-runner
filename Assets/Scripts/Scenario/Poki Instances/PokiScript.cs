using UnityEngine;

public class PokiScript : MonoBehaviour
{
    private static bool instanceExists = false;

    void Awake()
    {
        if (!instanceExists)
        {
            DontDestroyOnLoad(gameObject);
            instanceExists = true;

            PokiUnitySDK.Instance.sdkInitializedCallback = OnPokiSDKReady;
            PokiUnitySDK.Instance.init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnPokiSDKReady()
    {
        Debug.Log("✅ Poki SDK listo y activo en todas las escenas.");
    }
}
