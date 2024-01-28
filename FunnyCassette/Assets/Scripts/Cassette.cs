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

        // intro cassettes:
        Play,
        Quit,
        Credits,
    }

    [SerializeField] private Transform modelTransform;
    [SerializeField] private Transform hoverModelTransform;
    [SerializeField] private Transform normalModelTransform;

    [SerializeField] private Renderer cassetteRenderer;

    [SerializeField] private TMP_Text labelText;

    private Sequence _localModelSequence;

    private Collider _collider;

    public CassetteLabel TypeData { get; private set; }

    public CassetteType Type => TypeData.type;

    public Sequence Sequence;

    private TextPresenter _textPresenter;

    private Quaternion _initialLocalRotation;

    private void Awake()
    {
        _collider = GetComponent<Collider>();
        _textPresenter = FindObjectOfType<TextPresenter>();
    }

    private void Start()
    {
        _initialLocalRotation = normalModelTransform.localRotation;

        var randomYRot = UnityEngine.Random.Range(-20f, 20f);
        normalModelTransform.localRotation *= Quaternion.Euler(0, randomYRot, 0);

        modelTransform.localPosition = normalModelTransform.localPosition;
        modelTransform.localRotation = normalModelTransform.localRotation;
    }

    public bool CanClick()
    {
        return (GameManager.Singleton.State == GameManager.GameState.PlayerTurn ||
                GameManager.Singleton.State == GameManager.GameState.Starting &&
                IntroController.Singleton.ChosenCassette == null) &&
               !_textPresenter.IsReadingSomething;
    }

    private void Update()
    {
        _collider.enabled = CanClick();
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

    public void SetToStraightRotation()
    {
        normalModelTransform.localRotation = _initialLocalRotation;
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

        _localModelSequence.OnComplete(() =>
        {
            if (targetTransform == normalModelTransform)
            {
                AudioHandler.singleton.Play_Effect_DropCassette();
            }
            else if (targetTransform == hoverModelTransform)
            {
                AudioHandler.singleton.Play_Effect_LiftCassette();
            }
        });
    }
}