using UnityEngine;

public class SwipeHandler : MonoBehaviour
{
    public float rotationSpeed = 5f;  // Adjust swipe sensitivity
    private Vector2 lastTouchPos;
    private bool isDragging = false;

    void Update()
    {
#if UNITY_EDITOR || UNITY_STANDALONE
        HandleMouseDrag();
#else
        HandleTouchDrag();
#endif
    }

    void HandleMouseDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastTouchPos = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastTouchPos;
            RotateObject(delta);
            lastTouchPos = Input.mousePosition;
        }
    }

    void HandleTouchDrag()
    {
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPos = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 delta = touch.position - lastTouchPos;
                RotateObject(delta);
                lastTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }

    void RotateObject(Vector2 delta)
    {
        float rotX = delta.y * rotationSpeed * Time.deltaTime;
        float rotY = -delta.x * rotationSpeed * Time.deltaTime;

        transform.Rotate(Vector3.up, rotY, Space.World);   // Horizontal swipe
        transform.Rotate(Vector3.right, rotX, Space.World); // Vertical swipe
    }
}
