using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Linq;
using UniRx;
using DG.Tweening;

public class HomeView : MonoBehaviour
{

    [SerializeField] private Button[] loadInGameButton;
    [SerializeField] private Transform scrollCanvas;
    [SerializeField] private GameObject questionElemntPrefab;
    [SerializeField] private QuestionUIView questionUIView;

    //InGameStartButton
    [SerializeField] private Button startButton;

    private List<string> list = new List<string>()
    { QuestionTitle.InGame1,QuestionTitle.InGame2,QuestionTitle.InGame3,
      QuestionTitle.InGame4,QuestionTitle.InGame5
    };

    //PlayerPrefsKeyとタイトル、ボタン押したときに下にポップされる内容を変更する引数を一つのデータ型
    //リファクタリングの観点から優れたやり方がこっちだと思う
    //todoスコアデータ、タイトル、競走馬リストの３つのデータが必要
    Dictionary<string, string> dictionary = new Dictionary<string, string>() {
    {PlayerPrefsKeyName.InGame1score10, QuestionTitle.InGame1},
    {PlayerPrefsKeyName.InGame2score, QuestionTitle.InGame2},
    {PlayerPrefsKeyName.InGame3score, QuestionTitle.InGame3}
    };

    //model周り
    [SerializeField] private Text racingHorseList;
    private QuestionGenerate.StageData[] stageDatas;
    [SerializeField] private QuestionGenerate questionGenerate;
    private List<string> racingLists = new List<string>();
    private string listText;

    //
    [SerializeField] private Button[] questionCountButton;
    [SerializeField] private Sprite hoverSprite;
    [SerializeField] private Sprite defaultSprite;


    void Start()
    {
        startButton.onClick.AddListener(() => DOVirtual.DelayedCall(0.2f, () => SceneController.Instance.LoadSelectButton(1)));
        SetQuestionTile();
        AttachButton();
        Load("quiz1");
        Step();
    }

    private void AttachButton()
    {
        questionCountButton[0].onClick.AddListener(SetQuestionCount10Button);
        questionCountButton[1].onClick.AddListener(SetQuestionCount15Button);
        questionCountButton[2].onClick.AddListener(SetQuestionCount20Button);
        questionCountButton[3].onClick.AddListener(SetQuestionCount50Button);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //要素から重複内容を削除
    //= list.Distinct ();
    int number=0;
    [SerializeField]private QuestionUIView[] questionElemnt;

    private void SetQuestionTile()
    {
        
        list.ForEach(x => {
            questionElemnt[number] = Instantiate(questionElemntPrefab, scrollCanvas, false).GetComponent<QuestionUIView>();

             var hoge= number + 1;
            questionElemnt[number].RankkingText.text = hoge.ToString();
            //todo prefsKeyの参照をそれぞれにする,スマートではないが"INGAME"+hoge+"_10"
            questionElemnt[number].ScoreText.text = PlayerPrefs.GetInt("INGAME" + hoge + "_10").ToString();
            questionElemnt[number].NameText.text = x;

            //todo ファイル読み込み名をそれぞれにする,スマートではないが"quiz"+hoge
            questionElemnt[number].GetComponent<Button>().onClick.AddListener( ()=> SetSelectView(number,"quiz" + number));
            number++;
        });
    }

    /// <summary>
    /// ユーザーが設問をクリックしたら,
    /// 主題競争馬の描画がお題に関連した内容に変わるようにする
    /// </summary>
    private void SetSelectView(int count,string filename)
    {
        //スタートボタンにアタッチされる内容を変更する
        startButton.onClick.AddListener(() => DOVirtual.DelayedCall(0.2f, () => SceneController.Instance.LoadSelectButton(count)));
        
        //競走馬リストの読み込み
        Reset();
        Load(filename);
        Step();
    }


    //
    public void Reset()
    {
        listText = "";
        for (var i = 0; i < stageDatas.Length; i++)
        {
            racingLists.Remove(stageDatas[i].HorseName);

        }

    }

    public void Load(string filename)
    {
        var stagecsvdata = new List<QuestionGenerate.StageData>();
        var csvdata = Resources.Load<TextAsset>(filename).text;
        //文字列読み込み1行,区切りずつに
        StringReader sr = new StringReader(csvdata);
        while (sr.Peek() != -1)
        {
            var line = sr.ReadLine();
            var cols = line.Split(',');
            if (cols.Length != 15) continue; //6項目なければwhile脱出

            stagecsvdata.Add(
                new QuestionGenerate.StageData(
                    cols[0],
                    cols[1],
                    cols[2],
                    cols[3],
                    cols[4],
                    cols[5],
                    cols[6],
                    cols[7],
                    cols[8],
                    cols[9],
                    cols[10],
                    cols[11],
                    cols[12],
                    cols[13],
                    cols[14]
                    )
                );

            //Debug.Log(line);
        }
        stageDatas = stagecsvdata.OrderBy(item => System.Guid.NewGuid()).ToArray();
    }


    public void Step()
    {

        for (var i = 0; i < stageDatas.Length; i++) {
            racingLists.Add(stageDatas[i].HorseName);

        }
        foreach (var value in racingLists)
        {
            listText += string.Format(value + "、") ;
        }

        //末尾だけカンマ消えるようにする
        racingHorseList.text = listText;
    }

    private void SetQuestionCount10Button()
    {
        questionCountButton[0].image.sprite = hoverSprite;
        questionCountButton[1].image.sprite = defaultSprite;
        questionCountButton[2].image.sprite = defaultSprite;
        questionCountButton[3].image.sprite = defaultSprite;

        questionCountButton[0].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.white;
        questionCountButton[1].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;
        questionCountButton[2].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;
        questionCountButton[3].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;
        QuestionCount.CurrentMaxCount = QuestionCount.Count10;

        for(var i=0;i< questionElemnt.Length; i++)
        {
            //scoreとMax値の変更
            questionElemnt[i].ScoreText.text = PlayerPrefs.GetInt("INGAME"+(i+1)+"_10").ToString();
            questionElemnt[i].MaxCountText.text = QuestionCount.Count10.ToString();
        }
    }
    private void SetQuestionCount15Button()
    {
        questionCountButton[1].image.sprite = hoverSprite;
        questionCountButton[0].image.sprite = defaultSprite;
        questionCountButton[2].image.sprite = defaultSprite;
        questionCountButton[3].image.sprite = defaultSprite;
        questionCountButton[1].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.white;
        questionCountButton[0].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;
        questionCountButton[2].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;
        questionCountButton[3].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;
        QuestionCount.CurrentMaxCount = QuestionCount.Count15;

        for (var i = 0; i < questionElemnt.Length; i++)
        {
            questionElemnt[i].ScoreText.text = PlayerPrefs.GetInt("INGAME" + (i + 1) + "_15").ToString();
            questionElemnt[i].MaxCountText.text = QuestionCount.Count15.ToString();
        }
    }

    private void SetQuestionCount20Button()
    {
        questionCountButton[2].image.sprite = hoverSprite;
        questionCountButton[0].image.sprite = defaultSprite;
        questionCountButton[1].image.sprite = defaultSprite;
        questionCountButton[3].image.sprite = defaultSprite;

        questionCountButton[2].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.white;
        questionCountButton[0].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;
        questionCountButton[1].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;
        questionCountButton[3].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;

        QuestionCount.CurrentMaxCount = QuestionCount.Count20;
        for (var i = 0; i < questionElemnt.Length; i++)
        {
            questionElemnt[i].ScoreText.text = PlayerPrefs.GetInt("INGAME" + (i + 1) + "_20").ToString();
            questionElemnt[i].MaxCountText.text = QuestionCount.Count20.ToString();
        }
    }
    private void SetQuestionCount50Button()
    {
        questionCountButton[3].image.sprite = hoverSprite;
        questionCountButton[0].image.sprite = defaultSprite;
        questionCountButton[1].image.sprite = defaultSprite;
        questionCountButton[2].image.sprite = defaultSprite;

        questionCountButton[3].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.white;
        questionCountButton[0].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;
        questionCountButton[1].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;
        questionCountButton[2].transform.GetChild(0).gameObject.GetComponent<Text>().color = Color.black;

        QuestionCount.CurrentMaxCount = QuestionCount.Count50;
        for (var i = 0; i < questionElemnt.Length; i++)
        {
            questionElemnt[i].ScoreText.text = PlayerPrefs.GetInt("INGAME" + (i + 1) + "_50").ToString();
            questionElemnt[i].MaxCountText.text = QuestionCount.Count50.ToString();
        }
    }

}
