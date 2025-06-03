using UnityEngine;

public class DinoEvent : MonoBehaviour
{
    public void TriggerCameraShake()
    {
        if (CameraShake.Instance != null)
            CameraShake.Instance.TriggerShake();
    }
}
