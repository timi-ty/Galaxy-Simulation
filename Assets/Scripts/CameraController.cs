using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public UniverseUmpire myWorld;
    [Range(5, 150)]
    public float distanceFromCenter;
    public Vector3 positionVector;
    public Vector3 rotationAxis;
    Vector3 cameraPan;
    public bool seeAllStars = true;
    float angle;
    float dist;
    void Start()
    {
        transform.position = new Vector3(0, 0, -1) * 5;
        positionVector = new Vector3(0, 0, -1);
        rotationAxis = new Vector3(0, 1, 0);
        cameraPan = myWorld.cameraFocus = new Vector3(0, 0, 0);
    }

    void FixedUpdate()
    {
        if(Time.realtimeSinceStartup >= 20){
                float velocity = Mathf.Clamp(Time.realtimeSinceStartup - 20, 0, 100) ;
            dist = Mathf.Clamp(dist + (velocity * Time.deltaTime), 5, distanceFromCenter);
            rotationAxis = new Vector3(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle), 1);

            float scale = 15 * (1 - ((distanceFromCenter - dist)/distanceFromCenter));
            angle = (angle + (myWorld.rateOfEvents * scale * 
                            Time.fixedDeltaTime))%360;
            transform.rotation = Quaternion.AngleAxis(angle, rotationAxis);
            
            float var = Vector3.Magnitude(myWorld.cameraFocus - cameraPan) / dist;
            Vector3 tempVector = RotateByQuartenion(positionVector * dist, transform.rotation);
            cameraPan = Vector3.MoveTowards(cameraPan, myWorld.cameraFocus, velocity * var * Time.deltaTime);
            transform.position = tempVector + cameraPan; 
        }
        if(seeAllStars){
            distanceFromCenter = Mathf.Clamp(distanceFromCenter, Mathf.Min(myWorld.finalStarDistances.ToArray()), 
                                            Mathf.Max(myWorld.finalStarDistances.ToArray()));
        } 
    }

    Vector3 RotateByQuartenion(Vector3 vector, Quaternion quaternion){
        Quaternion inverseQ = Quaternion.Inverse(quaternion);
        Quaternion compVector = new Quaternion(vector.x, vector.y, vector.z, 0);
        Quaternion qNion = quaternion * compVector * inverseQ;
        return new Vector3(qNion.x, qNion.y, qNion.z);
    }
}
