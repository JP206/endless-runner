using System.Collections;
using UnityEngine;

public class TRex : MonoBehaviour
{
    [SerializeField] float speed;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(StartAnim());
    }

    IEnumerator StartAnim()
    {
        yield return new WaitForSeconds(1);
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(0.7f);
        animator.SetTrigger("idle");

        float posX = transform.position.x - 15;

        while (transform.position.x > posX)
        {
            transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            yield return null;
        }
    }
   
}
