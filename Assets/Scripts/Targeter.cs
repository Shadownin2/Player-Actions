using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    //A List to store all the available targets
    //A Reference to the current target
    private List<Target> targets = new List<Target> ();
    public Target currentTarget {get; private set;}

    //References to both cameras, free look and target camera
    [SerializeField] private CinemachineTargetGroup cinemachineTargetGroup;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void onTriggerEnter(Collider other)
    {
        if(!other.TryGetComponent<Target>(out Target target))
        {
            return;
        }

        targets.Add(target);
        target.onDestroyed += RemoveTarget;
    }

    private void OnTriggerExit(Collider other)
    {
        if(!other.TryGetComponent<Target>(out Target target))
        {
            return;
        }

        RemoveTarget(target);
    }

    //Method to select closest target first
    public bool SelectTarget()
    {
        //If there are no targets return false
        if(targets.Count ==0) return false;
        
        //Variables to track the closest target and its distance from the player
        Target closestTarget = null;
        float closestTargetDistance = Mathf.Infinity;

        //Iterate through all targets
        foreach(Target target in targets)
        {
            //Get the targets position in the cameras viewport coordinates      
            Vector2 viewPos = mainCamera.WorldToViewportPoint(target.transform.position);

            //Check if the target is within the view of the camera/screen, if its not move to next target
            if(viewPos.x < 0.0f || viewPos.x > 1.0f || viewPos.y < 0.0f || viewPos.y > 1.0f) continue;
        

            //Calculate the squared distance from the center of the viewport to the target location
            Vector2 toCenter = viewPos - new Vector2(0.5f, 0.5f);

            //If this target is closer to the center than the previous closest target, update the closest target
            if (toCenter.sqrMagnitude < closestTargetDistance)
            {
                closestTarget = target;
                closestTargetDistance = toCenter.sqrMagnitude;
            }
        }

        //If no valid target was found, return false 
        if (closestTarget ==  null) return false;

        //Set closest target as current target
        currentTarget = closestTarget;

        //Add current target to cinemachine target group with weight and radius
        cinemachineTargetGroup.AddMember(currentTarget.transform, 1f, 2f);

        return true;
    }

    //Tidy up method , when done with targeting
    public void RemoveTarget(Target target)
    {
        //If target is passed in and its the same as the current target. Release the current target as well
        if(currentTarget == target)
        {
            cinemachineTargetGroup.RemoveMember(currentTarget.transform);
            currentTarget = null;
        }

        //Using the target passed in tidy up the target list
        target.onDestroyed -= RemoveTarget;
        targets.Remove(target);      
    }

     //Simple method to stop targeting enemies 
    public void Cancel()
    {
        //If there is no target selected, exit method early
        if (currentTarget == null) return;

        cinemachineTargetGroup.RemoveMember(currentTarget.transform);

        currentTarget = null; 

    }
}
