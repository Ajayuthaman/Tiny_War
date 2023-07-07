using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class SpawnManager : MonoBehaviour
{
    public GameObject blueInfantryPrefab; // Prefab for blue team infantry unit
    public GameObject redInfantryPrefab; // Prefab for red team infantry unit

    private bool isSpawning = false; // Flag to check if a spawn is in progress
    private List<GameObject> blueUnits = new List<GameObject>(); // List to store blue team units
    private List<GameObject> redUnits = new List<GameObject>(); // List to store red team units

    private int blueTeamCount = 0; // Counter for blue team units
    private int redTeamCount = 0; // Counter for red team units
    private const int maxTeamCount = 5; // Maximum number of units per team

    private bool gameStart;

    public Animator animator;

    private void Start()
    {
        animator.enabled = true; 
    }

    private void Update()
    {
        if (!gameStart)
        {
            if (!isSpawning && Input.GetMouseButtonDown(0) && blueTeamCount < maxTeamCount)
            {
                SpawnUnit(blueInfantryPrefab, blueUnits);
                blueTeamCount++;
            }
            else if (!isSpawning && Input.GetMouseButtonDown(1) && redTeamCount < maxTeamCount)
            {
                SpawnUnit(redInfantryPrefab, redUnits);
                redTeamCount++;
            }
        }

        if (Input.GetKey(KeyCode.Space))
        {
            gameStart = true;
        }

        if (gameStart)
        {
            // Update the targets for blue team units
            AssignTargets(blueUnits, redUnits);

            // Update the targets for red team units
            AssignTargets(redUnits, blueUnits);
        }
    }

    private void SpawnUnit(GameObject prefab, List<GameObject> unitList)
    {
        isSpawning = true; // Set the flag to indicate a spawn is in progress

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 spawnPosition = hit.point;
            Quaternion spawnRotation = Quaternion.identity;

            // Instantiate the infantry unit prefab
            GameObject unit = Instantiate(prefab, spawnPosition, spawnRotation);

            // Set the team tag for collision detection
            unit.tag = (prefab == blueInfantryPrefab) ? "BlueTeam" : "RedTeam";

            // Add the unit to the respective team unit list
            unitList.Add(unit);
        }

        isSpawning = false; // Reset the flag after the spawn is complete
    }

    private void AssignTargets(List<GameObject> units, List<GameObject> targets)
    {
        foreach (GameObject unit in units)
        {
            if (unit != null)
            {
                GameObject closestTarget = FindClosestUnit(unit.transform.position, targets);
                if (closestTarget != null)
                {
                    AIPath aiPath = unit.GetComponent<AIPath>();
                    if (aiPath != null)
                    {
                        // Check if the unit has reached the end of its path
                        if (aiPath.reachedEndOfPath && aiPath.remainingDistance <= aiPath.endReachedDistance)
                        {
                            // Start the attack function here
                            AttackFunction(unit, closestTarget);
                        }
                        else
                        {
                            Animator animB = unit.GetComponent<Animator>();
                            animB.SetBool("Walk", true);
                            animB.SetBool("Attack", false);

                            // Set the destination as the closest target's position
                            aiPath.destination = closestTarget.transform.position;
                        }
                    }
                }
            }
        }
    }

    private GameObject FindClosestUnit(Vector3 position, List<GameObject> units)
    {
        GameObject closestUnit = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject unit in units)
        {
            if (unit != null)
            {
                float distance = Vector3.Distance(position, unit.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestUnit = unit;
                }
            }
        }

        return closestUnit;
    }

    public void AttackFunction(GameObject unit, GameObject target)
    {
        StartCoroutine(AttackCoroutine(unit, target));
    }

    private IEnumerator AttackCoroutine(GameObject unit, GameObject target)
    {
        Animator animB = unit.GetComponent<Animator>();
        animB.SetBool("Walk", false);
        animB.SetBool("Attack", true);

        // Face the target
        Vector3 direction = target.transform.position - unit.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        unit.transform.rotation = targetRotation;

        // Wait until the target is disabled
        while (target != null && target.activeSelf)
        {
            yield return null;
        }

        // Target is disabled, stop attacking
        animB.SetBool("Attack", false);

        // Check if the unit is still valid and if there are remaining targets
        if (unit != null && target == null)
        {
            // Get the updated list of targets (replace with your logic)
            List<GameObject> updatedTargets = new List<GameObject>();

            // Check if there are remaining targets
            if (updatedTargets.Count > 0)
            {
                GameObject newTarget = FindClosestUnit(unit.transform.position, updatedTargets);
                AttackFunction(unit, newTarget);
            }
            else
            {
                // No more targets, resume moving
                animB.SetBool("Walk", true);
                animB.SetBool("Attack", false);
                AIPath aiPath = unit.GetComponent<AIPath>();
                if (aiPath != null)
                {
                    aiPath.destination = Vector3.zero; // Set a new destination for movement
                }
            }
        }
    }

}
