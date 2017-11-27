using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Allows player to move camera around character and zoom in or out
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
    
    float currentSmoothSpeed;

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

        pivot.transform.position = target.transform.position;
        pivot.transform.parent = target.transform;

        // Hide cursor
        Cursor.lockState = CursorLockMode.Locked;

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

            // Get the Y position of mouse and rotate pivot.
            float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;

            float zoom = -Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

            if (invertY)
            {
                vertical = -vertical;
            }

            // Limit pitch to within rotation
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

            offsetWanted = Mathf.Clamp(offsetWanted + zoom, minDistance, maxDistance);

            // move the camera based on current rotation of target, using original offset
            float desiredYAngle = target.eulerAngles.y;
            float desiredXAngle = pivot.eulerAngles.x;

            Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
            
            // Check for occlusion by terrain
            //TODO create layer mask for things to ignore
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
