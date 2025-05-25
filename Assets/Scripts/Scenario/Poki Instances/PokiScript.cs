using UnityEngine;
using System.Collections;

public class PokiScript : MonoBehaviour
{
    private static bool instanceExists = false;
    public static PokiScript Instance;

    void Awake()
    {
        if (!instanceExists)
        {
            DontDestroyOnLoad(gameObject);
            instanceExists = true;
            Instance = this;

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

    public void CallGameplayStart()
    {
        Debug.Log("▶️ Poki: gameplayStart()");
        PokiUnitySDK.Instance.gameplayStart();
    }

    public void CallGameplayStop()
    {
        Debug.Log("⏸ Poki: gameplayStop()");
        PokiUnitySDK.Instance.gameplayStop();
    }

    public void CallRewardedBreak(System.Action<bool> callback)
    {
        Debug.Log("🎬 Poki: rewardedBreak() triggered");

        PokiUnitySDK.Instance.rewardedBreakCallBack = new PokiUnitySDK.RewardedBreakDelegate((bool withReward) =>
        {
            MuteAllAudio(false);
            callback(withReward);
        });

        StartCoroutine(DelayMuteAndStartAd());
    }



    public void MuteAllAudio(bool mute)
    {
        AudioListener.volume = mute ? 0f : 1f;
    }
    private IEnumerator DelayMuteAndStartAd()
    {
        yield return new WaitForEndOfFrame();

        MuteAllAudio(true);
        PokiUnitySDK.Instance.rewardedBreak();
    }

}
