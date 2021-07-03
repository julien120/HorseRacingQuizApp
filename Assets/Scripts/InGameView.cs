using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using UniRx.Triggers;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.IO;
using System.Linq;

public class InGameView : MonoBehaviour
{
    [SerializeField] private Text quizTitle;
    [SerializeField] private Text racingName;
    [SerializeField] private Text[] screenQuestion;
    [SerializeField] private Text[] answers;
    [SerializeField] private Button[] answerButtons;

    private readonly Subject<Unit> nextQuiz = new Subject<Unit>();
    public IObservable<Unit> IOnextQuiz => nextQuiz;

    private readonly Subject<Unit> setrestart = new Subject<Unit>();
    public IObservable<Unit> IOsetrestart => setrestart;

    private string[] arrayChoices = new string[] { };

    [SerializeField] private RectTransform winnerImage;
    [SerializeField] private RectTransform loserImage;

    //TimeBar
    [SerializeField] private Image timebar;
    private float countTime = 10.0f;

    [SerializeField] private State currentState = State.InGame;
    private string currentAnswer = "";

    [SerializeField] private RectTransform popPanel;


    //結果発表
    [SerializeField] private string[] correctAnswers = new string[]{};
    [SerializeField] private Text[] correctAnsewersText;
    [SerializeField] private Image[] answersPopIcon;
    [SerializeField] private Sprite correctIcon;
    [SerializeField] private Sprite mistakeIcon;
    [SerializeField] private RectTransform resultPanel;
    private int answerCount = 0;
    [SerializeField] private Text resultTitle;
    [SerializeField] private int correctAnswerCount =0;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button ToHomeButton;
    private int answerIcon = 0;
    private bool iconFlag = true;
    private bool isFlag = true;

    //消した箇所のテキスト
    private int bloodlineCount;

    //BGM・SE関連
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip correctAnswerAudio;
    [SerializeField] private AudioClip wrongAnswerAudio;
    [SerializeField] private AudioClip QuestionAudio;




    void Start()
    {
        StartSetting();


        this.UpdateAsObservable()
        .Subscribe(_ => {
            InGameState(currentState);

        });
    }

    private void StartSetting()
    {
        quizTitle.text = CommonValues.InGameTitleText;
        correctAnswerCount -= 1;
        answerIcon -= 1;
        restartButton.onClick.AddListener(RestartGame);
        ToHomeButton.onClick.AddListener(ToHomeFromInGame);
        loserImage.DOScale(0f, 0.1f).SetEase(Ease.Linear);
        winnerImage.DOScale(0f, 0.1f).SetEase(Ease.Linear);
        ResetText().Forget();
        popPanel.DOAnchorPos(Vector2.zero, 0.4f);
    }


    public void InGameState(State state)
    {
        switch (state)
        {
            case State.InGame:
                //ButtonEnabled();
               
                hoge = true;
                isFlag_i = true;
                timebar.fillAmount -= 1.0f / countTime * Time.deltaTime;
                if(timebar.fillAmount <= 0)
                {
                    //todo
                    currentState = State.InGameTimeOver;
                }
                break;

            case State.InGameResult:
                
                break;

            case State.InGameTimeOver:
                iconFlag = true;
                isFlags = true;
                InGameResult();

                //currentState = State.Resset;
                break;

            case State.Resulet:
                
                break;

            case State.Resset:
                ResetText().Forget();//todo:時間切れでこの処理を呼び出したときは1フレームに何度も呼ばれるかdowteen途中できれる
                isFlag = true;
                
                if (isFlag_i)
                {
                    isFlag_i = false;
                    if (hoge) {
                    nextQuiz.OnNext(Unit.Default);
                        hoge = false;
                    }

                }
                //Observable.IntervalFrame(230)
                //.Do(_ => nextQuiz.OnNext(Unit.Default))
                //.First()
                //.Subscribe();
               // Debug.Log("呼び出されたよ");

                currentState = State.InGame;
                break;

            default:
                break;
        }
    }
    private bool hoge = true;
    private bool isFlag_i = true;
    private bool isFlags =true;
    private void InGameResult()
    {

        for (var i = 0; i < 4; i++)
        {
            answerButtons[i].enabled = false;
            if (Equals(arrayChoices[i], currentAnswer))
            {
                audioSource.PlayOneShot(correctAnswerAudio);
                answers[i].color = Color.green;
            }
            if (!Equals(arrayChoices[i], currentAnswer))
            {
                audioSource.PlayOneShot(wrongAnswerAudio);
                answers[i].color = Color.red;
            }
        }
        loserImage.DOScale(1f, 0.3f).SetEase(Ease.OutBounce)
            .OnComplete(() =>
            {
                //isFlag = true;
                // ResetText().Forget();
                //nextQuiz.OnNext(Unit.Default);
                if (isFlags) {
                    currentState = State.Resset;
                    isFlags = false;
                }
            });

    }

    private void ButtonEnabled()
    {

        for(var i = 0; i < 4; i++) { 
        answers[i].enabled = true;
        }
    }





    public void ViewQuiz(string name, string a, string b, string c, string d, string e, string f,
        string g, string h, string i, string j, string k, string l, string m, string n, string answer,
        string mistake1, string mistake2,string mistake3)
    {
        audioSource.PlayOneShot(QuestionAudio);
        ResetText().Forget();
        //血統表の描画
        racingName.text = name;
        screenQuestion[0].text = a;
        screenQuestion[1].text = b;
        screenQuestion[2].text = c;
        screenQuestion[3].text = d;
        screenQuestion[4].text = e;
        screenQuestion[5].text = f;
        screenQuestion[6].text = g;
        screenQuestion[7].text = h;
        screenQuestion[8].text = i;
        screenQuestion[9].text = j;
        screenQuestion[10].text = k;
        screenQuestion[11].text = l;
        screenQuestion[12].text = m;
        screenQuestion[13].text = n;

        for(var z=0; z < 14; z++)
        {
            if(screenQuestion[z].text == answer)
            {
                screenQuestion[z].text = "";
                bloodlineCount = z;
            }
        }

        currentAnswer = answer;
        //選択肢(中にランダムの位置に答えをおく
        arrayChoices = new string[]
                        { answer,mistake1,mistake2,mistake3
                        };
        arrayChoices = arrayChoices.OrderBy(x => System.Guid.NewGuid()).ToArray();

        answers[0].text = arrayChoices[0];
        answers[1].text = arrayChoices[1];
        answers[2].text = arrayChoices[2];
        answers[3].text = arrayChoices[3];

        answerButtons[0].onClick.AddListener(() => ButtonChoices(0, arrayChoices[0], currentAnswer));
        answerButtons[1].onClick.AddListener(() => ButtonChoices(1, arrayChoices[1], currentAnswer));
        answerButtons[2].onClick.AddListener(() => ButtonChoices(2, arrayChoices[2], currentAnswer));
        answerButtons[3].onClick.AddListener(() => ButtonChoices(3, arrayChoices[3], currentAnswer));

        correctAnswers[answerCount] = answer;
        correctAnsewersText[answerCount].text = correctAnswers[answerCount];
        answerCount++;

    }

    public void ButtonChoices(int count,string choice,string answer)
    {
        audioSource.PlayOneShot(correctAnswerAudio);
        iconFlag = true;
        isFlags = true;
        answerButtons[count].enabled = false;
        currentState = State.InGameResult;
        screenQuestion[bloodlineCount].text = answer;
        screenQuestion[bloodlineCount].color = Color.green;
        if (iconFlag)
        {
            answerIcon += 1;
            iconFlag = false;
        }
        if (Equals(choice, answer))
        {
            if (isFlag)
            {
                correctAnswerCount += 1;
                
                if (correctAnswerCount > PlayerPrefs.GetInt(CommonValues.PlayerPrefsKeyCode))
                {
                     PlayerPrefs.SetInt(CommonValues.PlayerPrefsKeyCode, correctAnswerCount);
                }
                isFlag = false;
            }
            answers[count].color = Color.green;
            //answerButtons[count].image.color = Color.green;
//            answersPopIcon[answerIcon].sprite = correctIcon;
            resultTitle.text = QuestionCount.CurrentMaxCount+"問中" + correctAnswerCount + "問正解！";
            winnerImage.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);
            //Debug.Log(choice + "と"+answer);
        }
        else if (!Equals(choice, answer))
        {
            audioSource.PlayOneShot(wrongAnswerAudio);
            resultTitle.text = QuestionCount.CurrentMaxCount+"問中" + correctAnswerCount + "問正解！";
//            answersPopIcon[answerIcon].sprite = mistakeIcon;
            answers[count].color = Color.red;
            for(var i = 0; i < 3; i++)
            {
                if (Equals(arrayChoices[i], answer))
                {
                    answers[i].color = Color.green;
                }
            }
            loserImage.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);
        }
        //answerButtons[count].enabled = false;

        DOVirtual.DelayedCall(0.6f, () => currentState = State.Resset);
    }



    //ダサいし、リファクタリングの観点からこういうbool変数の使い方なくしていきたいが、一旦プロトタイプがちゃんと動くように仮置きする
    private bool _aflag = true;
    private async UniTaskVoid ResetText()
    {
            screenQuestion[bloodlineCount].text = "";
            screenQuestion[bloodlineCount].color = Color.black;
            for (var i = 0; i < 4; i++) {
                answers[i].color = Color.black;
                answerButtons[i].image.color = Color.white;
                answerButtons[i].enabled = true;
            }
            timebar.fillAmount = 1.0f;


        loserImage.DOScale(0f, 0.01f).SetEase(Ease.Linear);
        loserImage.DOComplete();
        await winnerImage.DOScale(0f, 0.001f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        winnerImage.DOComplete();
       
        
       //todo answerCount++;
    }


    public void SetResult()
    {
        currentState = State.Resulet;
        resultPanel.DOAnchorPos(Vector2.zero, 0.3f);

    }

    /// <summary>
    /// todo:動画広告を流してからシーン変遷
    /// </summary>
    private void RestartGame()
    {
        answerIcon = 0;
        isFlag = true;
        ResetText().Forget();
        popPanel.DOAnchorPos(Vector2.zero, 0.3f);
        resultPanel.DOAnchorPos(new Vector2(0, -1888), 0.3f);
        answerCount = 0;
        correctAnswerCount = 0;
        DOVirtual.DelayedCall(0.3f, () =>
        {
            SceneController.Instance.LoadInGameScene();
        });
        
    }

    private void ToHomeFromInGame()
    {
        SceneController.Instance.LoadHomeScene();
    }


  



}
