using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    public float gravityScale;
	public int treasureColleceted;

	[Header ("Lives")]
	public int currentlives;
	public int maxlives;
	public int minlives;

	[Header ("UI Elements")]

	public Text livesText;


    public CharacterController controller;

    private Vector3 moveDirection;

//  public Rigidbody playerRB;


	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
//        playerRB = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void Update ()
    { /*
        playerRB.velocity = new Vector3 (Input.GetAxis("Horizontal") * moveSpeed, playerRB.velocity.y, Input.GetAxis("Vertical") * moveSpeed);
        if(Input.GetButtonDown("Jump"))
        {
            playerRB.velocity = new Vector3(playerRB.velocity.x, jumpForce, playerRB.velocity.z);
        } */

        //        moveDirection = new Vector3(Input.GetAxis("Horizontal") * moveSpeed, moveDirection.y, Input.GetAxis("Vertical") * moveSpeed);

        float yStore = moveDirection.y;
        //Debug.Log("vert: " + Input.GetAxis("Vertical") + " horiz: " + Input.GetAxis("Horizontal"));
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        //moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f) * moveSpeed;
        moveDirection.y = yStore;

        if (controller.isGrounded)
        {
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }

        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);

        controller.Move(moveDirection * Time.deltaTime);

		livesText.text = "Lives Remaining: " + currentlives + " / " + maxlives ;
    }
}
