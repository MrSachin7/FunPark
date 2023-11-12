
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VehicleMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float rotationSpeed = 3f;
    [SerializeField] private bool allowFly = false;
    [SerializeField] private VehicleEnterManager vehicleEnterManager;

    void Start()
    {
        Debug.Log(vehicleEnterManager != null ? "VehicleMovement manager is not null" : "VehicleMovement manager is null");
    }

    void Update()
    {
        if (vehicleEnterManager == null) return;
        Debug.Log("VehicleMovement manager is not null");

        // If the player is not inside the vehicle, do not move the vehicle
        if (!vehicleEnterManager.GetIsPlayerInsideTheVehicle()) return;


        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, devices);

        if (devices.Count >= 2)
        {
            Vector2 leftThumbStickValue = GetThumbStickValue(devices[0]);
            Vector2 rightThumbStickValue = GetThumbStickValue(devices[1]);
            MoveVehicle(leftThumbStickValue,rightThumbStickValue);
            vehicleEnterManager.SyncPlayerPositionAndRotation();
        }
        else
        {
            Debug.Log("Controller is not connected");
        }

    }

    private Vector2 GetThumbStickValue(InputDevice device)
    {
        Vector2 thumbStickValue;
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out thumbStickValue);
        return thumbStickValue;
    }

     private void MoveVehicle(Vector2 leftThumbStickValue, Vector2 rightThumbStickValue)
    {
        Debug.Log("Left Thumbstick value: " + leftThumbStickValue + ", Right Thumbstick value: " + rightThumbStickValue);

        // Use the forward direction of the vehicle for movement
        Vector3 moveDirection = transform.forward * leftThumbStickValue.y;

        // Multiply by the speed and deltaTime
        moveDirection *= moveSpeed * Time.deltaTime;

        // Move the vehicle based on the allowed movement
        if (allowFly)
        {
            // For flying, apply translation in world space
            transform.Translate(moveDirection, Space.World);
        }
        else
        {
            // For grounded movement, apply translation relative to the vehicle's rotation
            transform.Translate(moveDirection, Space.Self);
        }

        // Rotate the vehicle based on the right thumbstick for direction control
        if (Mathf.Abs(rightThumbStickValue.x) > 0.1f || Mathf.Abs(rightThumbStickValue.y) > 0.1f)
        {
            Vector3 rotateDirection = new Vector3(rightThumbStickValue.x, 0, rightThumbStickValue.y);
            Quaternion rotation = Quaternion.LookRotation(rotateDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }



  
}

