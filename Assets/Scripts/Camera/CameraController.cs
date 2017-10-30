using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;

    public float offset;

    public float offsetWanted;

    public float cameraSmoothTime;

    float currentSmoothSpeed;
    
    public bool useOffsetValues;

    public float rotateSpeed;

    public float zoomSpeed;

    public Transform pivot;

    public float maxViewAngle;

    public float minViewAngle;

    public float maxDistance;

    public float minDistance;

    public bool invertY;

    public LevelManager level;

    public float occlusionRadius;

	// Use this for initialization
	void Start ()
    {
        //if (!useOffsetValues)
        //{
        //    offset = target.position - transform.position;
        //}

        pivot.transform.position = target.transform.position;
        pivot.transform.parent = target.transform;

        // Hide cursor
        Cursor.lockState = CursorLockMode.Locked;

        offset = offsetWanted;
	}

    // Update is called once per frame
    void LateUpdate()
    {

        if (!level.IsPaused)
        {
            RaycastHit hit;

            // Get the X position of mouse and rotate the target.
            float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
            target.Rotate(0, horizontal, 0);

            // Get the Y position of mouse and rotate pivot.
            float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;
            //pivot.Rotate(-vertical, 0, 0);

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

                if (newPitch > maxViewAngle)
                {
                    vertical += (maxViewAngle - newPitch);
                } 
                if (newPitch < minViewAngle)
                {
                    vertical += (minViewAngle - newPitch);
                }

            pivot.Rotate(vertical, 0, 0);

            offsetWanted = Mathf.Clamp(offsetWanted + zoom, minDistance, maxDistance);

            // move the camera based on current rotation of target, using original offset
            float desiredYAngle = target.eulerAngles.y;
            float desiredXAngle = pivot.eulerAngles.x;

            Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
            
            // Check for occlusion by terrain
            //TODO create layer mask for things to ignore
            if(Physics.SphereCast(target.transform.position, occlusionRadius, -pivot.forward, out hit, offsetWanted))
            {
                if(hit.distance > minDistance)
                {
                    offset = hit.distance;
                } else
                {
                    //TODO if less than minDistance, try rotating instead
                    offset = minDistance;
                }

            } else
            {
                offset = Mathf.SmoothDamp(offset, offsetWanted, ref currentSmoothSpeed, cameraSmoothTime);
            }

            // Move to offset wanted
            transform.position = target.position - (pivot.forward * offset);

            //  transform.position = target.position - offset;

            //if (transform.position.y < target.position.y)
            //{
            //    transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);
            //}
            transform.LookAt(target);
        }
    }


}
