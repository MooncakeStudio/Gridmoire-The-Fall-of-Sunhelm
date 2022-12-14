using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField] private int dineroJugador = 0;
    private AudioClip clipMenu;
    private int mundoSeleccionado = 1;
    private AudioSource audioSorce;
    public GameManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        
        DontDestroyOnLoad(gameObject);

        
        audioSorce = GetComponent<AudioSource>();
        audioSorce.volume = 1;
        clipMenu = audioSorce.clip;
    }

    private void Start()
    {
        if (!PlayerPrefs.HasKey("PrimeraVez"))
        {
            PlayerPrefs.SetInt("PrimeraVez", 1);
            dineroJugador = 0;
            PlayerPrefs.SetInt("Dinero", dineroJugador);
            PlayerPrefs.Save();
        }
        else
        {
            dineroJugador = PlayerPrefs.GetInt("Dinero");

        }
    }

    private void Update()
    {
        //if (em.GetEjercito().Count > 0)
            //Debug.Log(em.GetEjercito()[0]);
    }

    public void restarDinero(int dinero) { dineroJugador -= dinero; }
    public void sumarDinero(int dinero) { dineroJugador += dinero; }

    public void cambiarMundo(int mundo) { mundoSeleccionado = mundo; }

    public int getDineroJugador() { return dineroJugador; }

    public int GetMundoSeleccionado() { return mundoSeleccionado; }

    public AudioClip GetClipMenu() { return clipMenu; }

    public AudioSource GetAudioSource() { return audioSorce; }

    public void setClip(AudioClip clip) {
        if (clip.name != audioSorce.clip.name)
        {
            audioSorce.Stop();
            audioSorce.clip = clip;
            audioSorce.Play();
        }
    }

    public IEnumerator playSound(AudioClip clip)
    {
        audioSorce.clip = clip;
        audioSorce.Stop();
        audioSorce.Play();
        yield return new WaitForSeconds(clip.length);
        audioSorce.clip = clipMenu;
        audioSorce.Stop();
        audioSorce.Play();
    }
    
    public IEnumerator playOnce(AudioClip clip)
    {
        audioSorce.PlayOneShot(clip);
        yield return new WaitForSeconds(clip.length);
    }
}
