using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CarEnterManager : MonoBehaviour
{
    [SerializeField] private Transform carEnterPoint;
    private bool isPlayerInRange = false;
    private bool isPlayerInsideTheCar = false;

    private XRController xrController;
    private Collider playerCollider;

    void Start()
    {
        xrController = GetComponent<XRController>();
        xrController.inputDevice.TryGetFeatureValue(CommonUsages.primaryButton, out _); // Initialize button state
    }


    void Update()
    {
        if (!isPlayerInRange) return;
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

    void TogglePlayerInOrOutOfCar()
    {
        if (playerCollider == null) return;
        if (!isPlayerInsideTheCar)
        {
            MovePlayerToCar();
        }
        else
        {
            MovePlayerOutOfCar();
        }
    }

    void MovePlayerToCar()
    {
        playerCollider.transform.position = carEnterPoint.position;
        playerCollider.transform.rotation = carEnterPoint.rotation;
        isPlayerInsideTheCar = true;
    }

    void MovePlayerOutOfCar()
    {
        playerCollider.transform.position = carEnterPoint.position + new Vector3(0, 0, 2);
        playerCollider.transform.rotation = carEnterPoint.rotation;
        isPlayerInsideTheCar = false;
    }

    public bool GetIsPlayerInsideTheCar()
    {
        return isPlayerInsideTheCar;
    }

}
