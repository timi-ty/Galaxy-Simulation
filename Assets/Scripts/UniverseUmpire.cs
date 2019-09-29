using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UniverseUmpire : MonoBehaviour
{
    public GameObject starPrefab;
    public ParticleSystem spaceDebris;
    public List<GameObject> starsAlive = new List<GameObject>();
    public List<float> finalStarDistances = new List<float>();
    public CameraController myCamera;
    public Vector3 cameraFocus;
    public float focusTimeStamp;
    [Range(0.1f, 1.0f)]
    public float rateOfEvents;
    
    void Start()
    {
        starsAlive.Clear();
        for(int i = 0; i < 7; i++){
            Random.InitState(i);
            starsAlive.Add(Instantiate(starPrefab, 0 * (Random.insideUnitSphere), Random.rotation));
            starsAlive[i].transform.SetParent(gameObject.transform);
            starsAlive[i].transform.localScale = new Vector3(0, 0, 0);
            spaceDebris.trigger.SetCollider(i + 1, starsAlive[i].GetComponent<Collider>());
        }   
    }

    void Update()
    {
        focusTimeStamp += Time.deltaTime;
        if(focusTimeStamp > 30){
            cameraFocus = new Vector3(0, 0, 0);
        }
    }
}
