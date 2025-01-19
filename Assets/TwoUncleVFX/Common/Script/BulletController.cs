using System.Collections;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float movementSpeed = 10f; // Geschwindigkeit des Projektils
    public float maxDistance = 250f; // Maximale Flugdistanz
    private float curDistance = 0f; // Aktuelle zurückgelegte Distanz
    private bool isActive = false; // Kontrolliert, ob das Projektil aktiv ist

    private Vector3 direction; // Flugrichtung
    private Vector3 startPosition; // Startposition des Projektils

    private void OnEnable()
    {
        curDistance = 0f;
        startPosition = transform.position;
        isActive = true;
    }

    void Update()
    {
        if (!isActive) return;

        // Bewege das Projektil in die Richtung, in die es abgefeuert wurde
        transform.position += direction * movementSpeed * Time.deltaTime;

        // Berechne die zurückgelegte Distanz
        curDistance = Vector3.Distance(startPosition, transform.position);

        // Deaktiviere das Projektil, wenn es die maximale Distanz erreicht
        if (curDistance >= maxDistance)
        {
            ResetBullet();
        }
    }

    /// <summary>
    /// Setzt die Richtung des Projektils basierend auf der Blickrichtung des Gegners.
    /// </summary>
    /// <param name="enemyTransform">Der Transform des Gegners</param>
    public void SetDirection(Transform enemyTransform)
    {
        direction = enemyTransform.forward.normalized; // Setze die Richtung auf die Vorwärtsachse des Gegners
        transform.rotation = Quaternion.LookRotation(direction); // Rotiert das Projektil in die Richtung
    }

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Schaden zufügen oder andere Effekte hier
            Debug.Log("Spieler getroffen!");
        }

        // Deaktiviere das Projektil bei einer Kollision
        ResetBullet();
        //Destroy(collision.gameObject);
    }*/

    private void ResetBullet()
    {
        isActive = false;
        gameObject.SetActive(false); // Deaktiviere das Projektil
    }
}
