using UnityEngine;

public class DirectionalTargeterBehavior : MonoBehaviour
{
    // Rotation angles for each direction
    private Quaternion upRotation = Quaternion.Euler(0, 0, 0);
    private Quaternion downRotation = Quaternion.Euler(0, 0, 180);
    private Quaternion leftRotation = Quaternion.Euler(0, 0, 90);
    private Quaternion rightRotation = Quaternion.Euler(0, 0, 270);

    public float rotationSpeed = 5f; // Speed of rotation
    private Quaternion targetRotation; // Target rotation

    void Start()
    {
        // Set the initial target rotation to the current rotation
        targetRotation = transform.rotation;
    }

    void Update()
    {
        // Check for input and set the appropriate target rotation
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            targetRotation = upRotation;
        }
        else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            targetRotation = downRotation;
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            targetRotation = leftRotation;
        }
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            targetRotation = rightRotation;
        }

        // Smoothly interpolate towards the target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
