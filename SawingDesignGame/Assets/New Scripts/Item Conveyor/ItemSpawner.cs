using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] earthObjects;
    public float conveyorSpeed;
    GameObject currentObject;   //The object which will spawn next
    int chosenPositionInArray;
    float playTimer = 0;
    bool objectSpawned;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public ParticleSystem beamingParticles;

    void Start()
    {
        objectSpawned = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {  //Change to a different input or automatic once last object despawned
            //if (currentObject == null)
           // {
                chosenPositionInArray = Random.Range(0, earthObjects.Length);   //selects which index in the list of objects to choose
                currentObject = earthObjects[chosenPositionInArray];    //selects the object that is in the chosen index as the one to spawn
                    AbductAnObject();
                objectSpawned = true;
           // }
        }

        if (objectSpawned == true) {
            playTimer += (playTimer + 0.1f) * Time.deltaTime;
            Debug.Log(playTimer);
        }

        if (playTimer >= 6) {
            beamingParticles.Pause();
            beamingParticles.Clear();
            objectSpawned = false;
            playTimer = 0;
        }

    }

    public void AbductAnObject()
    {   
        beamingParticles.Play();
        
        Instantiate(currentObject, transform.position, Quaternion.identity); //spawns the prefab at the position of the spawn point
    }





}
