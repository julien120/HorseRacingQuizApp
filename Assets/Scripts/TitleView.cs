using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cysharp.Threading.Tasks;

public class TitleView : MonoBehaviour
{
    [SerializeField] private RectTransform image;
    // Start is called before the first frame update
    void Start()
    {
        LoadHomeScene().Forget();
    }

    private async UniTaskVoid LoadHomeScene()
    {
        await image.DOScale(0f, 0.01f).SetEase(Ease.Linear).AsyncWaitForCompletion();
        await DOVirtual.DelayedCall(1.4f, () => image.DOScale(7f, 0.4f).SetEase(Ease.Linear)).AsyncWaitForCompletion();
      //  await 
        await DOVirtual.DelayedCall(0.5f, () => SceneController.Instance.LoadHomeScene()).AsyncWaitForCompletion();

    }

}
