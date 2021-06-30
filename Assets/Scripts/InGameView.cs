using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UniRx;
using DG.Tweening;

public class InGameView : MonoBehaviour
{
    [SerializeField] private Text screenQuestion;
    [SerializeField] private Text[] answers;
    [SerializeField] private Button[] answerButtons;

    private readonly Subject<Unit> nextQuiz = new Subject<Unit>();
    public IObservable<Unit> IOnextQuiz => nextQuiz;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ViewQuiz(string question,string a, string b, string c, string d,string answer)
    {
        ResetText();
        screenQuestion.text = question;

        answers[0].text = a;
        answers[1].text = b;
        answers[2].text = c;
        answers[3].text = d;

        answerButtons[0].onClick.AddListener(() => ButtonChoices(0,a,answer));
        answerButtons[1].onClick.AddListener(() => ButtonChoices(1, b, answer));
        answerButtons[2].onClick.AddListener(() => ButtonChoices(2, c, answer));
        answerButtons[3].onClick.AddListener(() => ButtonChoices(3, d, answer));


        //答えは押したら色が緑になる
        //答え以外を押したら色が赤になる

    }

    public void ButtonChoices(int count,string choice,string answer)
    {
        if (Equals(choice, answer))
        {
            answers[count].color = Color.green;
        }
        else
        {
            answers[count].color = Color.red;
        }
        answerButtons[count].enabled = false;
        DOVirtual.DelayedCall(0.5f,()=> nextQuiz.OnNext(Unit.Default));
    }

    private void ResetText()
    {
        for(var i = 0; i < 4; i++) {
            answers[i].enabled = true;
            answers[i].color = Color.black;
        }
    }


}
