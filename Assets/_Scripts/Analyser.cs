using UnityEngine;

public class Analyser : MonoBehaviour
{
    public float speed;
    public float moveDistance;

    private bool isLaunched = false;
    private Vector3 topPosition;
    private Vector3 bottomPosition;
    private Vector3 targetPosition;

    public void MoveLaser()
    {
        isLaunched = true;

        bottomPosition = transform.position;
        topPosition = bottomPosition + new Vector3(0, moveDistance, 0);
        targetPosition = topPosition;
    }

    public void StopLaser()
    {
        isLaunched = false;
        transform.position = bottomPosition;
    }

    public void Start()
    {
        if (!isLaunched)
        {
            isLaunched = true;

            bottomPosition = transform.position;
            topPosition = bottomPosition + new Vector3(0, moveDistance, 0);
            targetPosition = topPosition;
        }
    }

    void Update()
    {
        if (!isLaunched) return;

        // Déplacer vers la position cible
        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        // Inverser le mouvement quand la cible est atteinte
        if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
        {
            targetPosition = (targetPosition == topPosition) ? bottomPosition : topPosition;
        }
    }
}
