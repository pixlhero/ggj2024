using System;
using DG.Tweening;
using UnityEngine;

public class Cassette : MonoBehaviour
{
    public enum CassetteType
    {
        DotDotDot,
        Haha,
        Cry
    }

    [SerializeField] private Transform modelTransform;
    [SerializeField] private Transform hoverModelTransform;
    [SerializeField] private Transform normalModelTransform;

    private Sequence _localModelSequence;
    
    public CassetteType Type { get; private set; }

    public Sequence Sequence;

    private void Start()
    {
        modelTransform.localPosition = normalModelTransform.localPosition;
        modelTransform.localRotation = normalModelTransform.localRotation;
    }

    private void OnMouseDown()
    {
        HandCassettesState.Singleton.PlayCassette(this);
    }

    private void OnMouseEnter()
    {
        MoveModelToTransform(hoverModelTransform);
    }

    private void OnMouseExit()
    {
        MoveModelToTransform(normalModelTransform);
    }

    private void MoveModelToTransform(Transform targetTransform)
    {
        _localModelSequence?.Kill();
        _localModelSequence = DOTween.Sequence();
        _localModelSequence.Insert(0,
            modelTransform
                .DOLocalMove(targetTransform.localPosition, 0.2f)
        );
        _localModelSequence.Insert(0,
            modelTransform
                .DOLocalRotate(targetTransform.localRotation.eulerAngles, 0.2f)
        );
    }
}