using UnityEngine;

public class MenuControler : MonoBehaviour
{   
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject menuCanvas;
    public static MenuControler Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);

        }
    }

    void Start()
    {
        menuCanvas.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        { 
            menuCanvas.SetActive(!menuCanvas.activeSelf);
        }
    }
}
