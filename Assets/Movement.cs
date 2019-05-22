using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private GameObject mainCamera;
    private GameObject playerBody;
    private GameObject player;
    private Rigidbody vehicleRigidBody;

    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        playerBody = GameObject.FindGameObjectWithTag("Vehicle");
        if(playerBody != null)
        {
            vehicleRigidBody = playerBody.GetComponent<Rigidbody>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerBody != null)
        {
            //Rotate the vehicle towards where the player is facing
            playerBody.transform.eulerAngles = new Vector3(playerBody.transform.eulerAngles.x, mainCamera.transform.eulerAngles.y, playerBody.transform.eulerAngles.z);
            player.transform.position = new Vector3(playerBody.transform.position.x, player.transform.position.y, playerBody.transform.position.z);
        }
    }

    void FixedUpdate()
    {
        Ray raycastRay = new Ray(mainCamera.transform.position, mainCamera.transform.forward);
        RaycastHit hitInfo;
        bool rayCastHit = Physics.Raycast(raycastRay, out hitInfo);

        //Move player forward if looking down
        if (vehicleRigidBody != null)
        {
            DoMove(mainCamera.transform.eulerAngles.x);
        }
    }

    private void DoMove(float cameraRotation)
    {
        if (cameraRotation > 4 && cameraRotation < 25)
        {
            vehicleRigidBody.velocity = vehicleRigidBody.transform.forward * speed;
        }
        else
        {
            vehicleRigidBody.velocity = Vector3.zero;
            vehicleRigidBody.angularVelocity = Vector3.zero;
        }
    }
}
