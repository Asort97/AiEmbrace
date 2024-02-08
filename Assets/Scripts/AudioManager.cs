using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip clickSound;
    [SerializeField] private AudioClip receiveMsgSound;
    [SerializeField] private Button[] allButtons;

    private void Awake() 
    {
        Instance = this;
        foreach (Button button in allButtons)
        {
            button.onClick.AddListener(() => PlayClickSound());
        }
    }

    public void PlayClickSound()
    {
        audioSource.PlayOneShot(clickSound);
    }

    public void PlayNofiticationSound()
    {
        audioSource.PlayOneShot(receiveMsgSound);
    }

}
