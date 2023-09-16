using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReproBehavior : MonoBehaviour
{
    private int iteration1;
    public float enlargementSpeed = 0.002f;
    public int maxIteration = 100;
    public GameObject thisThing;


    void Start()
    {
        iteration1 = 0;
    }


    void Update()
    {
        iteration1++;
        if (iteration1 > maxIteration)
        {
            Destroy(thisThing);
        }

        Vector3 currentSize = transform.localScale;
        currentSize.x += enlargementSpeed;
        currentSize.y += enlargementSpeed;
        currentSize.z += enlargementSpeed;

        transform.localScale = currentSize;

    }
}
