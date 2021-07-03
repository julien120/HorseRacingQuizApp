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

    //[SerializeField] private string filename = "";
    private int stagedataidx = 0;
    private const int MaxCount = 10;
    //選択肢
    //private readonly Subject<(string,string,string,string)> choices = new Subject<(string, string, string, string)>();
    //public IObservable<(string, string, string, string)> IOchoices => choices;

    //血統全部とランダムに描画した答え
    private readonly Subject<(string, string, string, string,string,string, string, string, string, string, string, string, string, string, string,string, string, string, string)> choices = new Subject<(string, string, string, string, string,string, string, string, string, string, string, string, string, string, string, string, string, string, string)>();
    public IObservable<(string, string, string, string, string, string, string, string, string, string, string, string, string, string, string,string, string, string, string)> IOchoices => choices;

    //答え
    private readonly Subject<string> answer = new Subject<string>();
    public IObservable<string> IOanswer => answer;

    //お題
    private readonly Subject<string> question = new Subject<string>();
    public IObservable<string> IOquiz => question;

    //resultViewの呼び出し
    private readonly Subject<Unit> setResult = new Subject<Unit>();
    public IObservable<Unit> IOsetResult => setResult;

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
        var csvdata = Resources.Load<TextAsset>(CommonValues.FileName).text;
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

    public void Reset()
    {
        stagedataidx = 0;
    }

    public void Step()
    {
        //todo:MaxCount10にする
        //後にユーザーが出題数をいじれるようにする
       if (stagedataidx >= stageDatas.Length|| stagedataidx >= QuestionCount.CurrentMaxCount) {
            setResult.OnNext(Unit.Default);
            //result画面の描画
            return;
        }
                        
        //答え
        string[] arrayChoices = new string[]
                        { stageDatas[stagedataidx].sire, stageDatas[stagedataidx].secondsire,
                        stageDatas[stagedataidx].siredam, stageDatas[stagedataidx].thirdsire, stageDatas[stagedataidx].secondsiredam,
                        stageDatas[stagedataidx].siredamsire, stageDatas[stagedataidx].siredamdam, stageDatas[stagedataidx].dam,
                        stageDatas[stagedataidx].broodMareSire, stageDatas[stagedataidx].seconddam, stageDatas[stagedataidx].damsiresire,
                        stageDatas[stagedataidx].damsiredam, stageDatas[stagedataidx].seconddamsire, stageDatas[stagedataidx].thirddam
                        };
        arrayChoices = arrayChoices.OrderBy(x => System.Guid.NewGuid()).ToArray(); // 回答候補のリストをシャッフル

        //その他の選択肢のランダムか
        string[] arrayMissChoices = new string[]
                        {  stageDatas[UnityEngine.Random.Range(0,stageDatas.Length)].secondsire,
                        stageDatas[UnityEngine.Random.Range(0,stageDatas.Length)].siredam, stageDatas[UnityEngine.Random.Range(0,stageDatas.Length)].thirdsire, stageDatas[UnityEngine.Random.Range(0,stageDatas.Length)].secondsiredam,
                        stageDatas[UnityEngine.Random.Range(0,stageDatas.Length)].siredamsire, stageDatas[UnityEngine.Random.Range(0,stageDatas.Length)].siredamdam, stageDatas[UnityEngine.Random.Range(0,stageDatas.Length)].dam,
                         stageDatas[UnityEngine.Random.Range(0,stageDatas.Length)].seconddam, stageDatas[UnityEngine.Random.Range(0,stageDatas.Length)].damsiresire,
                        stageDatas[UnityEngine.Random.Range(0,stageDatas.Length)].damsiredam, stageDatas[UnityEngine.Random.Range(0,stageDatas.Length)].seconddamsire, stageDatas[UnityEngine.Random.Range(0,stageDatas.Length)].thirddam
                        };
        arrayMissChoices = arrayMissChoices.OrderBy(x => System.Guid.NewGuid()).ToArray(); // 回答候補のリストをシャッフル

        //arrayChoicesとarrayMissChoicesの重複した選択肢になった場合、削除
        List<string> questionList = new List<string>();
        questionList.Add(arrayChoices[0]);
        questionList.Add(arrayMissChoices[1]);
        questionList.Add(arrayMissChoices[2]);
        questionList.Add(arrayMissChoices[3]);

        IEnumerable<string> duplicateAnswers = questionList.Distinct();

        for(var i = 0; i < 3; i++) { 
            if (Equals(arrayChoices[0], arrayMissChoices[i]))
            {
                questionList.Remove(arrayMissChoices[i]);
                //questionList.Add()
            }
        }

        //重複を避ける方法2
        var noDuplicatelist = new List<string>();

        var numbers = new string[] {
            arrayChoices[0], arrayMissChoices[1], arrayMissChoices[2], arrayMissChoices[3],arrayMissChoices[4],arrayMissChoices[5]};
        foreach (var num in numbers)
        {
            // リストに要素が無ければ追加
            if (!noDuplicatelist.Contains(num))
            {
                noDuplicatelist.Add(num);
            }
        }


        //血統表の描画
        choices.OnNext((stageDatas[stagedataidx].HorseName, stageDatas[stagedataidx].sire, stageDatas[stagedataidx].secondsire,
                        stageDatas[stagedataidx].siredam, stageDatas[stagedataidx].thirdsire, stageDatas[stagedataidx].secondsiredam,
                        stageDatas[stagedataidx].siredamsire, stageDatas[stagedataidx].siredamdam, stageDatas[stagedataidx].dam,
                        stageDatas[stagedataidx].broodMareSire, stageDatas[stagedataidx].seconddam, stageDatas[stagedataidx].damsiresire,
                        stageDatas[stagedataidx].damsiredam, stageDatas[stagedataidx].seconddamsire, stageDatas[stagedataidx].thirddam,
                        noDuplicatelist[0], noDuplicatelist[1], noDuplicatelist[2], noDuplicatelist[3]
                      ));

        ++stagedataidx;
    }

}
