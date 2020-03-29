using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class XRPlayerController : MonoBehaviour
{
    [Header("Behaviour Options")]

    [SerializeField]
    private float speed = 10.0f;

    [SerializeField]
    private float jumpForce = 250.0f;

    [SerializeField]
    private XRNode controllerNode = XRNode.LeftHand;

    [SerializeField]
    private bool checkForGroundOnJump = true;

    [Header("Capsule Collider Options")]
    [SerializeField]
    private Vector3 capsuleCenter = new Vector3(0, 1, 0);

    [SerializeField]
    private float capsuleRadius = 0.3f;

    [SerializeField]
    private float capsuleHeight = 1.6f;

    [SerializeField]
    private CapsuleDirection capsuleDirection = CapsuleDirection.YAxis;

    private InputDevice controller;

    private bool isGrounded;

    private bool buttonPressed;

    private Rigidbody rigidBodyComponent;

    private CapsuleCollider capsuleCollider;

    private List<InputDevice> devices = new List<InputDevice>();

    public enum CapsuleDirection
    {
        XAxis,
        YAxis,
        ZAxis
    }

    void OnEnable()
    {
        rigidBodyComponent = GetComponent<Rigidbody>();
        capsuleCollider = GetComponent<CapsuleCollider>();

        rigidBodyComponent.constraints = RigidbodyConstraints.FreezeRotation;
        capsuleCollider.direction = (int)capsuleDirection;
        capsuleCollider.radius = capsuleRadius;
        capsuleCollider.center = capsuleCenter;
        capsuleCollider.height = capsuleHeight;
    }

    void Start()
    {
        GetDevice();
    }

    private void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(controllerNode, devices);
        controller = devices.FirstOrDefault();
    }

    void Update()
    {
        if (controller == null)
        {
            GetDevice();
        }

        UpdateMovement();

        UpdateJump(controller);
    }

    private void UpdateMovement()
    {
        Vector2 primary2dValue;

        InputFeatureUsage<Vector2> primary2DVector = CommonUsages.primary2DAxis;

        if (controller.TryGetFeatureValue(primary2DVector, out primary2dValue) && primary2dValue != Vector2.zero)
        {
            Debug.Log("primary2DAxisClick is pressed " + primary2dValue);

            var xAxis = primary2dValue.x * speed * Time.deltaTime;
            var zAxis = primary2dValue.y * speed * Time.deltaTime;

            Vector3 right = transform.TransformDirection(Vector3.right);
            Vector3 forward = transform.TransformDirection(Vector3.forward);

            transform.position += right * xAxis;
            transform.position += forward * zAxis;
        }
    }

    private void UpdateJump(InputDevice controller)
    {
        isGrounded = (Physics.Raycast((new Vector2(transform.position.x, transform.position.y + 2.0f)), Vector3.down, 5.0f));

        Debug.DrawRay((new Vector3(transform.position.x, transform.position.y, transform.position.z)), Vector3.down, Color.red, 1.0f);

        if (!isGrounded && checkForGroundOnJump)
            return;

        bool buttonValue;

        if (controller.TryGetFeatureValue(CommonUsages.primaryButton, out buttonValue) && buttonValue)
        {
            if (!buttonPressed)
            {
                Debug.Log("primaryButton is pressed " + buttonValue);
                buttonPressed = true;
                rigidBodyComponent.AddForce(Vector3.up * jumpForce);
            }
        }
        else if (buttonPressed)
        {
            Debug.Log("primaryButton is released " + buttonValue);
            buttonPressed = false;
        }
    }
}
