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
        inGameModel.IOchoices.Subscribe(item => inGameView.ViewQuiz(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5, item.Item6,item.Item7,
                                                                    item.Item8, item.Item9, item.Item10, item.Item11, item.Item12, item.Item13, item.Item14,
                                                                    item.Item15, item.Item16, item.Item17, item.Item18, item.Item19));
        inGameModel.IOsetResult.Subscribe(_ => inGameView.SetResult());
        inGameView.IOnextQuiz.Subscribe(_ => inGameModel.Step());
        inGameView.IOsetrestart.Subscribe(_ => RestartModel());
        inGameModel.Load();
        inGameModel.Reset();
        inGameModel.Step();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RestartModel()
    {
        inGameModel.Load();
        inGameModel.Reset();
    }
}
