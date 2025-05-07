using System.Collections.Generic;
using UnityEngine;

public class PeopleManagerEnd : MonoBehaviour
{
    [Header("People")]
    public List<GameObject> peopleToDance = new List<GameObject>();

    [Header("Animations")]
    public RuntimeAnimatorController danceAnimation;

    private bool hasStartedDancing = false;

    // Ensure all people have an Animator component
    void Start()
    {
        if (peopleToDance.Count == 0)
        {
            Debug.LogWarning("No people assigned in PeopleManagerEnd.");
            return;
        }

        foreach (GameObject person in peopleToDance)
        {
            if (person == null) continue;

            Animator animator = person.GetComponent<Animator>();
            if (animator == null)
                animator = person.AddComponent<Animator>();
        }
    }

    // When the game is won, assign the dance animation once
    void Update()
    {
        if (!hasStartedDancing && GameManager.Instance != null && GameManager.Instance.isWon)
        {
            foreach (GameObject person in peopleToDance)
            {
                if (person == null) continue;

                Animator animator = person.GetComponent<Animator>();
                if (animator != null && danceAnimation != null)
                {
                    animator.runtimeAnimatorController = danceAnimation;
                }
            }

            hasStartedDancing = true;
        }
    }
}
