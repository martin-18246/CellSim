using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;


public class SimManagerBehavior: MonoBehaviour
{
    public GameObject cellPrefab;
    public GameObject foodPrefab;
    public GameObject reproSupernova;
    public GameObject diseasePrefab;
    public TextMeshPro textMeshPro;
    public Material maleMaterial;
    public Material femaleMaterial;



    private float factorDisease = 0.5f;
    private int iterationNum = 0;
    


    void Start()
    {
        spawnObjects(cellPrefab, Info.numCellsStart);
        spawnObjects(foodPrefab, Info.numFoodStart);
    }


    void Update()
    {
        iterationNum++;

        if (iterationNum % Info.foodSpawnFrequency == 0)
        {
            spawnObjects(foodPrefab, 1);
        }

        displayInfo();

        if (Info.displayDiseaseParticles && iterationNum % 25 == 0)
        {
            spawnDisease();
        }
        

        if (Input.GetKeyDown(KeyCode.Alpha1)) // toggle display info
        {
            textMeshPro.enabled = (textMeshPro.enabled) ? false : true; 
        }
    }


    void spawnObjects(GameObject spawnedObject, int numberObjects)
    {
        for (int x = 0; x < numberObjects; x++)
        {
            

            if (spawnedObject == cellPrefab)
            {
                CellObject.createNewCell(true, null, null, cellPrefab, maleMaterial, femaleMaterial, reproSupernova);
                continue;
            }

            if (spawnedObject == foodPrefab)
            {
                GameObject newObject = Instantiate(spawnedObject, createRandomVector(Info.restraintMin, Info.restraintMax), Quaternion.identity);
                Info.foodList.Add(newObject);
                continue;
            }
        }
    }


    void spawnDisease()
    {
        for (int i = 0; i < Info.listCellObject.Count; i++) 
        {
            float rateSquared = (float)Math.Pow(Info.listCellObject[i].diseaseRate, 2);
            float diseaseRatio = (float)rateSquared / 100;
            int numDiseaseCells = (int)(diseaseRatio * factorDisease);

            for (int diseaseCell = 0; diseaseCell < numDiseaseCells; diseaseCell++)
            {
                Instantiate(diseasePrefab, Info.listCellObject[i].positionCell, Quaternion.identity);

            }
        }
    }


    void displayInfo()
    {
        int numCells = Info.listCellObject.Count;
        int femaleCells = 0;
        float totalSpeed = 0;
        float totalDesireToExplore = 0;
        float totalFoodSight = 0;
        float totalPartnerSight = 0;
        float totalDiseaseRate = 0;

        for (int i = 0; i < numCells; i++)
        {
            if (Info.listCellObject[i].gender == "female")
            {
                femaleCells++;
            }

            totalSpeed += Info.listCellObject[i].cellSpeed;
            totalDesireToExplore += Info.listCellObject[i].desireToExplore;
            totalFoodSight += Info.listCellObject[i].foodSight;
            totalPartnerSight += Info.listCellObject[i].partnerSight;
            totalDiseaseRate += Info.listCellObject[i].diseaseRate;

        }

        int maleCells = numCells - femaleCells;

        float averageSpeed = (float)Math.Round(totalSpeed / numCells, 2);
        float averageDesireToExplore = (float)Math.Round(totalDesireToExplore / numCells, 2);
        float averageFoodSight = (float)Math.Round(totalFoodSight / numCells, 2);
        float averagePartnerSight = (float)Math.Round(totalPartnerSight / numCells, 2);
        float averageDiseaseRate = (float)Math.Round(totalDiseaseRate / numCells, 2);

        textMeshPro.text = $"Simulation Stats\n# of iterations: {iterationNum}\n# of cells: {Info.listCellObject.Count} ({maleCells} male; {femaleCells} female)\nAverage speed: {averageSpeed} thousandths of a unit/second\nAverage desire to explore: {averageDesireToExplore}\nAverage food sight: {averageFoodSight}\nAverage partner sight: {averagePartnerSight}\nAverage disease rate: {averageDiseaseRate}\n# Dead cells: {Info.deadDueToOldAge + Info.deadDueToStarvation + Info.deadDueToStillbirth} ({Info.deadDueToOldAge} old age, {Info.deadDueToStarvation} starvation, {Info.deadDueToStillbirth} stillbirth)\n# of born cells: {Info.totalBornCells} ({Info.artificiallyBorn} artificially, {Info.naturallyBorn} naturally)";

    }

    public static void instantiateCell(Vector3 positionCell, GameObject cellPrefab, GameObject reproSupernova)
    {
        Instantiate(cellPrefab, positionCell, Quaternion.identity);
        Instantiate(reproSupernova, positionCell, Quaternion.identity);

    }


    Vector3 createRandomVector(int min1, int max1)
    {
        int posX = UnityEngine.Random.Range(min1, max1);
        int posY = UnityEngine.Random.Range(min1, max1);
        int posZ = UnityEngine.Random.Range(min1, max1);

        Vector3 randomVector = new Vector3(posX, posY, posZ);

        return randomVector;
    }
}
