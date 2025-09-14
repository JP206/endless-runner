using System.Collections;
using UnityEngine;

public class RevivePlayer : MonoBehaviour
{
    [SerializeField] GameObject revivePlatform;
    [SerializeField] GameObject saveMeCanvas;

    GameObject player;
    Vector3 playerPos = new(-4.2f, 2.71f, 0);
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

        StartCoroutine(ReviveWithDelay());
    }


    IEnumerator PlatformTime()
    {
        yield return new WaitForSeconds(4);
        revivePlatform.SetActive(false);

        player.GetComponent<PlayerHealth>().SetExternalInvulnerability(false);
    }

    IEnumerator PlatformTimeInput()
    {
        yield return new WaitForSeconds(0.5f);
        revivePlatform.SetActive(false);

        player.GetComponent<PlayerHealth>().SetExternalInvulnerability(false);
    }

    IEnumerator ReviveWithDelay()
    {
        yield return null;

        Collider2D platCollider = revivePlatform.GetComponent<Collider2D>();
        if (platCollider != null)
        {
            Bounds bounds = platCollider.bounds;
            float newY = bounds.max.y + 0.5f;
            player.transform.position = new Vector3(revivePlatform.transform.position.x, newY, 0f);
        }

        foreach (var col in player.GetComponentsInChildren<Collider2D>())
        {
            col.enabled = true;
        }

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        player.GetComponent<Animator>().SetTrigger("revive");
        player.GetComponent<PlayerHealth>().Revive();
        player.GetComponent<PlayerHealth>().SetExternalInvulnerability(true);

        saveMeCanvas.SetActive(false);
        saveMeCanvas.transform.parent.transform.parent.gameObject.SetActive(false);

        FindFirstObjectByType<PlayerOutOfBounds>()?.ResetGameOverTriggered();

        StartCoroutine(PlatformTime());
    }
}
