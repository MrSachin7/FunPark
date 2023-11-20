using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class FlyingManager : MonoBehaviour
{
    [SerializeField] private VehicleEnterManager vehicleEnterManager;

    [SerializeField] private float responsiveness = 10f;
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] private float throttleAmount = 25f;


    [SerializeField] private float rotorSpeedModifier = 50f;
    [SerializeField] private Transform rotorsTransform;

    private float throttle, roll, pitch, yaw, movement;

    private bool isLeftTriggerPressed, isRightTriggerPressed;

    new Rigidbody rigidbody;


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }


    void Update()
    {
        if (vehicleEnterManager == null || !vehicleEnterManager.GetIsPlayerInsideTheVehicle()) return;
        GetInput();

        if (rotorsTransform != null)
        {
            rotorsTransform.Rotate(Vector3.forward  * rotorSpeedModifier);
        }
        vehicleEnterManager.SyncPlayerPositionAndRotation();

    }
    void FixedUpdate()
    {
        if (vehicleEnterManager == null || !vehicleEnterManager.GetIsPlayerInsideTheVehicle()) return;
        HandleInput();
    }

    private void HandleInput()
    {
        if (isRightTriggerPressed)
        {
            throttle += throttleAmount * Time.deltaTime;

        }
        else if (isLeftTriggerPressed)
        {
            throttle -= throttleAmount * Time.deltaTime;
        }
        throttle = Mathf.Clamp(throttle, 0f, 5f);

        rigidbody.velocity = transform.forward * movement * movementSpeed;
        rigidbody.AddForce(transform.up * throttle, ForceMode.Impulse);
        rigidbody.AddTorque(transform.right * pitch * responsiveness * Time.deltaTime);
        rigidbody.AddTorque(-transform.forward * roll * responsiveness * Time.deltaTime);
        rigidbody.AddTorque(transform.up * yaw * responsiveness * Time.deltaTime);

    }

    private void GetInput()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, devices);

        if (devices.Count >= 2)
        {
            Vector2 leftThumbStickValue = GetThumbStickValue(devices[0]);
            Vector2 rightThumbStickValue = GetThumbStickValue(devices[1]);
            isLeftTriggerPressed = GetTriggerValue(devices[0]);
            isRightTriggerPressed = GetTriggerValue(devices[1]);

            roll = leftThumbStickValue.x;
            movement = leftThumbStickValue.y;

            pitch = rightThumbStickValue.y;

            yaw = rightThumbStickValue.x;

        

        }
    }

    private Vector2 GetThumbStickValue(InputDevice device)
    {
        Vector2 thumbStickValue;
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out thumbStickValue);
        return thumbStickValue;
    }

    private bool GetTriggerValue(InputDevice device)
    {
        bool triggerValue;
        device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue);
        Debug.Log(triggerValue);
        return triggerValue;
    }

}
