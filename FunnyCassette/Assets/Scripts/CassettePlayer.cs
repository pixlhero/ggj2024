using UnityEngine;

public class CassettePlayer : MonoBehaviour
{
    private Animator _animator;
    
    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void PlayAnimation()
    {
        _animator.SetTrigger("Play");
    }
}
