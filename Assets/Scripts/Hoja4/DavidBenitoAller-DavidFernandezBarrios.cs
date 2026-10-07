using System.Data.SqlTypes;
using UnityEngine;
using System.Collections;

public class crossfade : MonoBehaviour
{

    [Range(0.1f,6.0f)]
    [SerializeField] private float lap = 1.0f;
    [SerializeField] private AudioClip[] clips;

    private AudioSource[] sources;

    private int actClip = -1;

    int channelActive = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sources = new AudioSource[2];
        for(int i = 0; i< 1; i++)
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

    private void ChangeClip()
    {
        int next = actClip;
        do
        {
            actClip = Random.Range(0,clips.Length-1);
        }while(next == actClip);

    }

    IEnumerator crossFade()
    {
        ChangeClip();
        int otherChannel = channelActive;
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
       
        for (int t = 0; t<= lap; t+= 0.1)
        {   
            //Sube
            sources[channelActive].volume = Mathf.Sqrt(t/lap);
            //Baja
            sources[otherChannel].volume = Mathf.Sqrt((lap-t)/lap);
            yield return new WaitForSeconds(0.1f);
        }
        sources[channelActive].volume = 1.0f;
        sources[otherChannel].volume = 0.0f;
    }
    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown("N"))
            StartCorrutine(crossFade());
    }
}
