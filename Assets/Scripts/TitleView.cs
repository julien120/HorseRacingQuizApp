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
        await DOVirtual.DelayedCall(1.9f, () => image.DOScale(5f, 0.7f).SetEase(Ease.InOutBounce)).AsyncWaitForCompletion();
        await DOVirtual.DelayedCall(1f, () => SceneController.Instance.LoadHomeScene()).AsyncWaitForCompletion();

    }

}
