using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VehicleEnterManager : MonoBehaviour
{
    [SerializeField] private Transform vehicleEnterPoint;
    [SerializeField] private ActionBasedContinuousMoveProvider continuousMoveProvider;
    [SerializeField] private ActionBasedSnapTurnProvider snapTurnProvider;
    [SerializeField] private ActionBasedContinuousTurnProvider continousTurnProvider;
    private bool isPlayerInRange = false;
    private bool isPlayerInsideTheVehicle = false;

    private Collider playerCollider;

    private bool isButtonPressedLastFrame = false;


    void Update()
    {
        if (!isPlayerInRange) return;

        List<InputDevice> devices = new List<InputDevice>();
        InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.Controller, devices);

        if (devices.Count > 0)
        {
            if (GetPrimaryButtonValue(devices))
            {
                if (!isButtonPressedLastFrame)
                {
                    TogglePlayerInOrOutOfVehicle();
                    isButtonPressedLastFrame = true;
                }
            }
            else
            {
                isButtonPressedLastFrame = false;
            }
        }
        else
        {
            Debug.Log("Controller is not connected");
        }


    }

    private bool GetPrimaryButtonValue(List<InputDevice> inputDevices)
    {
        foreach (var device in inputDevices)
        {
            device.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValue);
            if (primaryButtonValue)
            {
                return true;

            }

        }
        return false;
    }

    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is in range");
            isPlayerInRange = true;
            playerCollider = other;
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player is out of range");
            isPlayerInRange = false;
            playerCollider = null;
        }
    }


    void TogglePlayerInOrOutOfVehicle()
    {
        if (playerCollider == null) return;

        Debug.Log("Toggling player in or out of the vehicle");

        if (!isPlayerInsideTheVehicle)
        {
            MovePlayerInsideVehicle();
            isPlayerInsideTheVehicle = true;
            DisablePlayerMovements();
        }
        else
        {
            MovePlayerOutOfVehicle();
            isPlayerInsideTheVehicle = false;
            EnablePlayerMovements();
        }
    }

    void MovePlayerInsideVehicle()
    {
        playerCollider.transform.position = vehicleEnterPoint.position;
        playerCollider.transform.rotation = vehicleEnterPoint.rotation;
        isPlayerInsideTheVehicle = true;


    }

    void MovePlayerOutOfVehicle()
    {
        playerCollider.transform.position = vehicleEnterPoint.position + new Vector3(0, 0, 2);
        playerCollider.transform.rotation = vehicleEnterPoint.rotation;
        isPlayerInsideTheVehicle = false;



    }


    private void DisablePlayerMovements()
    {
        continuousMoveProvider.enabled = false;
        snapTurnProvider.enabled = false;
        continousTurnProvider.enabled = false;
    }


    private void EnablePlayerMovements()
    {
        continuousMoveProvider.enabled = true;
        snapTurnProvider.enabled = true;
    }

    public bool GetIsPlayerInsideTheVehicle()
    {
        return isPlayerInsideTheVehicle;
    }

    public Collider GetPlayerCollider()
    {
        return playerCollider;
    }

    public Transform GetVehicleEnterPoint()
    {
        return vehicleEnterPoint;
    }

    public void SyncPlayerPositionAndRotation()
    {
        // Sync the player position and rotation with the vehicle
        MovePlayerInsideVehicle();

    }
}
