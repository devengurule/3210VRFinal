using UnityEngine;

public class GunFireScript : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private ParticleSystem fireEffect;

    [Header("Values")]
    [SerializeField] private float bulletForce = 10;

    public void FireBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        bullet.transform.position = bulletSpawnPoint.position;
        bullet.GetComponent<Rigidbody>().AddForce(bulletSpawnPoint.forward * bulletForce, ForceMode.Impulse);

        if (fireEffect != null) fireEffect.Play();
    }
}
