using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningDoor : MonoBehaviour
{
    private Transform doorTransform;

    [SerializeField] private float doorOpeningSpeed;
    [SerializeField] private float threshold;
    private float doorOpeneingProgress;

    void Start()
    {
        doorTransform = GetComponent<Transform>();
        doorOpeneingProgress = 0;
    }


    void Update()
    {
        if (doorTransform.position.y < threshold) {

            doorOpeneingProgress += doorOpeningSpeed * Time.deltaTime;
            doorTransform.position += new Vector3(0, doorOpeneingProgress, 0);
        }
    }
}
