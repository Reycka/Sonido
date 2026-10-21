using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using UnityEngine;

[System.Serializable]
public struct ClipsArray
{
    public string name;
    public AudioClip[] clipsDataArray;
}
public class Slice2: MonoBehaviour {

    private AudioSource[] channels;

    public List<ClipsArray> clips;
   
    public float overlapTime=0.01f;
    private int overlapSamples;
    private int sRATE;

    void Awake(){
        Debug.Log("Awake");
        sRATE = AudioSettings.outputSampleRate;
    
        channels = AudioSource[clips.Count()];
        for(int i = 0; i< clips.Size(); i++)
        {
            channels[i] =  gameObject.AddComponent<AudioSource>();
        }  

        foreach(ClipsArray c in clips)
        {
            foreach(AudioClip a in c.clipsDataArray)
            {
                if (pcmDataHeads[i].channels!=1) throw new System.Exception("Tienen que ser muestras mono!!");
            }
        }  
        
        // convertimos tiempo de solapamiento a samples
        overlapSamples = (int) Mathf.Round(sRATE*overlapTime);
    }

    void Start(){
        Debug.Log("Start");
        // los fades se aplican sobre el clip original, el pitch se aplica luego. 
        // No hay que tenerlo en cuenta para aplicar los fades

        //PRimero solo fade out solo headers
        for (int i=0; i<clips[0].clipsDataArray.Length(); i++) FadeOut(clips[0].clipsDataArray[i]);

        // Aplicamos el FadeIn y FadeOut a los intermedios
        for(int i = 1;i < clips.Count-1; i++)
        {
            foreach(AudioClip a in clips[i].clipsDataArray)
            {
                FadeIn(a);
                FadeOut(a);
            }
        }
        //A los casquillos les aplicamos solo el fade in
        for (int i=0; i<clips[clips.Length()-1].clipsDataArray.Length(); i++) FadeIn(clips[clips.Length()-1].clipsDataArray[i]);
    }


    // Update is called once per frame
    void Update(){
        if (Input.GetKey(KeyCode.Alpha3)) {
            //Seteo de los clips aleatorios
            for(int i = 0; i < channels.Length(); i++)
            {
                channels[i].clip = clips[i].clipsDataArray[Random.Range(0, clips[i].clipsDataArray.Length())];
            }
     
            double clipLength = 0;
            channels[0].Play();
            // tail se reproduce solapado con head en el fragmento determinado por overlapSamples
            for(int i = 1; i< channels.Length();i++ ){
                clipLength += ((channels[i-1].clip.samples-overlapSamples) / channels[i-1].pitch);  
                channels.PlayScheduled(AudioSettings.dspTime+clipLength/sRATE);

            }
        }
    }

    //  crossFade out en [0,overlapTime]   sqrt((overlapTime-t)/overlapTime)  
    void FadeOut(AudioClip clip){
        // pasamos clip a un array de samples
        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);

        // ini: sample inicial donde aplicar fadeOut
        int ini=samples.Length-overlapSamples;
        for (int i=0; i<overlapSamples; i++){
            // mutiplicamos sample por valor de fadeOut. 
            // hay que convertir sample i a tiempo t = i/SRATE (regla de 3)
            samples[ini+i] *= Mathf.Sqrt((overlapTime-(i/sRATE))/overlapTime);
        }

        // volcamos array al clip
        clip.SetData(samples, 0);    
    }


    //  crossFadeIn en [0,overlapTime]   sqrt(t/overlapTime)  
    void FadeIn(AudioClip clip){
        float[] samples = new float[clip.samples];
        clip.GetData(samples, 0);
        
        for (int i=0; i<overlapSamples; i++){
            // ojo con la conversión de sample a tiempo
            samples[i] *= Mathf.Sqrt((i/sRATE)/overlapTime);
        }
        clip.SetData(samples, 0);    
    }


}
