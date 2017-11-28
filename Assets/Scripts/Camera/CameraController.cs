using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controls the position and rotation of the camera while playing.
/// The camera follow a transform set to the position of the player character.
/// The player is able to zoom in and out, and control the camera's pitch and yaw
/// within set limits. If the camera would clip through terrain, or other objects 
/// with the "Wall" or "Terrain" layers, it zooms in towards the player
/// </summary>
public class CameraController : MonoBehaviour
{
    [Tooltip("Target pointed at by camera defining its rotation")]
    public Transform target;

    [Tooltip("Child of target defining its pitch")]
    public Transform pivot;

    [Tooltip("Camera distance to target")]
    public float offset;

    [Tooltip("Distance set by player")]
    public float offsetWanted;

    [Tooltip("Time for camera to reach desired zoom when able")]
    public float cameraSmoothTime;
    
    float currentSmoothSpeed;   // Used by SmoothDamp for changing the offset

    [Tooltip("Rate of camera panning and pitching")]
    public float rotateSpeed;

    [Tooltip("Rate camera zooms in and out")]
    public float zoomSpeed;

    [Tooltip("Maximum pitch up")]
    public float maxViewAngle;

    [Tooltip("Maximum pitch down, if ground allows")]
    public float minViewAngle;

    [Tooltip("Inverts mouse Y axis for camera control")]
    public bool invertY;

    [Tooltip("Maximum distance camera can zoom out")]
    public float maxDistance;

    [Tooltip("Minimum distance camera can zoom in to target")]
    public float minDistance;

    [Tooltip("Minimum distance camera can be forced towards target by terrain")]
    public float hardMinDistance;

    public LevelManager level;

    [Tooltip("Radius of spherecast to check for obstructions in front of camera")]
    public float occlusionRadius;

    [Tooltip("LayerMask indicating objects that camera can't clip into")]
    public LayerMask BlocksCamera;

    public PlayerController player;

	void Start ()
    {
        // Ensure pivot is at target position and is its child
        pivot.transform.position = target.transform.position;
        pivot.transform.parent = target.transform;

        offset = offsetWanted;

        player = FindObjectOfType<PlayerController>();
	}

    // Update is called once per frame
    void LateUpdate()
    {

        if (!level.IsPaused)
        {
            float modifiedMaxAngle = maxViewAngle;
            float modifiedMinAngle = 0;
            RaycastHit hit;
            modifiedMinAngle = minViewAngle;

            // Move Camera Target towards player
            if (player.CameraFollows)
            {
                target.transform.position = player.transform.position + player.CameraTargetOffset;
            }
            // Get the X position of mouse and rotate the target.
            float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
            target.Rotate(0, horizontal, 0);
            
            float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
            float zoom = -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

            if (invertY)
            {
                vertical = -vertical;
            }

            // Rotate pivot, with pitch within specified limits
            float pivotPitch = pivot.rotation.eulerAngles.x;
            float newPitch = pivotPitch + vertical;
            
            if(newPitch > 180f)
            {
                newPitch -= 360f;
            }

            if (newPitch > modifiedMaxAngle)
            {
                vertical += (modifiedMaxAngle - newPitch);
            } 
            if (newPitch < modifiedMinAngle)
            {
                vertical += (modifiedMinAngle - newPitch);
            }

            pivot.Rotate(vertical, 0, 0);

            // Update requested level of zoom
            offsetWanted = Mathf.Clamp(offsetWanted + zoom, minDistance, maxDistance);

            // move the camera based on current rotation of target and pivot
            float desiredYAngle = target.eulerAngles.y;
            float desiredXAngle = pivot.eulerAngles.x;

            Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
            
            // Check for occlusion by terrain
            if(Physics.SphereCast(target.transform.position, occlusionRadius, -pivot.forward, out hit, offsetWanted, BlocksCamera, QueryTriggerInteraction.Ignore))
            {
                //Debug.Log("Camera collided with " + hit.collider + ", " + hit.collider.tag);
                if(hit.distance > minDistance)
                {
                    offset = hit.distance;
                } else
                {
                    offset = Mathf.Max(hit.distance, hardMinDistance);
                }
                currentSmoothSpeed = 0f;

            } else
            {
                offset = Mathf.SmoothDamp(offset, offsetWanted, ref currentSmoothSpeed, cameraSmoothTime);
            }

            // Move to offset wanted
            //HACK change variable names to fit
            transform.position = target.position - (pivot.forward * offset);

            transform.LookAt(target);
        }
    }


}
