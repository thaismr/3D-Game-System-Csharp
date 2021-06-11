using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityStandardAssets.Characters.FirstPerson;

public class Player : MonoBehaviour 
{
	
	bool raycastOn = true;		// is player raycasting?

    bool inLift = false;        // is player inside lift?

    bool liftDown = false;      // lift going down?

	string rayObjectName;

	public Sprite target_idle;

	public Sprite target_interact;

	public Button targetButton;		// image to highlight near interactive targets

    Animator lift;                  // lift animator component

    Transform liftPos;              // lift transform component

	Plot plot;						// register plot under raycast

	Artifact artifact;              // register artifact under raycast

    Silo silo;                      // register silo under raycast


    CharacterController character;	// get controller so player can be freezed

	public GameObject[] gameMenus;		// List of menus to check open & freeze player


	void Start()
	{
		character = GetComponent<CharacterController> ();
	}

	void Update()
	{
		bool freeze = false;

		if (gameMenus.Length > 0) 		// check if list of menus has items
		{
			foreach (GameObject menu in gameMenus) 
			{
                if (menu.activeInHierarchy)
                {
                    freeze = true;
                }
			}
		}

		character.enabled = !freeze;

		CursorState (freeze);


        if (Input.GetKeyDown(GameManager.KEY_CODE_LIFT) && inLift)

            MoveLift();


        if (inLift && lift.GetBool("isMoving"))
        {
            transform.position = liftPos.position + new Vector3(0,2,0);
        }




        if (raycastOn && !freeze) 		// if raycast is on :
		{			
			RaycastHit hit;

			if (Physics.Raycast (Camera.main.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0f)), out hit, 5)) 
			{
				
				plot = hit.collider.GetComponent<Plot> ();

				artifact = hit.collider.GetComponent<Artifact> ();

                silo = hit.collider.GetComponent<Silo>();


				if (plot || artifact || silo)
					targetButton.GetComponent<Image> ().sprite = target_interact;
				else
					targetButton.GetComponent<Image> ().sprite = target_idle;
				

				if (Input.GetKeyDown (GameManager.KEY_CODE_INTERACT) || Input.GetKeyDown (GameManager.KEY_CODE_INTERACT_ALT)) 
				{
					if (plot) 
					{
						GameManager.GM.activePlot = plot;

						plot.Interact ();
					}
					else if (artifact) 
					{
						GameManager.GM.activeArtifact = artifact;

						GameManager.GM.ShowInteractMenu(true);
					}
                    else if (silo)
                    {
                        GameManager.GM.activeSilo = silo;

                        GameManager.GM.ShowSiloMenu(true);
                    }
                }

			}
		}

	}

	void CursorState(bool show) 
	{
		Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
		Cursor.visible = show;
	}


	public void OnTriggerEnter(Collider other)
	{

		if (other.CompareTag (Fog.TAG))		// die when touched by fog
			
			GameManager.GM.Restart ();

		if (other.CompareTag (Trigger.TAG))	// start dialogue at triggers

			other.GetComponent<Trigger> ().Dialogue();

        if (other.GetComponent<Lift>())      // player is inside lift
        {
            inLift = true;

            lift = other.GetComponent<Animator>();

            liftPos = other.GetComponent<Transform>();
        }

        if (other.GetComponent<Artifact>())

			GameManager.GM.CheckMissionCollision(other.GetComponent<Artifact>().itemID);

	}

    public void OnTriggerExit(Collider other)
    {

        if (other.GetComponent<Lift>())      // player exited lift

            inLift = false;

    }


    void MoveLift()
    {
        if (liftDown)
        {
            lift.SetBool("Down", false);
            liftDown = false;
        }
        else
        {
            lift.SetBool("Down", true);
            liftDown = true;
        }
    }


}
