using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class VehicleMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private bool allowFly = false;
    private XRController xrController;
    private VehicleEnterManager vehicleEnterManager;

    void Start()
    {
        xrController = GetComponent<XRController>();
        vehicleEnterManager = GetComponent<VehicleEnterManager>();
    }

    void Update()
    {
        if (xrController == null || vehicleEnterManager == null) return;


        // If the player is not inside the vehicle, do not move the vehicle
        if (!vehicleEnterManager.GetIsPlayerInsideTheVehicle()) return;
        MoveVehicle();
    }

    private void MoveVehicle()
    {
        Vector2 thumbStickValue;
        if (xrController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out thumbStickValue))
        {
            Vector3 moveDirection = new Vector3(thumbStickValue.x, 0, thumbStickValue.y).normalized;


            if (allowFly)
            {
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

            }
            else
            {
                transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

            }
        }
    }
}
