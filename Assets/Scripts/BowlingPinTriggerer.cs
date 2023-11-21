using UnityEngine;
using System.Collections;

public class BowlingPinTriggerer : MonoBehaviour
{
    [SerializeField] private GameObject[] pins;
    [SerializeField] private GameObject[] balls;


    private Vector3[] pinOriginalPositions;
    private Vector3[] ballOriginalPositions;


    void Start(){
        pinOriginalPositions = new Vector3[pins.Length];
        ballOriginalPositions = new Vector3[balls.Length];

        for (int i = 0; i < pins.Length; i++)
        {
            pinOriginalPositions[i] = pins[i].transform.position;
        }

        for (int i = 0; i < balls.Length; i++)
        {
            ballOriginalPositions[i] = balls[i].transform.position;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BowlingBall"))
        {
            Debug.Log("Bowling ball hit the pins!");
            StartCoroutine(ResetPositions());
        }
    }

    private IEnumerator ResetPositions()
    {
        yield return new WaitForSeconds(5f);


        Debug.Log("Resetting positions...");
        Debug.Log("Pin positions: " + pins[0].transform.position);
        Debug.Log("pin original positions: " + pinOriginalPositions[0]);
    
        // Reset pin positions
        for (int i = 0; i < pins.Length; i++)
        {
            pins[i].transform.position = pinOriginalPositions[i];
                        // You may want to reset other properties as well, depending on your needs
        }

        // Reset ball positions
        for (int i = 0; i < balls.Length; i++)
        {
            balls[i].transform.position = ballOriginalPositions[i];
            // You may want to reset other properties as well, depending on your needs
        }
    }


}
