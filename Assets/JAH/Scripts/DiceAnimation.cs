using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DiceAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.DOBlendableRotateBy(Vector3.right * 80, 0.7f).SetEase(Ease.InOutQuad).SetLoops(-1, LoopType.Yoyo);
        transform.DOBlendableRotateBy(Vector3.up * 360, 1.7f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        transform.DOBlendableRotateBy(Vector3.forward * 360, 1.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
    }

   
}
