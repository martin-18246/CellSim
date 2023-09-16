using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodBehavior : MonoBehaviour
{
    public bool isTaken;
    public int iterationsTillEaten = -10;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (iterationsTillEaten > 0) 
        {
            iterationsTillEaten--;
        }
    }
}
