using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] earthObjects;
    GameObject currentObject;   //The object which will spawn next
    int chosenPositionInArray;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {  //Change to a different input or automatic once last object despawned
            chosenPositionInArray = Random.Range(0, earthObjects.Length);   //selects which index in the list of objects to choose
            currentObject = earthObjects[chosenPositionInArray];    //selects the object that is in the chosen index as the one to spawn
            AbductAnObject();
        }
    }

    public void AbductAnObject()
    {
        Instantiate(currentObject, transform.position, Quaternion.identity); //spawns the prefab at the position of the spawn point
    }
}
