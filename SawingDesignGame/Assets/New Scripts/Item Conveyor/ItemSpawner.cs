using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] earthObjects;
    public float conveyorSpeed;
    GameObject currentObject;   //The object which will spawn next
    int chosenPositionInArray;
    float playTimer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody objectRB;
    public ParticleSystem beamingParticles;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {  //Change to a different input or automatic once last object despawned
            if (currentObject == null)
            {
                chosenPositionInArray = Random.Range(0, earthObjects.Length);   //selects which index in the list of objects to choose
                currentObject = earthObjects[chosenPositionInArray];    //selects the object that is in the chosen index as the one to spawn
               // playTimer += playTimer + 1 * Time.deltaTime;
               // Debug.Log(playTimer);
               // if (playTimer > 3)
               // {
                    StartParticles();
                    AbductAnObject();

               // }
            }


            //if(currentObject != null && inCenter == false)  //when there is an object in the scene but it is not in the pickup area of the belt
            //{
            //    objectRB = currentObject.GetComponent<Rigidbody>(); //get the rigidbody from the currently present cuttable object
            //    objectRB.AddForce(0, 0, conveyorSpeed, ForceMode.Force);    //applies the force in the direction of down the conveyor
            //    Debug.Log("Force being applied");
            //}

            //if(currentObject.position)
        }
    }

    public void StartParticles()
    {
        beamingParticles.Play();
    }


    public void AbductAnObject()
    {
        Instantiate(currentObject, transform.position, Quaternion.identity); //spawns the prefab at the position of the spawn point

    }





}
