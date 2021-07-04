using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour
{
    //動画広告のPlacementID
    private static readonly string BANNER_PLACEMENT_ID = "Banner";
    private const string iosID = "4197442";
    private const string androidID = "4197443";

    void Awake()
    {
#if UNITY_ANDROID
                string gameID = androidID;
#else
        string gameID = iosID;
#endif

        //広告の初期化
        //Advertisement.Initialize(gameID, testMode: false);
        //Advertisement.Initialize(gameID, testMode: true, enablePerPlacementLoad: true);
        Advertisement.Initialize(gameID, testMode: true, enablePerPlacementLoad: true);
        StartCoroutine(ShowBannerWhenInitialized());

    }
    private void Start()
    {
        // ShowBannerAd();
    }



    IEnumerator ShowBannerWhenInitialized()
    {
        while (!Advertisement.isInitialized)
        {
            yield return new WaitForSeconds(0.3f);
        }
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
        Advertisement.Banner.Show(BANNER_PLACEMENT_ID);
    }

    /// <summary>
    /// バナー広告の表示
    /// </summary>
    public void ShowBannerAd()
    {
        //広告全体の準備が出来ているかチェック
        if (!Advertisement.IsReady())
        {
            Debug.LogWarning("広告全体の準備が出来ていません");
            return;
        }

        //表示したい広告の準備が出来ているかチェック
        var state = Advertisement.GetPlacementState(BANNER_PLACEMENT_ID);
        if (state != PlacementState.Ready)
        {
            Debug.LogWarning($"{BANNER_PLACEMENT_ID}の準備が出来ていません。現在の状態 : {state}");
            return; ;
        }

        //バナー広告の表示場所の設定
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);

        //バナー広告の表示
        Advertisement.Banner.Show(BANNER_PLACEMENT_ID);
    }

    /// <summary>
    /// バナー広告の非表示
    /// </summary>
    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }
}
