using System.Data.SqlTypes;
using UnityEngine;
using System.Collections;

public class crossfade : MonoBehaviour
{

    [Range(0.1f,6.0f)]
    [SerializeField] private float lap = 1.0f;
    [SerializeField] private AudioClip[] clips;

    private AudioSource[] sources;

    //indice del clip actual para repetirlo a la hora de tirar el random
    private int actClip = -1;

    //indice para saber cual de los dos canales es el que actualmente esta sonando
    int channelActive = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Creamos dos audioSource que represetan nuestros dos canales
        sources = new AudioSource[2];
        for(int i = 0; i< 2; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
            sources[i].loop = true;
            sources[i].playOnAwake = false;
        }
        ChangeClip();
        sources[0].clip = clips[actClip];
         sources[0].volume = 1;
        sources[0].Play();
    }

    //Metodo auxiliar para hacer el cambio de del indice del clip actual
    private void ChangeClip()
    {
        int next = actClip;
        do
        {
            actClip = Random.Range(0,clips.Length);
        }while(next == actClip);

    }

    IEnumerator CrossFadeRoutine()
    {
        ChangeClip();
        int otherChannel = channelActive;
        // En funcion del canal activo cambiamos la pista del otro canal por la nueva y seteamos su volumen a 0
        if (channelActive == 0)
        {
            sources[1].clip = clips[actClip];
            sources[1].volume = 0;
            sources[1].Play();
            channelActive = 1;
        }
        else
        {
            sources[0].clip = clips[actClip];
            sources[0].volume = 0;
            sources[0].Play();
            channelActive = 0;
        }
       
       float t = 0;
       //Crossfade de los canales
        while ( t<= lap)
        {   
            t+= Time.deltaTime;
            //Sube
            sources[channelActive].volume = Mathf.Sqrt(t/lap);
            //Baja
            sources[otherChannel].volume = Mathf.Sqrt((lap-t)/lap);
            yield return new WaitForEndOfFrame();
        }
        sources[channelActive].volume = 1.0f;
        sources[otherChannel].volume = 0.0f;
    }
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.N)) 
        {
            StartCoroutine(CrossFadeRoutine());
        }
    }
}
