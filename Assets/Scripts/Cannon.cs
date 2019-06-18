using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField]
    private float distance;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float cooldown;

    [Space(10)]
    [SerializeField]
    private Transform spawn;
    [SerializeField]
    private Projectile projectile;

    private bool isReady = true;
    private void Ready() => isReady = true;

    public void Shoot()
    {
        if (isReady)
        {
            isReady = false;
            Invoke("Ready", cooldown);

            Instantiate(this.projectile, spawn.position, spawn.rotation)
                .Launch(distance, speed);
        }
    }

    public bool IsInRange(Vector3 point) =>
        (transform.position - point).sqrMagnitude <= distance * distance;
}
