using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VehicleEnterManager : MonoBehaviour
{
    [SerializeField] private Transform vehicleEnterPoint;
    [SerializeField] private ActionBasedContinuousMoveProvider continuousMoveProvider;
    private bool isPlayerInRange = false;
    private bool isPlayerInsideTheVehicle = false;

    private XRController xrController;
    private Collider playerCollider;

    private bool isButtonPressedLastFrame = false;

    void Start()
    {
        xrController = GetComponent<XRController>();
        xrController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out _); // Initialize button state

    }


    void Update()
    {
        if (!isPlayerInRange) return;
        if (xrController == null) return;

        // Check if button is pressed
        bool isButtonPressedNow = false;
        xrController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out isButtonPressedNow);

        if (isButtonPressedNow && !isButtonPressedLastFrame)
        {
            TogglePlayerInOrOutOfVehicle();

        }
        isButtonPressedLastFrame = isButtonPressedNow;

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
            playerCollider = other;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
            playerCollider = null;
        }
    }

    void TogglePlayerInOrOutOfVehicle()
    {
        if (playerCollider == null) return;
        if (!isPlayerInsideTheVehicle)
        {
            MovePlayerInsideVehicle();
        }
        else
        {
            MovePlayerOutOfVehicle();
        }
    }

    void MovePlayerInsideVehicle()
    {
        playerCollider.transform.position = vehicleEnterPoint.position;
        playerCollider.transform.rotation = vehicleEnterPoint.rotation;
        isPlayerInsideTheVehicle = true;

        continuousMoveProvider.enabled = false;

    }

    void MovePlayerOutOfVehicle()
    {
        playerCollider.transform.position = vehicleEnterPoint.position + new Vector3(0, 0, 2);
        playerCollider.transform.rotation = vehicleEnterPoint.rotation;
        isPlayerInsideTheVehicle = false;

        continuousMoveProvider.enabled = true;

    }

    public bool GetIsPlayerInsideTheVehicle()
    {
        return isPlayerInsideTheVehicle;
    }

}
