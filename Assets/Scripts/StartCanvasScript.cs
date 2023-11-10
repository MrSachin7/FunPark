using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StartCanvasScript : MonoBehaviour
{
    [SerializeField] private ActionBasedSnapTurnProvider snapTurnProvider;
    [SerializeField] private ActionBasedContinuousTurnProvider continousTurnProvider;
    [SerializeField] private ActionBasedContinuousMoveProvider continuousMoveProvider;


    public void OnDropDownValueChanged(int index)
    {
        if (index == 0)
        {
            snapTurnProvider.enabled = true;
            continousTurnProvider.enabled = false;
        }
        else if (index == 1)
        {
            snapTurnProvider.enabled = false;
            continousTurnProvider.enabled = true;
        }
    }

    public void OnSliderValueChanged(float value)
    {
        continuousMoveProvider.moveSpeed = value;
    }
    public void Exit() 
    {
        Debug.Log("Exiting the application");
        Application.Quit();
    }
}
