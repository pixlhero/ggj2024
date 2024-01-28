using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class HandCassettesVisuals : MonoBehaviour
{
    [SerializeField] private float cassettesDistance;

    [SerializeField] private Transform playingDestinationTransform;

    [SerializeField] private Transform drawingOriginTransform;

    private Cassette _lastPlayedCassette;

    private void Awake()
    {
        HandCassettesState.CassetteAdded += OnCassetteAdded;
        HandCassettesState.CassetteAddedList += OnCassetteAddedList;
        HandCassettesState.CassetteRemoved += OnCassetteRemoved;
    }

    private void OnCassetteAdded(Cassette newCassette)
    {
        if (_lastPlayedCassette != null)
            Destroy(_lastPlayedCassette.gameObject);

        newCassette.transform.SetParent(transform);
        newCassette.transform.position = drawingOriginTransform.position;
        newCassette.transform.rotation = drawingOriginTransform.rotation;
        var newLocalPos = GetLocalPosition(newCassette);

        var sequence = DOTween.Sequence();
        sequence.Insert(0,
            newCassette.transform
                .DOLocalJump(newLocalPos, 0.1f, 1, 0.5f)
        );
        sequence.Insert(0,
            newCassette.transform
                .DORotate(Vector3.zero, 0.5f)
        );
        sequence.OnComplete(() => {
            AudioHandler.singleton.Play_Effect_DropCassette();
        });

        foreach (var cassette in HandCassettesState.Singleton.Cassettes)
        {
            if (cassette == newCassette)
                continue;

            var localTargetPos = GetLocalPosition(cassette);
            cassette.Sequence?.Kill();
            cassette.Sequence = DOTween.Sequence();
            cassette.Sequence.Insert(0,
                cassette.transform
                    .DOLocalMove(localTargetPos, 0.3f)
            );
        }
    }

    private void OnCassetteAddedList(List<Cassette> newCassettes)
    {
        foreach (var newCassette in newCassettes)
        {
            newCassette.transform.SetParent(transform);
            newCassette.transform.position = drawingOriginTransform.position;
            newCassette.transform.rotation = drawingOriginTransform.rotation;

            var localTargetPos = GetLocalPosition(newCassette);

            newCassette.Sequence = DOTween.Sequence();
            newCassette.Sequence.Insert(0,
                newCassette.transform
                    .DOLocalJump(localTargetPos, 0.1f, 1, 0.5f)
            );
            newCassette.Sequence.Insert(0,
                newCassette.transform
                    .DORotate(Vector3.zero, 0.5f)
            );
        }
    }

    private void OnCassetteRemoved(Cassette cassette)
    {
        _lastPlayedCassette = cassette;
        cassette.transform.SetParent(null);

        var cassettePlayer = FindObjectOfType<CassettePlayer>();
        cassettePlayer.PlayAnimation();
        
        cassette.SetToStraightRotation();

        cassette.Sequence?.Kill();
        cassette.Sequence = DOTween.Sequence();

        cassette.Sequence.InsertCallback(0.2f, () => { AudioHandler.singleton.Play_Effect_PutinCassette(); });

        cassette.Sequence.Insert(0,
            cassette.transform
                .DOJump(playingDestinationTransform.position, 0.25f, 1, 0.5f)
        );
        cassette.Sequence.Insert(0,
            cassette.transform
                .DORotate(playingDestinationTransform.rotation.eulerAngles, 0.3f)
        );
        cassette.Sequence.InsertCallback(0.7f, () => { cassette.TypeData.Play(); });

        // cassette.Sequence.OnComplete( () => { AudioHandler.singleton.Play_Effect_PutinCassette(); });

        /*
        foreach (var remainingCassette in HandCassettesState.Singleton.Cassettes)
        {
            var newLocalPos = GetLocalPosition(remainingCassette);
            remainingCassette.Sequence?.Kill();
            remainingCassette.Sequence = DOTween.Sequence();
            remainingCassette.Sequence.Insert(0,
                remainingCassette.transform
                    .DOLocalMove(newLocalPos, 0.3f)
            );
        }
        */
    }

    private Vector3 GetLocalPosition(Cassette cassette)
    {
        var cassetteIndex = HandCassettesState.Singleton.Cassettes.IndexOf(cassette);
        var cassettesCount = HandCassettesState.Singleton.Cassettes.Count;
        var leftOffset = cassettesDistance * (cassettesCount - 1f) / 2f;

        var targetPos = new Vector3(cassetteIndex * cassettesDistance - leftOffset, 0f, 0f);
        return targetPos;
    }
}