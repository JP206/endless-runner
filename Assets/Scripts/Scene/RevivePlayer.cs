using System.Collections;
using UnityEngine;

public class RevivePlayer : MonoBehaviour
{
    [SerializeField] GameObject revivePlatform;
    [SerializeField] GameObject saveMeCanvas;

    GameObject player;
    Vector3 playerPos = new(-4.06f, 4.5f, 0);
    bool hasDied = false;

    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    void Update()
    {
        if (hasDied && (Input.GetButtonDown("Jump") || Input.GetButtonDown("Shoot")))
        {
            StopAllCoroutines();
            StartCoroutine(PlatformTimeInput());
            GetComponent<RevivePlayer>().enabled = false;
        }
    }

    public void revivePlayer()
    {
        Time.timeScale = 1;
        hasDied = true;

        revivePlatform.SetActive(true);
        player.transform.position = playerPos;

        saveMeCanvas.SetActive(false);
        saveMeCanvas.transform.parent.transform.parent.gameObject.SetActive(false);

        player.GetComponent<Animator>().SetTrigger("revive");

        player.GetComponent<PlayerHealth>().Revive();

        FindFirstObjectByType<PlayerOutOfBounds>()?.ResetGameOverTriggered();

        StartCoroutine(PlatformTime());
    }

    IEnumerator PlatformTime()
    {
        yield return new WaitForSeconds(4);
        revivePlatform.SetActive(false);
    }

    IEnumerator PlatformTimeInput()
    {
        yield return new WaitForSeconds(0.5f);
        revivePlatform.SetActive(false);
    }
}
