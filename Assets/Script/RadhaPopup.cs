using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System;

public class RadhaPopup : MonoBehaviour
{
    public Button clickButton;
    public TextMeshProUGUI radhaText;

    private bool isPlaying = false;

    void Start()
    {
        radhaText.gameObject.SetActive(false);
        clickButton.onClick.AddListener(PlayEffect);
    }

    private void PlayEffect()
    {
        if (isPlaying) return;

        isPlaying = true;
        clickButton.interactable = false;

        radhaText.gameObject.SetActive(true);
        radhaText.text = "Radha";

        radhaText.color = GetRandomColor();
        radhaText.transform.localScale = Vector3.zero;
        radhaText.alpha = 0f;

#if UNITY_ANDROID || UNITY_IOS
        Handheld.Vibrate();
#endif
        Sequence seq = DOTween.Sequence();
        seq.Append(radhaText.DOFade(1f, 0.3f));
        seq.Join(radhaText.transform
            .DOScale(1.2f, 0.4f)
            .SetEase(Ease.OutBack));

        seq.AppendInterval(1f);

        seq.Append(radhaText.DOFade(0f, 0.3f));
        seq.Join(radhaText.transform.DOScale(0f, 0.3f));

        seq.OnComplete(() =>
        {
            radhaText.gameObject.SetActive(false);
            clickButton.interactable = true;
            isPlaying = false;
        });
    }

    Color GetRandomColor()
    {
        return new Color(
            UnityEngine.Random.Range(0.3f, 1f),
            UnityEngine.Random.Range(0.3f, 1f),
            UnityEngine.Random.Range(0.3f, 1f)
        );
    }
}
