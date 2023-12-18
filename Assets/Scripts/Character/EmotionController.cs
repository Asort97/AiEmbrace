using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionController : MonoBehaviour
{
    [SerializeField] private string[] faceEmotions;
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayRandomAnimation()
    {
        animator.SetTrigger(faceEmotions[Random.Range(0, faceEmotions.Length)]);
    }

    public void PlayBodyAnimation(int idAnim)
    {
        animator.SetFloat("animation", idAnim);
    }
}
