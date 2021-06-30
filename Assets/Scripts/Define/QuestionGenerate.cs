using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Linq;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ThemeOfQuiz")]
public class QuestionGenerate : ScriptableObject
{
    [SerializeField] public string filename = "";

    public enum CommandType
    {
        SETSPEED,
        PUTENEMY
    }
    //文字列と列挙体の一致、対応表
    static readonly Dictionary<string, CommandType> commandlist =
        new Dictionary<string, CommandType>()
        {
            {"SETSPEED",CommandType.SETSPEED },
            {"PUTENEMY",CommandType.PUTENEMY },
        };

    /// <summary>
    /// ファイルから読み込んだ内容をそれぞれ対応する変数に格納するstruct変数
    /// stageDataに引数で持たせることができる
    /// </summary>
    public struct StageData
    {
        public readonly string question;
        public readonly string a;
        public readonly string b;
        public readonly string c;
        public readonly string d;
        public readonly string answer;
        public StageData(string _question, string _a, string _b, string _c, string _d, string _answer)
        {
            question = _question;
            a = _a;
            b = _b;
            c = _c;
            d = _d;
            answer = _answer;
        }

    }

    StageData[] stageDatas;

    private int stagedataidx = 0;


    public void Load()
    {
        var stagecsvdata = new List<StageData>();
        var csvdata = Resources.Load<TextAsset>(filename).text;
        //文字列読み込み1行,区切りずつに
        StringReader sr = new StringReader(csvdata);
        while (sr.Peek() != -1)
        {
            var line = sr.ReadLine();
            var cols = line.Split(',');
            if (cols.Length != 6) continue; //6項目なければwhile脱出

            stagecsvdata.Add(
                new StageData(
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


}
