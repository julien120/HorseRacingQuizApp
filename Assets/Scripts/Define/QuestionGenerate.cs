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
        //15
        public readonly string HorseName;
        public readonly string sire;//父
        public readonly string secondsire;//父父
        public readonly string siredam;//父母
        public readonly string thirdsire;//父父父
        public readonly string secondsiredam;//父父母
        public readonly string siredamsire;//父母父
        public readonly string siredamdam;//父母母

        public readonly string dam;//母
        public readonly string broodMareSire;//母父
        public readonly string seconddam;//母母
        public readonly string damsiresire;//母父父
        public readonly string damsiredam;//母父母
        public readonly string seconddamsire;//母母父
        public readonly string thirddam;//母母母

        public StageData(string _HorseName, string _sire, string _secondsire, string _siredam, string _thirdsire,
            string _secondsiredam, string _siredamsire, string _siredamdam, string _dam, string _broodMareSire,
            string _seconddam, string _damsiresire, string _damsiredam, string _seconddamsire, string _thirddam)
        {
            HorseName = _HorseName;
            sire = _sire;//父
            dam = _dam; //母
            secondsire = _secondsire;//父父
            siredam = _siredam;//父母
            broodMareSire = _broodMareSire;
            seconddam = _seconddam;

            thirdsire = _thirdsire;//父父父
            secondsiredam = _secondsiredam;//父父母
            siredamsire = _siredamsire;
            siredamdam = _siredamdam;
            damsiresire = _damsiresire;
            damsiredam = _damsiredam;
            seconddamsire = _seconddamsire;
            thirddam = _thirddam;

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
            if (cols.Length != 15) continue; //6項目なければwhile脱出

            stagecsvdata.Add(
                new StageData(
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


}
