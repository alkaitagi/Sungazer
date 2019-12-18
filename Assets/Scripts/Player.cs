using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private PlayerRemnant remnant;

    [Header("Movement")]
    [SerializeField]
    private Engine mainEngine;
    [SerializeField]
    private Engine leftTorque;
    [SerializeField]
    private Engine rightTorque;
    [SerializeField]
    private float rotationSpeed;

    [Header("Modules")]
    [SerializeField]
    private LightSwitch[] lights;
    [SerializeField]
    private Thruster thruster;
    [SerializeField]
    private Cannon cannon;

    public static Player Main { get; private set; }
    public static Vector3 Position => Main.transform.position;

    private void Awake()
    {
        Main = this;

        foreach (var light in lights)
            light.gameObject.SetActive(ModuleManager.Main.HasLight);
        thruster.gameObject.SetActive(ModuleManager.Main.HasThruster);
        cannon.gameObject.SetActive(ModuleManager.Main.HasCannon);

        CameraManager.VirtualCamera.Follow = transform;
    }

    public void SendRemnant() => Instantiate(remnant, transform.position, Quaternion.identity);

    private void Update()
    {
        mainEngine.IsOn = Input.GetKey("w");
        leftTorque.IsOn = Input.GetKey("a");
        rightTorque.IsOn = Input.GetKey("d");

        transform.rotation = Quaternion.Euler
        (
            0,
            0,
            Mathf.LerpAngle
            (
                transform.eulerAngles.z,
                -Vector2.SignedAngle
                (
                    CameraManager.MouseWorld() - transform.position,
                    Vector3.up
                ),
                rotationSpeed * Time.deltaTime
            )
        );

        if (Input.GetKeyDown("space"))
            if (thruster.gameObject.activeInHierarchy)
                thruster.Burst();
            else if (cannon.gameObject.activeInHierarchy)
                cannon.Shoot();
            else
                foreach (var light in lights)
                    if (light.gameObject.activeInHierarchy)
                        light.ToggleActive();
    }

    private void OnDisable()
    {
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        mainEngine.IsOn = false;
        leftTorque.IsOn = false;
        rightTorque.IsOn = false;
    }
}
