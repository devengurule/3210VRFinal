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
        StopAllCoroutines();
        Destroy(gameObject);
    }

    private IEnumerator TimeOut()
    {
        yield return new WaitForSeconds(5);

        Destroy(gameObject);
    }
}
