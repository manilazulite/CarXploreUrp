using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{
    public static GameManger Instance;

    [SerializeField] private Button moveLeft, moveRight;
        
    public Button Body, Rim, Exhaust, RedColor, YellowColor, BlackColor, WhiteColor, BlueColor;

    [SerializeField] private GameObject[] cars;

    [SerializeField] private Vector3 intialPosition = Vector3.zero;

    [SerializeField] private Transform hidePos, leftTargetPos, rightTargetPos;

    [SerializeField] private int currentIndex = 0;

    public bool canSwipe = false;

    public Material[] selectedMaterial;

    private enum carPart
    {
        None,
        body,
        rim,
        exhaust
    };

    private carPart part;

    private void Awake()
    {
        Init();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {        

        for (int i = 0; i < cars.Length; i++)
        {
            cars[i].GetComponent<CarBehaviour>().enabled = false;
            cars[i].transform.position = hidePos.position;
        }

        cars[0].GetComponent<CarBehaviour>().enabled = true;
        cars[0].transform.position = intialPosition;

        canSwipe = true;

    }


    private void Init()
    {
        Instance = this;

        part = carPart.None;

        moveLeft.onClick.AddListener(MoveLeft);
        moveRight.onClick.AddListener(MoveRight);

        Body.onClick.AddListener(() => SetSelectedCarPartState(carPart.body));
        Rim.onClick.AddListener(() => SetSelectedCarPartState(carPart.rim));
        Exhaust.onClick.AddListener(() => SetSelectedCarPartState(carPart.exhaust));

        RedColor.onClick.AddListener(() => ChangeColor(RedColor.GetComponent<Image>().color));
        YellowColor.onClick.AddListener(() => ChangeColor(YellowColor.GetComponent<Image>().color));
        BlackColor.onClick.AddListener(() => ChangeColor(BlackColor.GetComponent<Image>().color));
        WhiteColor.onClick.AddListener(() => ChangeColor(WhiteColor.GetComponent<Image>().color));
        BlueColor.onClick.AddListener(() => ChangeColor(BlueColor.GetComponent<Image>().color));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void MoveRight()
    {
        //Debug.Log("Move right");

        canSwipe = false;

        if (currentIndex > 0)
        {
            cars[currentIndex].transform.DOMove(rightTargetPos.position, 1f);
            cars[currentIndex].GetComponent<CarBehaviour>().enabled = false;

            cars[currentIndex - 1].transform.position = leftTargetPos.position;

            cars[currentIndex - 1].transform.DOMove(intialPosition, 1f).OnComplete(OnTweenEnd);

            //cars[currentIndex - 1].GetComponent<CarBehaviour>().enabled = true;

            currentIndex--;
        }

        
    }

    private void MoveLeft()
    {
        //Debug.Log("Move left");

        canSwipe = false;

        if (currentIndex < cars.Length - 1)
        {
            cars[currentIndex].transform.DOMove(leftTargetPos.position, 1f);
            cars[currentIndex].GetComponent<CarBehaviour>().enabled = false;

            cars[currentIndex + 1].transform.position = rightTargetPos.position;

            cars[currentIndex + 1].transform.DOMove(intialPosition, 1f).OnComplete(OnTweenEnd);
            //cars[currentIndex + 1].GetComponent<CarBehaviour>().enabled = true;

            currentIndex++;
        }

        //if (currentIndex < cars.Length - 1)
            

        
    }

    private void OnTweenEnd() //0 - left // 1 - right
    {
        cars[currentIndex].GetComponent<CarBehaviour>().enabled = true;
        //Hide the car
        HideCar();
    }

    private void HideCar()
    {
        canSwipe = true;
        //cars[currentIndex].transform.position = hidePos.position;
    }

    private void SetSelectedCarPartState(carPart _part)
    {
        part = _part;
        GetTheSelectedCarPartMaterial();
    }

    private void GetTheSelectedCarPartMaterial()
    { 
        switch(part)
        {
            case carPart.body:
                selectedMaterial = cars[currentIndex].GetComponent<CarBehaviour>().BodyMaterial;
                break;
            case carPart.rim:
                selectedMaterial = cars[currentIndex].GetComponent<CarBehaviour>().rimMaterial;
                break;
            case carPart.exhaust:
                selectedMaterial = cars[currentIndex].GetComponent<CarBehaviour>().exhaustMaterial;
                break;
            default:
                break;
        }
        
    }

    public void ChangeColor(Color color)
    {
        for (int i = 0; i < selectedMaterial.Length; i++)
        {
            selectedMaterial[i].color = color;
        }
    }
}
