using UnityEngine;

public class CollectibleObject : MonoBehaviour
{
    private bool collected = false;

    public void ResetState()
    {
        collected = false;
        gameObject.SetActive(true);
    }
}
