using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceMushroom : MonoBehaviour
{
    [Header("Direction Control")]
    [SerializeField]
    private bool directionCustomizable;

    [Header("Push Values")]
    [SerializeField]
    private float pushDistance = 0.5f;

    [SerializeField]
    private float pushDuration = 0.1f;

    [Header("For Customized Direction")]
    [SerializeField]
    private float unitCircleAngleDegrees = 0;

    private void OnCollisionEnter(Collision collision)
    {
        print("Hi");
        print(collision.gameObject.name);

        if (collision.transform.tag == "Player")
        {
            print(collision.gameObject.name);

            Transform player = collision.transform;
            Vector2 direction;

            if (directionCustomizable)
                direction = new Vector2(Mathf.Cos(unitCircleAngleDegrees * Mathf.Deg2Rad), Mathf.Sin(unitCircleAngleDegrees * Mathf.Deg2Rad));
            else
                direction = player.position - collision.GetContact(0).point;

            Vector2 pushPosition = new Vector2(direction.x, direction.y);
            pushPosition = pushPosition.normalized * pushDistance + new Vector2(player.localPosition.x, player.localPosition.y);

            StartCoroutine(PushGameObject(player, pushPosition));
        }
    }

    private IEnumerator PushGameObject(Transform player, Vector2 finalPosition)
    {
        float elapsedTime = 0;
        Vector2 initialPosition = new Vector2(player.localPosition.x, player.localPosition.y);
        Vector2 currentPosition;

        float originalMovementSpeed = player.GetComponent<ShipMovement>().xySpeed;
        player.GetComponent<ShipMovement>().xySpeed = 0;

        while (elapsedTime < pushDuration)
        {
            currentPosition = Vector2.Lerp(initialPosition, finalPosition, elapsedTime / pushDuration);
            player.transform.localPosition = new Vector3(currentPosition.x, currentPosition.y, player.localPosition.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        player.transform.localPosition = finalPosition;
        player.GetComponent<ShipMovement>().xySpeed = originalMovementSpeed;
    }


}
