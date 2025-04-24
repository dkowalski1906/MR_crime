using UnityEditor.Animations;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PeopleManager : MonoBehaviour
{
    public List<Transform> targets = new List<Transform>();             // Liste des points à visiter
    public List<GameObject> peopleToMove = new List<GameObject>();      // Liste des personnages
    public AnimatorController walkAnimation;                            // Animation de marche
    public AnimatorController lookAnimation;                            // Animation d'observation

    public float speed = 2f;
    public float stopDistance = 0.5f;
    public float rotationSpeed = 5f;
    public float waitTime = 5f;

    private class PersonState
    {
        public GameObject person;
        public Animator animator;
        public Transform target;
        public bool isWaiting;
    }

    private List<PersonState> personStates = new List<PersonState>();

    void Start()
    {
        if (targets.Count == 0)
        {
            Debug.LogWarning("Aucune cible définie !");
            return;
        }

        foreach (GameObject person in peopleToMove)
        {
            if (person == null) continue;

            Animator animator = person.GetComponent<Animator>();
            if (animator == null)
                animator = person.AddComponent<Animator>();

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

    void Update()
    {
        foreach (PersonState state in personStates)
        {
            if (state.isWaiting || state.person == null || state.target == null)
                continue;

            Vector3 direction = state.target.position - state.person.transform.position;
            direction.y = 0f;

            if (direction.magnitude > stopDistance)
            {
                // Move
                state.person.transform.position = Vector3.MoveTowards(
                    state.person.transform.position,
                    state.target.position,
                    speed * Time.deltaTime
                );

                // Rotate
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

    IEnumerator HandleArrival(PersonState state)
    {
        state.isWaiting = true;

        // Change animation
        if (state.animator != null && lookAnimation != null)
            state.animator.runtimeAnimatorController = lookAnimation;

        // Calculer la rotation intermédiaire entre l'actuelle et celle de la cible
        Quaternion targetRotation = Quaternion.Euler(0f, state.target.eulerAngles.y, 0f);
        // Interpoler de la position actuelle vers la rotation cible, à moitié (0.5f)
        Quaternion intermediateRotation = Quaternion.Slerp(state.person.transform.rotation, targetRotation, 0.5f);
        state.person.transform.rotation = intermediateRotation;

        yield return new WaitForSeconds(waitTime);

        // Nouvelle cible
        state.target = GetNewRandomTarget(state.target);

        // Reprend l'animation de marche
        if (state.animator != null && walkAnimation != null)
            state.animator.runtimeAnimatorController = walkAnimation;

        state.isWaiting = false;
    }



    Transform GetNewRandomTarget(Transform exclude)
    {
        if (targets.Count == 0) return null;

        Transform newTarget;
        do
        {
            newTarget = targets[Random.Range(0, targets.Count)];
        } while (newTarget == exclude && targets.Count > 1); // Évite de reprendre la même

        return newTarget;
    }
}
