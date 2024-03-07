using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public float moveSpeed = 5f; // Snelheid van het object
    private Vector3 targetPosition; // Doelpositie

    // Update wordt elke frame aangeroepen
    void Update()
    {
        // Controleer of de doelpositie is ingesteld en beweeg naar die positie
        if (targetPosition != null)
        {
            MoveToTarget();
        }
    }

    // Functie om het object naar de doelpositie te bewegen
    void MoveToTarget()
    {
        // Bereken de richting naar de doelpositie
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Bereken de positie waar naartoe moet worden bewogen
        Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;

        // Beweeg het object naar de nieuwe positie
        transform.position = newPosition;

        // Controleer of het object de doelpositie heeft bereikt
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Reset de doelpositie
            targetPosition = Vector3.zero;
        }
    }

    // Functie om de doelpositie in te stellen
    public void MoveTo(Vector3 target)
    {
        targetPosition = target;
    }
}
