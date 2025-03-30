using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float offsetX = 0f;
    public float offsetz = -10f;
    public float centerThresholdY = 5f;

    void LateUpdate()
    {
        if (player.position.y > centerThresholdY)
        {
            transform.position = new Vector3(offsetX, player.position.y, offsetz);
        }
        else
        {
            transform.position = new Vector3(offsetX, centerThresholdY, offsetz);
        }
    }
}
