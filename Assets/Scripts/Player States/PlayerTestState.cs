using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTestState : PlayerBaseState
{
    public PlayerTestState(PlayerStateMachine StateMachine) : base(StateMachine) {}

    public override void Enter()
    {
        Debug.Log("Entering State");
    }

    public override void Exit()
    {
        Debug.Log("Exiting State");
    }
    
    public override void Tick(float deltaTime)
    {
        Vector3 movement = new Vector3();
        movement.x = StateMachine.InputReader.MovementValue.x;
        movement.y = 0;
        movement.z = StateMachine.InputReader.MovementValue.y;

        StateMachine.transform.Translate(movement * deltaTime);
    }
 
    
}
