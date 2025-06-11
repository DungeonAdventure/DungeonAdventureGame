using UnityEngine;
using GameScripts.Control;
using UnityEngine.SceneManagement;
using Unity.Cinemachine;
using UnityEngine.SceneManagement;


public class UIManager : MonoBehaviour
{
    // 主菜单面板（Start Game / Load Game / Settings / Exit）
    public GameObject panelMainMenu;

    // 角色选择面板（选择 Warrior / Thief / Priestess）
    public GameObject panelCharacterSelect;

    // 主菜单背景
    public GameObject backgroundMain;

    // 角色选择背景
    public GameObject backgroundCharacter;

    // 黑色半透明遮罩层（用来突出弹窗）
    public GameObject overlayDim;

    // 加载游戏面板（包含三个存档按钮）
    public GameObject panelLoadGame;

    // 英雄命名面板（输入角色名字的界面）
    public GameObject panelHeroName;
    
    public GameObject panelGameInformation;
    
    public GameObject panelTopTabSettings;
    void Start()
    {
        // 初始显示主菜单，隐藏其他所有面板
        panelMainMenu.SetActive(true);
        panelCharacterSelect.SetActive(false);
        backgroundMain.SetActive(true);
        backgroundCharacter.SetActive(false);
        overlayDim.SetActive(false);
        panelHeroName.SetActive(false);
        panelLoadGame.SetActive(false);
        
        
        
        panelGameInformation.SetActive(false);
        panelTopTabSettings.SetActive(false);
    }

    // 玩家点击主菜单中的“Start Game”
    public void OnStartGameClicked()
    {
        // 隐藏主菜单和角色选择等所有 UI（实际应跳转到游戏场景）
        panelMainMenu.SetActive(false);
        panelCharacterSelect.SetActive(false);
        backgroundMain.SetActive(false);
        backgroundCharacter.SetActive(false);
        overlayDim.SetActive(false);
        panelHeroName.SetActive(false);
        panelLoadGame.SetActive(false);
        
        
        panelGameInformation.SetActive(false);
        panelTopTabSettings.SetActive(false);
        
        if (SceneManager.sceneCount == 1) {
            SceneManager.LoadScene("5thScenes", LoadSceneMode.Additive);
        }
    }

    // 显示英雄命名面板
    public void ShowHeroName(string heroName)
    {
        panelCharacterSelect.SetActive(false); // 隐藏角色选择界面
        panelHeroName.SetActive(true);         // 显示命名输入面板
        overlayDim.SetActive(true);            // 开启遮罩防止误点其他 UI
        
    }
    
    // 显示 游戏信息面板
    public void ShowGTopTabSettings()
    {   
        Debug.Log("panelTopTabSettings called ");
        panelMainMenu.SetActive(false);            // 隐藏主菜单
        panelTopTabSettings.SetActive(true);    // 显示游戏介绍面板
        overlayDim.SetActive(true);                // 显示黑色遮罩
    }
    
    // 显示 游戏信息面板
    public void ShowGameInformation()
    {   
        Debug.Log("Game Information used");
        panelMainMenu.SetActive(false);            // 隐藏主菜单
        panelGameInformation.SetActive(true);    // 显示游戏介绍面板
        panelTopTabSettings.SetActive(false); 
        overlayDim.SetActive(true);                // 显示黑色遮罩
        
        Debug.Log("panelGameInformation activeSelf: " + panelGameInformation.activeSelf);
    }

    // 玩家点击主菜单中的“Load Game”按钮
    public void OnLoadGameClicked()
    {   
        Debug.Log("✅ Load Game button clicked!");
        panelMainMenu.SetActive(false);        // 隐藏主菜单
        panelLoadGame.SetActive(true);         // 显示加载面板
        overlayDim.SetActive(true);            // 显示遮罩层
    }

    // 玩家从 Load Game 面板点击“返回”按钮
    public void OnBackFromLoadGame()
    {
        panelLoadGame.SetActive(false);        // 隐藏加载游戏面板
        panelMainMenu.SetActive(true);         // 返回主菜单
        overlayDim.SetActive(false);           // 隐藏遮罩
    }

    // 玩家在命名面板点击“取消”按钮时调用
    public void CancelHeroNaming()
    {
        panelHeroName.SetActive(false);        // 隐藏命名面板
        panelCharacterSelect.SetActive(true);  // 返回角色选择界面
        overlayDim.SetActive(false);           // 隐藏遮罩
    }

    // 返回主菜单（通常是从角色选择界面触发）
    public void OnBackToMenu()
    {
        panelCharacterSelect.SetActive(false); // 隐藏角色选择
        panelMainMenu.SetActive(true);         // 显示主菜单
        backgroundMain.SetActive(true);        // 显示主菜单背景
        backgroundCharacter.SetActive(false);  // 隐藏角色选择背景
        overlayDim.SetActive(false);           // 隐藏遮罩
        panelHeroName.SetActive(false);        // 隐藏命名面板（确保清除）
        panelLoadGame.SetActive(false);
        
        //new panel
        panelGameInformation.SetActive(false);
        panelTopTabSettings.SetActive(false);
    }
    
    public void OnExitGameClicked()
    {
        Debug.Log("❌ Exit button clicked. Quitting game...");

        #if UNITY_EDITOR
                // 在 Unity 编辑器中运行时停止播放
                UnityEditor.EditorApplication.isPlaying = false;
        #else
            // 打包后退出游戏
            Application.Quit();
        #endif
    }

}
