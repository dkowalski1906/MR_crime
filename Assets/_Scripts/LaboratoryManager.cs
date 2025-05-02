using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaboratoryManager : MonoBehaviour
{
    //son
    [Header("Laboratory Sound")]
    public GameObject laboratorySoundGO;
    public AudioClip laboratorymSound;
    private AudioSource audioSourceLaboratorySound;

    private void Start()
    {
        //son d'ambiance de laboratoire
        audioSourceLaboratorySound = laboratorySoundGO.GetComponent<AudioSource>();
        if (audioSourceLaboratorySound == null)
            audioSourceLaboratorySound = laboratorySoundGO.AddComponent<AudioSource>();

        audioSourceLaboratorySound.clip = laboratorymSound;

        audioSourceLaboratorySound.volume = 0.5f;
        audioSourceLaboratorySound.loop = true;
        audioSourceLaboratorySound.Play();
    }

    /// <summary>
    /// Chargement de la scène "Museum"
    /// </summary>
    public void GoToMuseum()
    {
        Debug.Log("GoToMuseum");
        SceneManager.LoadScene("Museum");
    }
}
