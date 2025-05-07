using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PeopleManager : MonoBehaviour
{
    [Header("People & Targets")]
    public List<Transform> targets = new List<Transform>();
    public List<GameObject> peopleToMove = new List<GameObject>();

    [Header("Animations")]
    public RuntimeAnimatorController walkAnimation;
    public RuntimeAnimatorController lookAnimation;

    [Header("Movement Settings")]
    public float speed = 2f;
    public float stopDistance = 0.5f;
    public float rotationSpeed = 5f;
    public float waitTime = 5f;

    // Class to hold state for each moving person
    private class PersonState
    {
        public GameObject person;
        public Animator animator;
        public Transform target;
        public bool isWaiting;
    }

    private List<PersonState> personStates = new List<PersonState>();

    // Initialization: assign walk animation and random target to each person
    void Start()
    {
        if (targets.Count == 0)
        {
            Debug.LogWarning("No targets assigned in PeopleManager.");
            return;
        }

        foreach (GameObject person in peopleToMove)
        {
            if (person == null) continue;

            Animator animator = person.GetComponent<Animator>() ?? person.AddComponent<Animator>();
            if (walkAnimation != null)
                animator.runtimeAnimatorController = walkAnimation;

            var state = new PersonState
            {
                person = person,
                animator = animator,
                target = GetNewRandomTarget(null),
                isWaiting = false
            };

            personStates.Add(state);
        }
    }

    // Frame update: move and rotate each person toward their target
    void Update()
    {
        foreach (PersonState state in personStates)
        {
            if (state.isWaiting || state.person == null || state.target == null)
                continue;

            Vector3 direction = state.target.position - state.person.transform.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > stopDistance * stopDistance)
            {
                // Move forward
                state.person.transform.position = Vector3.MoveTowards(
                    state.person.transform.position,
                    state.target.position,
                    speed * Time.deltaTime
                );

                // Rotate toward target
                if (direction != Vector3.zero)
                {
                    Quaternion lookRotation = Quaternion.LookRotation(direction);
                    state.person.transform.rotation = Quaternion.Slerp(
                        state.person.transform.rotation,
                        lookRotation,
                        rotationSpeed * Time.deltaTime
                    );
                }
            }
            else
            {
                StartCoroutine(HandleArrival(state));
            }
        }
    }

    // Handle logic when a person reaches their target
    private IEnumerator HandleArrival(PersonState state)
    {
        state.isWaiting = true;

        // Switch to "look" animation
        if (state.animator != null && lookAnimation != null)
            state.animator.runtimeAnimatorController = lookAnimation;

        // Slight rotation toward target's forward direction
        Quaternion targetRotation = Quaternion.Euler(0f, state.target.eulerAngles.y, 0f);
        Quaternion intermediateRotation = Quaternion.Slerp(state.person.transform.rotation, targetRotation, 0.5f);
        state.person.transform.rotation = intermediateRotation;

        // Wait for a moment
        yield return new WaitForSeconds(waitTime);

        // Assign a new random target (different from the current one)
        state.target = GetNewRandomTarget(state.target);

        // Switch back to walking animation
        if (state.animator != null && walkAnimation != null)
            state.animator.runtimeAnimatorController = walkAnimation;

        state.isWaiting = false;
    }

    // Pick a new target that isn't the current one
    private Transform GetNewRandomTarget(Transform exclude)
    {
        if (targets.Count == 0) return null;

        Transform newTarget;
        do
        {
            newTarget = targets[Random.Range(0, targets.Count)];
        } while (newTarget == exclude && targets.Count > 1);

        return newTarget;
    }
}
