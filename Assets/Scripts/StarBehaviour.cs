using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBehaviour : MonoBehaviour
{
    UniverseUmpire myWorld;
    float scale;
    float angle;
    Vector3 rotationAxis;
    Vector3 finalPosition;
    ParticleSystemForceField myGravity;
    float gravityEventTS;
    float influenceRange;
    int deltaParticles;
    void Start()
    {
        rotationAxis = new Vector3(0, 1, 0);
        myGravity = GetComponentInChildren<ParticleSystemForceField>();
        myWorld = GetComponentsInParent<UniverseUmpire>()[0];
        finalPosition = 150 * Random.insideUnitSphere;
        myWorld.finalStarDistances.Add(finalPosition.magnitude);
        myGravity.endRange = 0;
        InvokeRepeating("expandGravity", 8.5f, 20.0f);
    }

    private void OnEnable() {
        
    }

    void FixedUpdate()
    {
        if(Time.realtimeSinceStartup < 20){
            scale = 0.1f * (Time.realtimeSinceStartup/20.0f);
            transform.localScale = new Vector3(scale, scale, scale);
        }
         
        angle = (angle + (myWorld.rateOfEvents * 50 * Time.deltaTime))%360;
        transform.rotation = Quaternion.AngleAxis(angle, rotationAxis);
        if(Time.realtimeSinceStartup >= 8){
            transform.position = Vector3.MoveTowards(transform.position, finalPosition, 
                                                    (myWorld.rateOfEvents * 0.5f * Time.deltaTime));
        }

        if(deltaParticles > 25 & myWorld.focusTimeStamp > 10){
            Debug.LogWarning("Changing Camera Focus");
            myWorld.cameraFocus = transform.position;
            Debug.LogWarning(myWorld.cameraFocus);
            myWorld.focusTimeStamp = 0;
        }
        deltaParticles = 0;
    }

    private void AbsorbedParticle(int index) {
        if(Time.realtimeSinceStartup > 20){
            deltaParticles++;
            if(transform.localScale.magnitude < 8){
                scale = scale + ((myWorld.rateOfEvents/500));
                transform.localScale = new Vector3(scale, scale, scale);
            } 
        }
    }

    private void expandGravity(){
        if(transform.localScale.magnitude < 5){
            myGravity.endRange = 300/(1 + transform.localScale.magnitude);
        }
        Invoke("collapseGravity", 2.0f);
    }

    private void collapseGravity(){
        myGravity.endRange = 1;
    }
}
