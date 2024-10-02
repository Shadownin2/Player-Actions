using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;
    
    public void SwitchState(State newState)
    {
        //If we have a current state. Tidy up the current state and prepare for a transition to a new state
        currentState?.Exit();

        //Set the current state to the new state
        currentState = newState;

        //Prepare new state
        currentState?.Enter();
    }

    private void Update()
    {
        //Run the new states tick function every frame
        currentState?.Tick(Time.deltaTime);
    }

}
