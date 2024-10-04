using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFreeLookState : PlayerBaseState
{
    public PlayerFreeLookState(PlayerStateMachine StateMachine) : base(StateMachine){}

    //Animation Variables
    private readonly int FreeLookSpeedHash = Animator.StringToHash("FreeLookSpeed");
    private const float AnimationDampTime = 0.1f;

    public override void Enter()
    {
        Debug.Log("We Have Entered FreeLookState");
    }

    public override void Tick(float deltaTime)
    {
        StateMachine.Movement = CalculateMovement();

        StateMachine.characterController.Move(StateMachine.Movement * deltaTime * StateMachine.FreeLookMovementSpeed);

        FaceMovementDirection(deltaTime);
    }
    public override void Exit()
    {

    }

    

    Vector3 CalculateMovement()
    {
        Vector3 cameraForward = StateMachine.MainCameraTransform.transform.forward;

        cameraForward.y = 0f;
        cameraForward.Normalize();

        //Get the right direction 
        Vector3 camerarRight = StateMachine.MainCameraTransform.transform.right;

        return cameraForward*StateMachine.InputReader.MovementValue.y + camerarRight*StateMachine.InputReader.MovementValue.x;
    }

    private void FaceMovementDirection(float deltaTime)
    {
        //Animation handling 
        //If we are not moving
        if (StateMachine.InputReader.MovementValue==Vector2.zero)
        {
            StateMachine.animator.SetFloat(FreeLookSpeedHash, 0, AnimationDampTime, deltaTime);
            return;
        }
        
        //If we are moving play running/walking animation
        StateMachine.animator.SetFloat(FreeLookSpeedHash, 1, AnimationDampTime, deltaTime);
        
        //Rotation Handling
        StateMachine.transform.rotation = Quaternion.Lerp(StateMachine.transform.rotation, Quaternion.LookRotation(StateMachine.Movement), deltaTime* StateMachine.RotationDampaning);
    }


}
