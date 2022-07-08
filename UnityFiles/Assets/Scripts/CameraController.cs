using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float lerpSpeed;

    Vector3 tempPosition;
    [SerializeField]
    float minX, minY, maxX, maxY;

    // Update is called once per frame
    void FixedUpdate()
    {

        if (target == null) return;
        tempPosition = target.position;
        tempPosition.z = -10;

        if (target.position.x < minX)
            tempPosition.x = minX;
        if (target.position.y < minY)
            tempPosition.y = minY;

        if (target.position.x > maxX)
            tempPosition.x = maxX;
        if (target.position.y > maxY)
            tempPosition.y = maxY;

        transform.position = Vector3.Lerp(transform.position, tempPosition, lerpSpeed * Time.deltaTime);
    }
}
