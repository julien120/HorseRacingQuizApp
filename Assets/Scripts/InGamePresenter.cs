using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InGamePresenter : MonoBehaviour
{
    [SerializeField] private InGameModel inGameModel;
    [SerializeField] private InGameView inGameView;

    void Start()
    {
        //InGameView.ViewQuiz
        inGameModel.IOchoices.Subscribe(item => inGameView.ViewQuiz(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5, item.Item6));
        inGameView.IOnextQuiz.Subscribe(_ => inGameModel.Step());
        inGameModel.Load();
        inGameModel.Reset();
        inGameModel.Step();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
