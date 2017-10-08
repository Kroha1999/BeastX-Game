using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundboom : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private AudioClip sounds;
    [SerializeField]
    private AudioClip sounds2;
    private AudioSource audioSource;
    void Awake()
    {
     
    }
    void Start () {
        audioSource = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
