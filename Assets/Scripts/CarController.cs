using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

using UnityEngine.XR.Interaction.Toolkit;


public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle;

    [SerializeField] private VehicleEnterManager vehicleEnterManager;

    [SerializeField] private float motorForce, maxSteerAngle;

    [SerializeField] private WheelCollider frontLeftWheelCollider, frontRightWheelCollider;
    [SerializeField] private WheelCollider rearLeftWheelCollider, rearRightWheelCollider;

    [SerializeField] private Transform frontLeftWheelTransform, frontRightWheelTransform;
    [SerializeField] private Transform rearLeftWheelTransform, rearRightWheelTransform;



    private void FixedUpdate()
    {
        if (vehicleEnterManager == null) return;
        
        // If the player is not inside the vehicle, do not move the vehicle
        if (!vehicleEnterManager.GetIsPlayerInsideTheVehicle()) return;

        GetInput();
        HandleMotor();
        HandleSteering();
        UpdateWheels();
    }


    private void GetInput()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, devices);

        if (devices.Count >= 2)
        {
            Vector2 leftThumbStickValue = GetThumbStickValue(devices[0]);
            Vector2 rightThumbStickValue = GetThumbStickValue(devices[1]);

            // Acceleration 
            horizontalInput = leftThumbStickValue.x;

            // Steering
            verticalInput = rightThumbStickValue.y;

        }
    }

    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;

    }

    private void HandleSteering()
    {
        currentSteerAngle = maxSteerAngle * horizontalInput;
        frontLeftWheelCollider.steerAngle = currentSteerAngle;
        frontRightWheelCollider.steerAngle = currentSteerAngle;

    }

    private void UpdateWheels()
    {
        UpdateSingleWheel(frontLeftWheelCollider, frontLeftWheelTransform);
        UpdateSingleWheel(frontRightWheelCollider, frontRightWheelTransform);
        UpdateSingleWheel(rearLeftWheelCollider, rearLeftWheelTransform);
        UpdateSingleWheel(rearRightWheelCollider, rearRightWheelTransform);

    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform wheelTransform)
    {
        Vector3 pos;
        Quaternion rot;
        wheelCollider.GetWorldPose(out pos, out rot);
        wheelTransform.rotation = rot;
        wheelTransform.position = pos;

    }

    private Vector2 GetThumbStickValue(InputDevice device)
    {
        Vector2 thumbStickValue;
        device.TryGetFeatureValue(CommonUsages.primary2DAxis, out thumbStickValue);
        return thumbStickValue;
    }
}
