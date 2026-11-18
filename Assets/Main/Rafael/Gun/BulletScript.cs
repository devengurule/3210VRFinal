using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] private float bulletSpeed;

    private void Start()
    {
        StartCoroutine(TimeOut());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "NPC") //Name is temporary just in case either of you are using a tag
        {
            Destroy(collision.gameObject);
        }

        StopAllCoroutines();
        Destroy(gameObject);
    }

    private IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }
}
