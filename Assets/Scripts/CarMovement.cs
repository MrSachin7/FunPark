using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class CarMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    private XRController xrController;
    private CarEnterManager carEnterManager;

    void Start()
    {
        xrController = GetComponent<XRController>();
        carEnterManager = GetComponent<CarEnterManager>();
    }

    void Update()
    {
        if (xrController == null || carEnterManager == null) return;
        if (!carEnterManager.GetIsPlayerInsideTheCar()) return;
        MoveCar();
    }

    private void MoveCar()
    {
        Vector2 thumbStickValue;
        if (xrController.inputDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out thumbStickValue))
        {
            Vector3 moveDirection = new Vector3(thumbStickValue.x, 0, thumbStickValue.y).normalized;
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }
}
