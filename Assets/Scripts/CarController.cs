using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

using UnityEngine.XR.Interaction.Toolkit;


public class CarController : MonoBehaviour
{
    private float horizontalInput, verticalInput;
    private float currentSteerAngle, currentBreakForce;
    private bool isBreaking;

    [SerializeField] private VehicleEnterManager vehicleEnterManager;

    [SerializeField] private float motorForce, maxSteerAngle, breakForce;

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

    void Update()
    {
        if (vehicleEnterManager == null) return;

        // If the player is not inside the vehicle, do not move the vehicle
        if (!vehicleEnterManager.GetIsPlayerInsideTheVehicle()) return;
        SyncPlayerAndCar();

    }




    private void GetInput()
    {
        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, devices);

        if (devices.Count >= 2)
        {
            Vector2 leftThumbStickValue = GetThumbStickValue(devices[0]);
            Vector2 rightThumbStickValue = GetThumbStickValue(devices[1]);
           isBreaking = GetTriggerValue(devices[1]) || GetTriggerValue(devices[0]);

            // Steering 
            horizontalInput = rightThumbStickValue.x;

            // Acceleration
            verticalInput = leftThumbStickValue.y;

        }
    }




    private void HandleMotor()
    {
        frontLeftWheelCollider.motorTorque = verticalInput * motorForce;
        frontRightWheelCollider.motorTorque = verticalInput * motorForce;
        currentBreakForce = isBreaking ? breakForce : 0f;
        ApplyBreaking();

    }

    private void ApplyBreaking()
    {
        Debug.Log("Current Break Force: " + currentBreakForce);
        frontLeftWheelCollider.brakeTorque = currentBreakForce;
        frontRightWheelCollider.brakeTorque = currentBreakForce;
        rearLeftWheelCollider.brakeTorque = currentBreakForce;
        rearRightWheelCollider.brakeTorque = currentBreakForce;

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

    private bool GetTriggerValue(InputDevice device)
    {
        bool triggerValue;
        device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerValue);
        return triggerValue;
    }

    private void SyncPlayerAndCar()
    {
        // Sync the player's position and rotation with the car's position and rotation
        vehicleEnterManager.SyncPlayerPositionAndRotation();
    }

}
