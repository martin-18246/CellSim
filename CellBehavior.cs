using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Info : MonoBehaviour
{
    // Lists of objects
    public static List<GameObject> foodList = new List<GameObject>();
    public static List<CellObject> listCellObject = new List<CellObject>();
    
    // Information to be displayed
    public static int totalDeadCells = 0;
    public static int deadDueToStarvation = 0;
    public static int deadDueToStillbirth = 0;
    public static int deadDueToOldAge = 0;
    public static int totalBornCells = 0;
    public static int artificiallyBorn = 0;
    public static int naturallyBorn = 0;

    // Num cells and food at start
    public static int numCellsStart = 10;
    public static int numFoodStart = 10;

    // Variable settings
    public static int restraintMin = -50;
    public static int restraintMax = 50;
    public static float cellSpeedMin = 100;
    public static float cellSpeedMax = 200;
    public static float desireToExploreMin = 10;
    public static float desireToExploreMax = 20;
    public static int maximumAgeMin = 3000;
    public static int maximumAgeMax = 6000;
    public static int pauseInReproductionMin = 500;
    public static int pauseInReproductionMax = 1000;
    public static int hungerBarSizeMin = 10;
    public static int hungerBarSizeMax = 10;
    public static int hungerReproductionThresholdMin = 5;
    public static int hungerReproductionThresholdMax = 5;
    public static float foodSightMin = 20;
    public static float foodSightMax = 40;
    public static float partnerSightMin = 50;
    public static float partnerSightMax = 100;
    public static float diseaseRateMin = 0;
    public static float diseaseRateMax = 0;

    public static int hungerDecreaseRate = 250;
    public static int foodSpawnFrequency = 40;

    public static bool displayDiseaseParticles = false;
    public static float sizeDiseaseParticles = 5f;

    public static float cameraOrbitSpeed = 50f;

    public static List<string> genders = new List<string> { "male", "female" };
}


public class CellObject
{
    // Objects
    public CellObject thisCell;
    public CellObject partnerForMating;
    public CellObject parentA;
    public CellObject parentB;

    // Unchanging variables
    public string gender;
    public int ageOfCell;
    public int maximumAge;
    public int pauseInReproduction;
    public int hungerBarSize;
    public int maturityAge = 2000;
    public float baseSize = 0.05f;
    public float adultSize = 5f;
    public float cellSpeed;
    public float desireToExplore;
    public int hungerReproductionThreshold;
    public int hungerDecreaseRate;
    public int intelligence = 50;
    public int diseaseRate;

    // Changing variables
    public Vector3 positionCell;
    public string task;
    public Vector3 cellGoal;
    public int hunger;
    public float foodSight;
    public float partnerSight;
    public bool goingAfterFood;
    public bool lookingForPartner;
    public int matingClock;
    public bool frozen = false;
    public int numIterationsToGoal;
    public int iterationsFromStartToGoal;
    public Vector3 movementVector;
    public int iterationsSinceStart;

    // Materials
    public MeshRenderer meshR;
    public Material maleMaterial;
    public Material femaleMaterial;


    public static void createNewCell(bool randomCell, CellObject parentA, CellObject parentB, GameObject objectToSpawn, Material maleMaterial, Material femaleMaterial, GameObject reproSupernova)
    {
        CellObject thisCell = new CellObject();

        if (randomCell)
        {
            thisCell.positionCell = CellBehavior.createRandomVector(Info.restraintMin, Info.restraintMax);
            thisCell.cellSpeed = UnityEngine.Random.Range(Info.cellSpeedMin, Info.cellSpeedMax);


            thisCell.desireToExplore = UnityEngine.Random.Range(Info.desireToExploreMin, Info.desireToExploreMax);
            thisCell.maximumAge = UnityEngine.Random.Range(Info.maximumAgeMin, Info.maximumAgeMax);
            thisCell.pauseInReproduction = UnityEngine.Random.Range(Info.pauseInReproductionMin, Info.pauseInReproductionMax);
            thisCell.matingClock = thisCell.pauseInReproduction;
            thisCell.hungerBarSize = UnityEngine.Random.Range(Info.hungerBarSizeMin, Info.hungerBarSizeMax);
            thisCell.hunger = thisCell.hungerBarSize;
            thisCell.hungerReproductionThreshold = UnityEngine.Random.Range(Info.hungerReproductionThresholdMin, Info.hungerReproductionThresholdMax);
            thisCell.foodSight = UnityEngine.Random.Range(Info.foodSightMin, Info.foodSightMax);
            thisCell.partnerSight = UnityEngine.Random.Range(Info.partnerSightMin, Info.partnerSightMax);
            thisCell.diseaseRate = (int)UnityEngine.Random.Range(Info.diseaseRateMin, Info.diseaseRateMax);

            SimManagerBehavior.instantiateCell(thisCell.positionCell, objectToSpawn, reproSupernova);
            Info.artificiallyBorn++;
        }

        else
        {
            thisCell.positionCell = parentA.positionCell;
            thisCell.cellSpeed = CellBehavior.normalDistribution((parentA.cellSpeed + parentB.cellSpeed) / 2, 10, Info.cellSpeedMin, Info.cellSpeedMax);
            thisCell.desireToExplore = CellBehavior.normalDistribution((parentA.desireToExplore + parentB.desireToExplore) / 2, 10, Info.desireToExploreMin, Info.desireToExploreMax);
            thisCell.maximumAge = (int)CellBehavior.normalDistribution((parentA.maximumAge + parentB.maximumAge) / 2, 10, Info.maximumAgeMin, Info.maximumAgeMax);
            thisCell.pauseInReproduction = (int)CellBehavior.normalDistribution((parentA.pauseInReproduction + parentB.pauseInReproduction) / 2, 10, Info.pauseInReproductionMin, Info.pauseInReproductionMax);
            thisCell.matingClock = thisCell.pauseInReproduction;
            thisCell.hungerBarSize = (int)CellBehavior.normalDistribution((parentA.hungerBarSize + parentB.hungerBarSize) / 2, 1, Info.hungerBarSizeMin, Info.hungerBarSizeMax);
            thisCell.hunger = thisCell.hungerBarSize;
            thisCell.hungerReproductionThreshold = (int)CellBehavior.normalDistribution((parentA.hungerReproductionThreshold + parentB.hungerReproductionThreshold) / 2, 1, Info.hungerReproductionThresholdMin, Info.hungerReproductionThresholdMax);
            thisCell.foodSight = CellBehavior.normalDistribution((parentA.foodSight + parentB.foodSight) / 2, 10, Info.foodSightMin, Info.foodSightMax);
            thisCell.partnerSight = CellBehavior.normalDistribution((parentA.partnerSight + parentB.partnerSight) / 2, 10, Info.partnerSightMin, Info.partnerSightMax);
            thisCell.diseaseRate = (int)CellBehavior.normalDistribution((parentA.diseaseRate + parentB.diseaseRate) / 2, 1, 0, 0);


            SimManagerBehavior.instantiateCell(parentA.positionCell, objectToSpawn, reproSupernova);
            Info.naturallyBorn++;
        }

        int genderSelection = UnityEngine.Random.Range(0, 2);
        thisCell.gender = Info.genders[genderSelection];

        thisCell.maleMaterial = maleMaterial;
        thisCell.femaleMaterial = femaleMaterial;

        thisCell.lookingForPartner = false;
        thisCell.goingAfterFood = false;
        thisCell.ageOfCell = 0;

        thisCell.task = CellBehavior.chooseTask(thisCell);

        Info.totalBornCells++;
        Info.listCellObject.Add(thisCell);

    }
}



public class CellBehavior : MonoBehaviour
{
    // Game objects
    public GameObject cellObject;
    public GameObject foodObject;
    public GameObject reproSupernova;
    public CellObject cellInList;

    // Materials
    private MeshRenderer meshR;
    public Material maleMaterial;
    public Material femaleMaterial;



    void Start()
    {
        MeshRenderer meshR = GetComponent<MeshRenderer>();
        pairCellObjects();
        putOnMaterial(meshR);
        manageNewGoal();

    }


    void Update()
    {
        resolveIterations();
        resolveMovement();
        resolveHunger();
        resolveMaturity();
       
    }




    void putOnMaterial(MeshRenderer mr)
    {
        if (cellInList.gender == "male")
        {
            mr.material = cellInList.maleMaterial;
        }

        if (cellInList.gender == "female")
        {
            mr.material = cellInList.femaleMaterial;
        }
    }


    void resolveMaturity()
    {
        if (cellInList.ageOfCell >= cellInList.maturityAge)
        {
            transform.localScale = new Vector3(cellInList.adultSize, cellInList.adultSize, cellInList.adultSize);
        }

        else
        {
            float maturityRatio = (float)cellInList.ageOfCell / cellInList.maturityAge;
            float sizeCell = cellInList.baseSize + maturityRatio * (cellInList.adultSize - cellInList.baseSize);
            transform.localScale = new Vector3(sizeCell, sizeCell, sizeCell);
        }
    }


    void resolveIterations()
    {
        cellInList.ageOfCell++;
        cellInList.matingClock--;
        cellInList.iterationsSinceStart++;

        if (cellInList.ageOfCell > cellInList.maximumAge)
        {
            decay(cellInList);
            Info.deadDueToOldAge++;
        }
    }


    void pairCellObjects()
    {
        foreach (var cell1 in Info.listCellObject)
        {
            if (cell1.positionCell == transform.position && cell1.ageOfCell < 10)
            {
                cellInList = cell1;

            }
        }
    }


    void resolveMovement()
    {
        if (!cellInList.frozen)
        {
            Vector3 newPosition = transform.position + cellInList.movementVector;
            transform.position = newPosition;
            cellInList.positionCell = newPosition;
        }
        

        if (cellInList.iterationsSinceStart >= cellInList.numIterationsToGoal - 1)
        {
            if (cellInList.frozen)
            {
                return;
            }
            transform.position = cellInList.cellGoal;
            cellInList.positionCell = cellInList.cellGoal;

            if (cellInList.task == "eat")
            {
                eatFood();
            }

            if (cellInList.task == "reproduce" && cellInList.gender == "male" && cellInList.partnerForMating != null)
            {
                mate(cellInList, cellInList.partnerForMating);
            }
            
            manageNewGoal();
        }
    }


    void mate(CellObject maleParent, CellObject femaleParent)
    {
        float probabilityOfSurvival = 100 - ((maleParent.diseaseRate + femaleParent.diseaseRate) / 2);

        int randomThrow = UnityEngine.Random.Range(0, 100);

        if (randomThrow < probabilityOfSurvival)
        {
            CellObject.createNewCell(false, maleParent, femaleParent, cellObject, maleMaterial, femaleMaterial, reproSupernova);

        }

        else
        {
            Info.naturallyBorn++;
            Info.deadDueToStillbirth++;
        }




        maleParent.matingClock = maleParent.pauseInReproduction;
        femaleParent.matingClock = femaleParent.pauseInReproduction;

        femaleParent.frozen = false;

        femaleParent.task = chooseTask(femaleParent);
        maleParent.task = chooseTask(maleParent);


        maleParent.lookingForPartner = false;
        femaleParent.lookingForPartner = false;

        maleParent.partnerForMating = null;
        femaleParent.partnerForMating = null;
        



    }


    void eatFood()
    {
        if (cellInList.goingAfterFood == true)
        {
            cellInList.goingAfterFood = false;
            Vector3 locationNow = transform.position;

            foreach (var food1 in Info.foodList)
            {
                if (food1.transform.position == locationNow)
                {
                    Info.foodList.Remove(food1);
                    Destroy(food1);
                    cellInList.hunger++;
                    return;
                }
            }
        }

    }


    void resolveHunger()
    {
        if (cellInList.ageOfCell % Info.hungerDecreaseRate == 0)
        {
            cellInList.hunger -= 1;
        }

        if (cellInList.hunger <= 0)
        {

            Info.deadDueToStarvation++;
            decay(cellInList);
        }
    }


    void decay(CellObject deadCell)
    {
        Destroy(cellObject);
        Info.listCellObject.Remove(deadCell);

        if (deadCell.gender == "male" && deadCell.partnerForMating != null)
        {
            CellObject partnerCell = deadCell.partnerForMating;
            partnerCell.task = chooseTask(partnerCell);
            configureNewGoal(new Vector3(0, 0, 0), partnerCell);
            partnerCell.partnerForMating = null;
        }
    }


    public static string chooseTask(CellObject cellInList1)
    {

        if (cellInList1.hunger < cellInList1.hungerReproductionThreshold || cellInList1.matingClock > 0 || cellInList1.ageOfCell < cellInList1.maturityAge)
        {
            return "eat";
        }

        else
        {
            if (cellInList1.task == "eat")
            {
                cellInList1.lookingForPartner = true;
            }
            return "reproduce";
        }
    }


    void manageNewGoal()
    {
        cellInList.task = chooseTask(cellInList);
        cellInList.cellGoal = selectGoal();
        configureNewGoal(cellInList.cellGoal, cellInList);

    }


    Vector3 selectGoal()
    {
        if (cellInList.task == "eat")
        {
            return selectFoodGoal();
        }

        if (cellInList.task == "wait" || cellInList.frozen == true)
        {
            return cellInList.positionCell;
        }

        if (cellInList.task == "reproduce" && cellInList.gender == "male")
        {
            return selectPartnerGoal();
        }

        return selectExplorationGoal();

    }


    Vector3 selectFoodGoal()
    {
        Vector3 closestFoodLocation = new Vector3(0, 0, 0);
        Vector3 myLocation = transform.position;
        int indexClosestFood = int.MaxValue;
        float distanceToClosestFood = float.PositiveInfinity;
        GameObject chosenFood = null;

        int lenFoodList = Info.foodList.Count;

        for (int foodObject = 0; foodObject < lenFoodList; foodObject++)
        {
            FoodBehavior foodBehavior = Info.foodList[foodObject].GetComponent<FoodBehavior>();
            float distanceToFood = Vector3.Distance(myLocation, Info.foodList[foodObject].transform.position);

            if (distanceToClosestFood > distanceToFood)
            {
                int iterationsToReach = calculateNumIterations(closestFoodLocation);
                int intelligenceRollADie = UnityEngine.Random.Range(0, 100);
                if (!foodBehavior.isTaken || foodBehavior.iterationsTillEaten > iterationsToReach || cellInList.intelligence < intelligenceRollADie)
                {
                    

                    distanceToClosestFood = distanceToFood;
                    closestFoodLocation = Info.foodList[foodObject].transform.position;
                    indexClosestFood = foodObject;
                    chosenFood = Info.foodList[foodObject];
                }

            }
        }


        if (distanceToClosestFood <= cellInList.foodSight)
        {
            FoodBehavior foodGoalBehavior = chosenFood.GetComponent<FoodBehavior>();
            foodGoalBehavior.isTaken = true;
            int iterationsTillArrival = calculateNumIterations(closestFoodLocation);

            if (iterationsTillArrival < foodGoalBehavior.iterationsTillEaten)
            {
                foodGoalBehavior.iterationsTillEaten = iterationsTillArrival;
            }
             
            cellInList.goingAfterFood = true;
            return closestFoodLocation;

        }

        else
        {
            //return closestFoodLocation;
            return selectExplorationGoal();
        }

        
    }


    Vector3 selectPartnerGoal() 
    {
        Vector3 closestPartnerToMate = new Vector3(0, 0, 0);
        Vector3 myLocation = cellInList.positionCell;
        int indexClosestPartner = int.MaxValue;
        float distanceToClosestPartner = float.PositiveInfinity;

        int lenPartnerList = Info.listCellObject.Count;



        for (int partnerObject = 0; partnerObject < lenPartnerList; partnerObject++)
        {
            float distanceToPartner = Vector3.Distance(myLocation, Info.listCellObject[partnerObject].positionCell);

            if (Info.listCellObject[partnerObject].gender == "female" && Info.listCellObject[partnerObject].task == "reproduce" && Info.listCellObject[partnerObject].lookingForPartner && distanceToClosestPartner > distanceToPartner)
            {
                distanceToClosestPartner = distanceToPartner;
                closestPartnerToMate = Info.listCellObject[partnerObject].positionCell;
                indexClosestPartner = partnerObject;
            }
        }

        if (distanceToClosestPartner <= cellInList.partnerSight)
        {
            cellInList.partnerForMating = Info.listCellObject[indexClosestPartner];
            cellInList.partnerForMating.task = "wait";
            cellInList.partnerForMating.frozen = true;

            return closestPartnerToMate;

        }

        else
        {
            return selectExplorationGoal();
        }



    }


    Vector3 selectExplorationGoal()
    {

        while (true)
        {
            Vector3 directionGoal = UnityEngine.Random.insideUnitSphere;
            Vector3 normalizedVectorGoal = directionGoal.normalized;
            Vector3 proposedGoal = normalizedVectorGoal * cellInList.desireToExplore + transform.position;

            if (isInConstraints(proposedGoal))
            {
                configureNewGoal(proposedGoal, cellInList);
                return proposedGoal;
            }



        }
    }


    void configureNewGoal(Vector3 proposedGoal, CellObject theCell)
    {
        theCell.numIterationsToGoal = calculateNumIterations(proposedGoal);
        theCell.iterationsSinceStart = 0;

        Vector3 wholeVector = theCell.cellGoal - theCell.positionCell;
        Vector3 singleIterationVector = wholeVector / theCell.numIterationsToGoal;

        theCell.movementVector = singleIterationVector;
    }


    bool isInConstraints(Vector3 vector1)
    {

        if (vector1.x > Info.restraintMin && vector1.x < Info.restraintMax)
        {
            if (vector1.y > Info.restraintMin && vector1.y < Info.restraintMax)
            {
                if (vector1.z > Info.restraintMin && vector1.z < Info.restraintMax)
                {
                    return true;
                }
            }
        }

        return false;
    }


    int calculateNumIterations(Vector3 selectedGoal)
    {
        double distanceToGoal = Vector3.Distance(selectedGoal, transform.position);
        int iterationsToReachGoal = (int)((distanceToGoal / cellInList.cellSpeed) * 1000 + 1);

        return iterationsToReachGoal;
    }


    public static Vector3 createRandomVector(int min1, int max1)
    {
        int posX = UnityEngine.Random.Range(min1, max1);
        int posY = UnityEngine.Random.Range(min1, max1);
        int posZ = UnityEngine.Random.Range(min1, max1);

        Vector3 randomVector = new Vector3(posX, posY, posZ);

        return randomVector;
    }


    public static float normalDistribution(float mean, float standardDeviation, float minValue, float maxValue)
    {
        float u1 = UnityEngine.Random.Range(0, 1);
        float u2 = UnityEngine.Random.Range(0, 1);
        float z1 = (float)(Math.Sqrt(-2f * Math.Log(u1)) * Math.Cos(2f * Math.PI * u2));

        float result = (float)mean + z1 * standardDeviation;

        if (result > maxValue)
        {
            result = maxValue;
        }

        if (result < minValue)
        {
            result = minValue;
        }

        return result;

    }


}

