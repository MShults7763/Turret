using System.Collections;
using UnityEngine;

public class SecurityCamera : MonoBehaviour
{
    [SerializeField] private float lookInterval = 0.1f;
    [Range(30, 110)]
    [SerializeField] private float fieldOfView = 75;
    private Transform emitter;
    private GameObject player;
    private bool canSeePlayer = false;

    // Start is called before the first frame update
    void Start()
    {
        emitter = transform.GetChild(0);
        player = GameObject.Find("FirstPersonController");
        StartCoroutine(CheckForBadGuys());
    }

    IEnumerator CheckForBadGuys()
    {
        while (true)
        {
            yield return new WaitForSeconds(lookInterval);

            // Check for player
            // Draw a ray from the emitter to the player
            Vector3 targetDir = player.transform.position - emitter.position;
            Ray ray = new Ray(emitter.position, targetDir);

            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.transform.gameObject.CompareTag("Player"))
                {
                    // Testing to see if the player is in front of the camera
                    float angle = Vector3.Angle(targetDir, emitter.forward);

                    if (angle < fieldOfView * 1.5f)
                    {
                        Debug.Log("Found a bad guy!");
                        // turn all the turrets on.
                        StartCoroutine(CallTurrets());
                        Debug.DrawRay(emitter.position, targetDir, Color.green, 4);
                    }
                    else
                    {
                        canSeePlayer = false;
                        Debug.DrawRay(emitter.position, targetDir, Color.yellow, 4);
                    }
                }
                else
                {
                    Debug.DrawRay(emitter.position, targetDir, Color.red, 4);
                    canSeePlayer = false;
                }
            }
        }
    }

    IEnumerator CallTurrets()
    {
        if(canSeePlayer == false)
        {
            canSeePlayer = true;
            yield return new WaitForSeconds(1);
            // if we can still see the player 1 second later.
            if(canSeePlayer)
            {
                // find all of the turrets, and turn them on.
                GameObject[] turrets = GameObject.FindGameObjectsWithTag("Turret");
                foreach(GameObject turret in turrets)
                {
                    turret.GetComponent<TurretController>().Activate();
                }
            }
        }
        // else, don't run  this code
    }
}

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SecurityCamera : MonoBehaviour
//{
//    [SerializeField] private float lookInterval = 0.1f;
//    [Range(30, 110)]
//    [SerializeField] private float fieldOfView = 75;
//    private Transform emitter;
//    private GameObject player;


//    // Start is called before the first frame update
//    void Start()
//    {
//        emitter = this.transform.GetChild(0);
//        player = GameObject.Find("FirstPersonController");
//        StartCoroutine(CheckForBadGuys());
//    }

//    IEnumerable CheckForBadGuys()
//    {
//        while (true)
//        {
//            yield return new WaitForSeconds(lookInterval);

//            // Check for player
//            // draw a ray from the emitter to player
//            Ray ray = new Ray(emitter.position, player.transform.position - emitter.position);
//            RaycastHit hit;
//            if (Physics.Raycast(ray, out hit, 100))
//            {
//                if (hit.transform.gameObject.CompareTag("Player"))
//                {
//                    //testing to see if player is in front of the camera
//                    Vector3 targetDir = player.transform.position - emitter.position;
//                    float angle = Vector3.Angle(targetDir, emitter.forward);

//                    if (angle < fieldOfView)
//                    {
//                        Debug.Log("Found a bad guy!");
//                        Debug.DrawRay(emitter.position, player.transform.position - emitter.position, Color.green, 4);
//                    }
//                    else
//                    {
//                        Debug.DrawRay(emitter.position, player.transform.position - emitter.position, Color.yellow, 4);
//                    }

//                }

//                else
//                {
//                    Debug.DrawRay(emitter.position, player.transform.position - emitter.position, Color.red, 4);
//                }
//            }
//        }
//    }


//}
