using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Intro : MonoBehaviour
{
    Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("anim_bucle_2")) SceneManager.LoadScene("Menu");
    }




}


public static class SigEscena
{
    public static string CrossSceneInformation { get; set; }
}
