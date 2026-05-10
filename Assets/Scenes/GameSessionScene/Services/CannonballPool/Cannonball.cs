using UnityEngine;
using Zenject;

public class Cannonball : MonoBehaviour
{
    public float speed = 10f;
    private Vector3 direction;

    [SerializeField] private GameObject explosionPrefab;

    [Inject] private VfxService vfx;

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            vfx.PlayEffect(explosionPrefab, collision.transform.position);
            gameObject.SetActive(false);
        }
    }

    public void Init(Vector3 dir)
    {
        direction = dir.normalized;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
