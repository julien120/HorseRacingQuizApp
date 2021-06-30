using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UniRx;

public class InGameModel : MonoBehaviour
{
    private QuestionGenerate.StageData[] stageDatas;

    [SerializeField] private QuestionGenerate questionGenerate;
    //[SerializeField] private string filename = "";
    private int stagedataidx = 0;

    //選択肢
    //private readonly Subject<(string,string,string,string)> choices = new Subject<(string, string, string, string)>();
    //public IObservable<(string, string, string, string)> IOchoices => choices;

    private readonly Subject<(string, string, string, string,string,string)> choices = new Subject<(string, string, string, string,string,string)>();
    public IObservable<(string, string, string, string,string,string)> IOchoices => choices;

    //答え
    private readonly Subject<string> answer = new Subject<string>();
    public IObservable<string> IOanswer => answer;

    //お題
    private readonly Subject<string> question = new Subject<string>();
    public IObservable<string> IOquiz => question;

    private static InGameModel instance;
    public static InGameModel Instance { get => instance; }

    private void Awake()
    {
        instance = GetComponent<InGameModel>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Load()
    {
        var stagecsvdata = new List<QuestionGenerate.StageData>();
        var csvdata = Resources.Load<TextAsset>(questionGenerate.filename).text;
        //文字列読み込み1行,区切りずつに
        StringReader sr = new StringReader(csvdata);
        while (sr.Peek() != -1)
        {
            var line = sr.ReadLine();
            var cols = line.Split(',');
            if (cols.Length != 6) continue; //5項目なければwhile脱出

            stagecsvdata.Add(
                new QuestionGenerate.StageData(
                    cols[0],
                    cols[1],
                    cols[2],
                    cols[3],
                    cols[4],
                    cols[5]
                    )
                );

            //Debug.Log(line);
        }
        stageDatas = stagecsvdata.OrderBy(item => System.Guid.NewGuid()).ToArray();
    }

    public void Reset()
    {
        stagedataidx = 0;
    }

    public void Step()
    {
       if (stagedataidx >= stageDatas.Length) { return; }
        //{
            //  stageDatas[stagedataidx].question
            string[] arrayChices = new string[] { stageDatas[stagedataidx].a, stageDatas[stagedataidx].b, stageDatas[stagedataidx].c, stageDatas[stagedataidx].d };
            arrayChices = arrayChices.OrderBy(x => System.Guid.NewGuid()).ToArray(); // 回答候補のリストをシャッフル
            choices.OnNext((stageDatas[stagedataidx].question,arrayChices[0], arrayChices[1], arrayChices[2], arrayChices[3], stageDatas[stagedataidx].answer));
            Debug.Log(stagedataidx);
           
        //}
        ++stagedataidx;
    }

}
