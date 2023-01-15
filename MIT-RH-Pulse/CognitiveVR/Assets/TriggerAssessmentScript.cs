using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAssessmentScript : MonoBehaviour
{
    public StateMachine statemachine;
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered");
        statemachine.triggerAssessCollision();
    }
}
