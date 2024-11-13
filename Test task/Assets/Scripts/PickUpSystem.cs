using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSystem : MonoBehaviour
{

    public GameObject player;
    public Transform holdPos;

    [SerializeField] private float throwForce = 500f; 
    [SerializeField] private float pickUpRange = 5f; 
    private float rotationSensitivity = 1f; 
    private GameObject heldObj;
    private Rigidbody heldObjRb; 
    private bool canDrop = true; 
    private int LayerNumber; 

    private PlayerMovement playerMovement;
    private float prevoiousLookSpeed;

    void Start()
    {
        LayerNumber = LayerMask.NameToLayer("HoldLayer"); 
        playerMovement = player.GetComponent<PlayerMovement>();

        prevoiousLookSpeed = playerMovement.lookSpeed;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObj == null)
            {

                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, pickUpRange))
                    if (hit.transform.gameObject.tag == "CanPickUp")
                        PickUpObject(hit.transform.gameObject);
            }
            else if (canDrop == true)
            {
                StopClipping(); 
                DropObject();
            }
        }
        if (heldObj != null)
        {
            MoveObject(); 
            RotateObject();
            if (Input.GetKeyDown(KeyCode.Mouse0) && canDrop == true)
            {
                StopClipping();
                ThrowObject();
            }

        }
    }
    void PickUpObject(GameObject pickUpObj)
    {
        if (pickUpObj.GetComponent<Rigidbody>())
        {
            heldObj = pickUpObj; 
            heldObjRb = pickUpObj.GetComponent<Rigidbody>(); 

            heldObjRb.isKinematic = true;
            heldObjRb.transform.parent = holdPos.transform; 
            heldObj.layer = LayerNumber; 

            Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), true);
        }
    }
    void DropObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0; 
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null; 
        heldObj = null; 
    }
    void MoveObject()
    {
        heldObj.transform.position = holdPos.transform.position;
    }
    void RotateObject()
    {
        if (Input.GetKey(KeyCode.R))
        {
            canDrop = false; 

            playerMovement.lookSpeed = 0f;

            float XaxisRotation = Input.GetAxis("Mouse X") * rotationSensitivity;
            float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSensitivity;

            heldObj.transform.Rotate(Vector3.down, XaxisRotation);
            heldObj.transform.Rotate(Vector3.right, YaxisRotation);
        }
        else
        {
            playerMovement.lookSpeed = prevoiousLookSpeed;
            canDrop = true;
        }
    }
    void ThrowObject()
    {
        Physics.IgnoreCollision(heldObj.GetComponent<Collider>(), player.GetComponent<Collider>(), false);
        heldObj.layer = 0;
        heldObjRb.isKinematic = false;
        heldObj.transform.parent = null;
        heldObjRb.AddForce(transform.forward * throwForce);
        heldObj = null;
    }
    void StopClipping()
    {
        var clipRange = Vector3.Distance(heldObj.transform.position, transform.position); 
        RaycastHit[] hits;
        hits = Physics.RaycastAll(transform.position, transform.TransformDirection(Vector3.forward), clipRange);
 
        if (hits.Length > 1)
        {
            heldObj.transform.position = transform.position + new Vector3(0f, -0.5f, 0f);
        }
    }
}
