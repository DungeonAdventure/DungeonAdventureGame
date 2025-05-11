using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class TabController : MonoBehaviour
{
    
    public Image[] tabImages;
    public GameObject[] pages;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ActiveTab(0);
        
    }

    public void ActiveTab(int tabIndex)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            pages[i].SetActive(false);
            tabImages[i].color = Color.grey;
        }
        
        pages[tabIndex].SetActive(true);
        tabImages[tabIndex].color = Color.white;
        
    }
    
}
