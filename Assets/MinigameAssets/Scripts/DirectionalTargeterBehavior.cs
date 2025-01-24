using UnityEngine;

public class DirectionalTargeterBehavior : MonoBehaviour
{
    // Rotation angles for each direction
    private Quaternion upRotation = Quaternion.Euler(0, 0, 0);
    private Quaternion downRotation = Quaternion.Euler(0, 0, 180);
    private Quaternion leftRotation = Quaternion.Euler(0, 0, 90);
    private Quaternion rightRotation = Quaternion.Euler(0, 0, 270);

    void Update()
    {
        // Check for input and apply the appropriate rotation
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.rotation = upRotation;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.rotation = downRotation;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.rotation = leftRotation;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.rotation = rightRotation;
        }
    }
}
