using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiseaseScript : MonoBehaviour
{
    public GameObject diseasePrefab;
    public int itersToGoal = 50;
    public int iterationNum = 0;
    private Vector3 initialPosition;
    private Vector3 initialScale;
    public Vector3 singleMovementVector;


    void Start()
    {
        
        Vector3 randomDirection = createRandomVector(-2f, 2f);

        initialPosition = transform.position;
        initialScale = transform.localScale;

        Vector3 goalDisease = initialPosition + randomDirection * Info.sizeDiseaseParticles;
        singleMovementVector = (goalDisease - initialPosition) / itersToGoal;
        

    }


    void Update()
    {
        iterationNum++;

        moveDisease();

        if (iterationNum >= itersToGoal)
        {
            Destroy(diseasePrefab);
        }

    }

    void moveDisease()
    {
        transform.position = initialPosition + iterationNum * singleMovementVector;
        transform.localScale = initialScale - (itersToGoal - iterationNum) * initialScale;
    }

    Vector3 createRandomVector(float min1, float max1)
    {
        float posX = UnityEngine.Random.Range(min1, max1);
        float posY = UnityEngine.Random.Range(min1, max1);
        float posZ = UnityEngine.Random.Range(min1, max1);

        Vector3 randomVector = new Vector3(posX, posY, posZ);

        return randomVector;
    }
}
