using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class Cassette : MonoBehaviour
{
    public enum CassetteType
    {
        DotDotDot,
        Haha,
        OhNo,
    }

    [SerializeField] private Transform modelTransform;
    [SerializeField] private Transform hoverModelTransform;
    [SerializeField] private Transform normalModelTransform;

    [SerializeField] private Renderer cassetteRenderer;

    [SerializeField] private TMP_Text labelText;

    private Sequence _localModelSequence;

    private Collider _collider;
    
    public CassetteLabel TypeData{ get; private set;}

    public CassetteType Type => TypeData.type;

    public Sequence Sequence;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void Start()
    {
        modelTransform.localPosition = normalModelTransform.localPosition;
        modelTransform.localRotation = normalModelTransform.localRotation;
    }

    private void Update()
    {
        _collider.enabled = GameManager.Singleton.State == GameManager.GameState.PlayerTurn;
    }
    
    public void SetTypeData(CassetteLabel label)
    {
        TypeData = label;
        labelText.text = label.text;
        cassetteRenderer.material = label.material;
    }

    private void OnMouseDown()
    {
        HandCassettesState.Singleton.PlayCassette(this);
    }

    private void OnMouseEnter()
    {
        MoveModelToTransform(hoverModelTransform);
        CursorController.singleton.Hover();
    }

    private void OnMouseExit()
    {
        MoveModelToTransform(normalModelTransform);
        CursorController.singleton.Pointer();
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