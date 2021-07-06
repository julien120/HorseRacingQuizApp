using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    private static SceneController instance;

    public static SceneController Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject single = new GameObject();
                //instanceに格納されてる値を管理する
                instance = single.AddComponent<SceneController>();
                //scene跨いでもインスタンスが残るのでnull処理に行かない
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene(SceneName.TitleScene);
    }
    public void LoadHomeScene()
    {
        SceneManager.LoadScene(SceneName.HomeScene);
    }

    public void LoadInGameScene()
    {
        SceneManager.LoadScene(SceneName.InGame1Scene);
    }

    public void LoadSelectButton(int count)
    {
        //todoCommonValues.PlayerPrefsKeyCode = "INGAME" + count + "_" + QuestionCount.CurrentMaxCount;
//        Debug.Log(CommonValues.PlayerPrefsKeyCode);
        switch (count)
        {
            
            case 1:
                
                LoadInGameScene();
                break;

            case 2:
                LoadInGameScene();
                break;

            case 3:
                LoadInGameScene();
                break;

            case 4:
                LoadInGameScene();
                break;

            case 5:
                LoadInGameScene();
                break;

            default:
                LoadInGameScene();
                break;
        }


    }

}
