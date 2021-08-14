using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerCarController : MonoBehaviour
{
    public Rigidbody sphereRigidbody;
    public RoadGenerator roadGenerator;
    public GameObject smokeObject;
    public AudioSource audioSource;
    public GameObject policeExplosionObject;

    public float forwardAcceleration = 8f;
    public float reverseAcceleration = 4f;
    public float maxSpeed = 50f;
    public float turnStrength = 180f;
    public float gravityForce = 10f;
    public float dragOnGround = 3f;

    private float speedInput;
    private float turnInput;

    private bool grounded;

    public LayerMask whatIsGround;
    public float groundRayLength = 0.1f;
    public Transform groundRayPoint;

    public Transform leftFrontWheel;
    public Transform rightFrontWheel;
    public float maxWheelTurn = 25f;

    public int policeDestroyed = 0;
    public int animalsHit = 0;

    public int timer = 61;
    public bool gameOver = false;

    private void Start()
    {
        sphereRigidbody.transform.parent = null;
        audioSource = GetComponent<AudioSource>();

        InvokeRepeating("DecreaseTimer", 0f, 1f);
        //MessageBoxController.SayMessage(policeDestroyed.ToString());
    }

    private void Update()
    {
        if (!gameOver)
        {
            speedInput = 0f;

            float verticalInput = Input.GetAxisRaw("Vertical");
            float horizontalInput = Input.GetAxisRaw("Horizontal");

            if (verticalInput > 0)
            {
                speedInput = verticalInput * forwardAcceleration * 1000f;
            }
            else if (verticalInput < 0)
            {
                speedInput = verticalInput * reverseAcceleration * 1000f;
            }

            if (verticalInput != 0 || horizontalInput != 0)
            {
                smokeObject.SetActive(true);
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
            else
            {
                smokeObject.SetActive(false);
                audioSource.Stop();
            }

            turnInput = horizontalInput;

            if (grounded)
            {
                transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, turnInput * turnStrength * Time.deltaTime * verticalInput, 0f));
            }

            leftFrontWheel.localRotation = Quaternion.Euler(leftFrontWheel.localRotation.eulerAngles.x, (turnInput * maxWheelTurn) - 180f, leftFrontWheel.localRotation.eulerAngles.z);
            rightFrontWheel.localRotation = Quaternion.Euler(rightFrontWheel.localRotation.eulerAngles.x, turnInput * maxWheelTurn, rightFrontWheel.localRotation.eulerAngles.z);

            transform.position = sphereRigidbody.transform.position;
        }
    }

    private void FixedUpdate()
    {
        grounded = false;
        if (Physics.Raycast(groundRayPoint.position, -transform.up, out RaycastHit hit, groundRayLength, whatIsGround))
        {
            grounded = true;

            transform.rotation = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
        }

        if (grounded)
        {
            sphereRigidbody.drag = dragOnGround;
            if (Mathf.Abs(speedInput) > 0)
            {
                sphereRigidbody.AddForce(transform.forward * speedInput);
            }
        }
        else
        {
            sphereRigidbody.drag = 0.1f;
            sphereRigidbody.AddForce(Vector3.up * -gravityForce * 100f);
        }
    }

    private void DecreaseTimer()
    {
        if (timer > 0)
        {
            timer--;
            MessageBoxController.SayMessage(timer.ToString());
        }
        else
        {
            gameOver = true;
            audioSource.Stop();
            smokeObject.SetActive(false);

            MessageBoxController.SayMessage((policeDestroyed * animalsHit).ToString());
            Time.timeScale = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PoliceCar"))
        {
            float randomRotation = Random.Range(0, 360);
            Vector3 explosionLoc = new Vector3(other.transform.position.x, 0.0f, other.transform.position.z);

            Instantiate(policeExplosionObject, explosionLoc, Quaternion.Euler(0, randomRotation, 0));

            Destroy(other.gameObject);
            policeDestroyed++;

            //MessageBoxController.SayMessage(policeDestroyed.ToString());

            if (roadGenerator != null)
            {
                StartCoroutine(roadGenerator.SpawnPolice());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Animal"))
        {
            animalsHit++;
        }
    }
}