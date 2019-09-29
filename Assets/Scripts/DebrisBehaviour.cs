using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisBehaviour : MonoBehaviour
{
    ParticleSystem debrisParticles;
    UniverseUmpire myUniverse;
    List<ParticleSystem.Particle> outside = new List<ParticleSystem.Particle>();
    List<ParticleSystem.Particle> allParticles = new List<ParticleSystem.Particle>();
    void Start()
    {
        debrisParticles = GetComponent<ParticleSystem>();
        myUniverse = GetComponentInParent<UniverseUmpire>();
    }


    void Update()
    {
        
    }


    private void OnParticleTrigger() 
    {
        int numberOut = debrisParticles.GetTriggerParticles(ParticleSystemTriggerEventType.Outside, outside);
        int numberAlive = debrisParticles.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, allParticles);

        for (int i = 0; i < numberAlive; i++)
        {
            ParticleSystem.Particle p = allParticles[i];
            int j = 0;

            foreach (var star in myUniverse.starsAlive)
            {
                if(Time.realtimeSinceStartup > 8){
                    if(star.GetComponent<Collider>().bounds.Contains(p.position)){
                        star.SendMessage("AbsorbedParticle", j);
                        p.remainingLifetime = -1;
                    }
                }
                j++;
            }

            if(p.startColor.a == 150){
                p.startColor = Random.ColorHSV(0f, 1f, 0f, 0.3f, 0.5f, 1f, 1f, 1f);
            }
            allParticles[i] = p;
        }
        
        for (int i = 0; i < numberOut; i++)
        {
            ParticleSystem.Particle p = outside[i];
            p.remainingLifetime = -1;
            outside[i] = p;
        }
        
        debrisParticles.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, allParticles);
        debrisParticles.SetTriggerParticles(ParticleSystemTriggerEventType.Outside, outside);
    }
}