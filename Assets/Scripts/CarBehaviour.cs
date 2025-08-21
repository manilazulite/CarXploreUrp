using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CarBehaviour : MonoBehaviour
{
    public float rotationSpeed = 5f;  // Swipe sensitivity
    private Vector2 lastTouchPos;
    private bool isDragging = false;

    [SerializeField] public Material[] BodyMaterial;
    [SerializeField] public Material[] rimMaterial;
    [SerializeField] public Material[] exhaustMaterial;  


    private void Init()
    {
        //GameManger.Instance.Body.onClick.AddListener(() => GetTheSelectedCarPartMaterial(this.BodyMaterial));
        //GameManger.Instance.Rim.onClick.AddListener(() => GetTheSelectedCarPartMaterial(this.rimMaterial));
        //GameManger.Instance.Exhaust.onClick.AddListener(() => GetTheSelectedCarPartMaterial(this.exhaustMaterial));       
    }

    private void Start()
    {
        Init();
    }

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
            // Ignore clicks on UI
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            if (IsTouchingThisObject(Input.mousePosition))
            {
                lastTouchPos = Input.mousePosition;
                isDragging = true;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastTouchPos;
            RotateHorizontal(delta.x);
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
                // Ignore UI touches
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                    return;

                if (IsTouchingThisObject(touch.position))
                {
                    lastTouchPos = touch.position;
                    isDragging = true;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 delta = touch.position - lastTouchPos;
                RotateHorizontal(delta.x);
                lastTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
            }
        }
    }

    bool IsTouchingThisObject(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == transform)
            {
                return true;
            }
        }
        return false;
    }

    void RotateHorizontal(float deltaX)
    {
        float rotY = -deltaX * rotationSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, rotY, Space.World);
    }

    private void GetTheSelectedCarPartMaterial(Material[] _mat)
    {
        GameManger.Instance.selectedMaterial = _mat;
    }    
}
