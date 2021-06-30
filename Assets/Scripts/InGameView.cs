using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.IO;
using System.Linq;

public class InGameView : MonoBehaviour
{
    [SerializeField] private Text racingName;
    [SerializeField] private Text[] screenQuestion;
    [SerializeField] private Text[] answers;
    [SerializeField] private Button[] answerButtons;

    private readonly Subject<Unit> nextQuiz = new Subject<Unit>();
    public IObservable<Unit> IOnextQuiz => nextQuiz;

    private string[] arrayChoices = new string[] { };

    [SerializeField] private RectTransform winnerImage;
    [SerializeField] private RectTransform loserImage;

    //TimeBar
    [SerializeField] private Image timebar;
    private float countTime = 10.0f;

    [SerializeField] private State currentState = State.InGame;
    private string currentAnswer = "";


    //結果発表
    [SerializeField] private string[] correctAnswers = new string[]{};
    [SerializeField] private Text[] correctAnsewersText;
    [SerializeField] private Image[] answersPopIcon;
    [SerializeField] private Image correctIcon;
    [SerializeField] private Image mistakeIcon;
    [SerializeField] private RectTransform resultPanel;
    private int answerCount = 0;
    [SerializeField] private Text resultTitle;
    private int correctAnswerCount;

    

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        InGameState(currentState);

    }

    public void InGameState(State state)
    {
        switch (state)
        {
            case State.InGame: 
                timebar.fillAmount -= 1.0f / countTime * Time.deltaTime;
                if(timebar.fillAmount <= 0) { currentState = State.InGameTimeOver; }
                break;
            case State.InGameResult:
                
                break;

            case State.InGameTimeOver:
                InGameResult().Forget();
                break;

            case State.Resulet:
                
                break;

            case State.Resset:
                ResetText().Forget();
                nextQuiz.OnNext(Unit.Default);
                currentState = State.InGame;
                break;

            default:
                break;
        }
    }

    private async UniTaskVoid InGameResult()
    {
        timebar.fillAmount = 1.0f;
        answersPopIcon[answerCount] = mistakeIcon;
        for (var i = 0; i < 4; i++)
        {
            answerButtons[i].enabled = false;
            if (Equals(arrayChoices[i], currentAnswer))
            {
                answers[i].color = Color.green;
            }
            if (!Equals(arrayChoices[i], currentAnswer))
            {
                answers[i].color = Color.red;
            }
        }
        await loserImage.DOScale(1f, 0.3f).SetEase(Ease.OutBounce).AsyncWaitForCompletion();
        //DOVirtual.DelayedCall(0.9f, () => ResetText());
        DOVirtual.DelayedCall(1f, () => currentState = State.Resset);

    }





    public void ViewQuiz(string name, string a, string b, string c, string d, string e, string f,
        string g, string h, string i, string j, string k, string l, string m, string n, string answer,
        string mistake1, string mistake2,string mistake3)
    {
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
            }
        }


        //選択肢(中にランダムの位置に答えをおく
        arrayChoices = new string[]
                        { answer,mistake1,mistake2,mistake3
                        };
        arrayChoices = arrayChoices.OrderBy(x => System.Guid.NewGuid()).ToArray();

        answers[0].text = arrayChoices[0];
        answers[1].text = arrayChoices[1];
        answers[2].text = arrayChoices[2];
        answers[3].text = arrayChoices[3];

        answerButtons[0].onClick.AddListener(() => ButtonChoices(0, arrayChoices[0], answer));
        answerButtons[1].onClick.AddListener(() => ButtonChoices(1, arrayChoices[1], answer));
        answerButtons[2].onClick.AddListener(() => ButtonChoices(2, arrayChoices[2], answer));
        answerButtons[3].onClick.AddListener(() => ButtonChoices(3, arrayChoices[3], answer));
        currentAnswer = answer;
        correctAnswers[answerCount] = answer;
        correctAnsewersText[answerCount].text = correctAnswers[answerCount];

    }


    public void ButtonChoices(int count,string choice,string answer)
    {
        currentState = State.InGameResult;
        if (Equals(choice, answer))
        {
            correctAnswerCount++;
            answers[count].color = Color.green;
            //answerButtons[count].image.color = Color.green;
            answersPopIcon[answerCount] = correctIcon;
            resultTitle.text = "10問中" + correctAnswerCount + "問正解！";
            winnerImage.DOScale(1f, 0.3f).SetEase(Ease.OutBounce);
        }
        else
        {
            Debug.Log("間違い");
            resultTitle.text = "10問中" + correctAnswerCount + "問正解！";
            answersPopIcon[answerCount] = mistakeIcon;
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
        answerButtons[count].enabled = false;
        DOVirtual.DelayedCall(0.6f, () => currentState = State.Resset);
    }

    private async UniTaskVoid ResetText()
    {
        for(var i = 0; i < 4; i++) {
            answers[i].enabled = true;
            answers[i].color = Color.black;
            answerButtons[i].image.color = Color.white;
        }
        await loserImage.DOScale(0f, 0.001f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        await winnerImage.DOScale(0f, 0.001f).SetEase(Ease.Linear).AsyncWaitForCompletion();
       
        timebar.fillAmount = 1.0f;
        answerCount++;
    }


    public void SetResult()
    {
        currentState = State.Resulet;
        resultPanel.DOAnchorPos(Vector2.zero, 0.3f);
    }
    
    public void RestartGame()
    {
        resultPanel.DOAnchorPos(new Vector2(0, -1888), 0.3f);
        answerCount = -1;
        correctAnswerCount = 0;
    }
}
