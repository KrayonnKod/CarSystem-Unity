using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public GameObject player;
    public Transform carSeat;
    public Camera mainCamera;
    public Camera carCamera;
    public TextMeshProUGUI speedText;
    public Image fKeyImage;
    public float interactionRange = 3f;

    private Rigidbody carRigidbody;
    private bool isDriving = false;

    public float moveSpeed = 10f;
    public float turnSpeed = 50f;
    public float nitroMultiplier = 2f;
    public float acceleration = 5f;
    public float brakingPower = 10f;

    private float currentSpeed = 0f;
    private float defaultMoveSpeed;

    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        carCamera.enabled = false;
        speedText.gameObject.SetActive(false);
        fKeyImage.gameObject.SetActive(false);
        defaultMoveSpeed = moveSpeed;
    }

    void Update()
    {
        if (!isDriving)
        {
            float distanceToCar = Vector3.Distance(player.transform.position, transform.position);

            if (distanceToCar < interactionRange)
            {
                fKeyImage.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.F))
                {
                    EnterCar();
                }
            }
            else
            {
                fKeyImage.gameObject.SetActive(false);
            }
        }
        else
        {
            Drive();

            if (Input.GetKeyDown(KeyCode.F))
            {
                ExitCar();
            }

            UpdateSpeedText();
        }
    }

    void EnterCar()
    {
        isDriving = true;
        player.SetActive(false);
        carCamera.enabled = true;
        mainCamera.enabled = false;
        speedText.gameObject.SetActive(true);
        fKeyImage.gameObject.SetActive(false);
    }

    void ExitCar()
    {
        isDriving = false;
        player.SetActive(true);
        player.transform.position = transform.position + transform.right * 2;
        carCamera.enabled = false;
        mainCamera.enabled = true;
        carRigidbody.velocity = Vector3.zero;
        currentSpeed = 0;
        speedText.gameObject.SetActive(false);
    }

    void Drive()
    {
        float moveInput = Input.GetAxis("Vertical");
        float turnInput = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.LeftShift))
        {
            moveSpeed = defaultMoveSpeed * nitroMultiplier;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
        }

        if (moveInput != 0)
        {
            carRigidbody.velocity = transform.forward * moveInput * moveSpeed;
        }
        else
        {
            carRigidbody.velocity = Vector3.Lerp(carRigidbody.velocity, Vector3.zero, Time.deltaTime * brakingPower);
        }

        transform.Rotate(0, turnInput * turnSpeed * Time.deltaTime, 0);
    }

    void UpdateSpeedText()
    {
        float carSpeed = carRigidbody.velocity.magnitude * 3.6f;
        speedText.text = "Hız: " + Mathf.RoundToInt(carSpeed).ToString() + " km/h";
    }
}
