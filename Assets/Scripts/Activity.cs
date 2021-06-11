using UnityEngine;

/// <summary>
/// 
/// ACTIVITY : Located in objects that perform Actions triggered by events
/// 
/// </summary>


public class Activity : MonoBehaviour
{

    public enum actionTrigger
    {
        emergencyAlarmOn,
        emergencyAlarmOff,
        escapeDoorLock,
        escapeDoorUnlock
    };

    AudioSource[] alarmSounds;                  // emergency alarm sounds

    Light[] emergencyLights;                    // emergency alarm lights

    Animator animator;                          // animation controller



    void Start()
    {
        alarmSounds = GetComponentsInChildren<AudioSource>();

        emergencyLights = GetComponentsInChildren<Light>();

        animator = GetComponent<Animator>();
    }


    // Functions called from Game Manager:

    public void EmergencyAlarmOn()
    {
        foreach (AudioSource alarm in alarmSounds)
        {
            alarm.Play();
        }
        foreach (Light light in emergencyLights)
        {
            light.enabled = true;
        }
    }

    public void EmergencyAlarmOff()
    {
        foreach (AudioSource alarm in alarmSounds)
        {
            alarm.Stop();
        }
        foreach (Light light in emergencyLights)
        {
            light.enabled = false;
        }
    }

    public void DoorLock()
    {
        animator.SetBool("Lock", true);
    }

    public void DoorUnlock()
    {
        animator.SetBool("Lock", false);
    }

}
