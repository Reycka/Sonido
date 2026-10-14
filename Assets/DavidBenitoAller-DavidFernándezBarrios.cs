
// '1' empieza la reproducción del sonido intermitente
// '2' lo para


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IntermitentSound : MonoBehaviour {
    [SerializeField]
    private int channels;
    private AudioParameters[] _speakParameters;  // audio source asosicada a la entidad
    [Range(-1f, 1f)]
    public float minPan, maxPan;
    [Range(0f, 1f )]
    public float minVol, maxVol, SourceVol,pitchVar;  // volumenes máximo y mínimo establecidos y volumen origintal del source
    [Range(0f, 30f )]
    public float minTime, maxTime;  // intervalo temporal de lanzamiento
    [Range(0, 50)]
    public int distRand, maxDist;   // 
    [Range(0f, 1.1f )]
    public float spatialBlend;
    public AudioClip[] pcmData;
    public bool enablePlayMode;

    [SerializeField]
    bool oldSustitutionPoliticy = true;

    private struct AudioParameters
    {
        public AudioSource _speaker;

        public DateTime startTime;

        public bool isEnable;

        public DateTime endTime;
    }

    void Awake(){
        _speakParameters = new AudioParameters[channels];
        for(int i = 0; i < _speakParameters.Length; ++i)
        {
            _speakParameters[i]._speaker = gameObject.AddComponent<AudioSource>();
            _speakParameters[i].isEnable = true;
        }
    }

    void Start() {

        for (int i = 0; i < _speakParameters.Length; ++i)
        {
            _speakParameters[i]._speaker.playOnAwake = false;
            _speakParameters[i]._speaker.loop = false;
            _speakParameters[i]._speaker.volume = 0.1f;
        }
    
    }

    // Update is called once per frame
    void Update() {
        if (!enablePlayMode){
            if (Input.GetKeyDown(KeyCode.Alpha1)) {
                enablePlayMode = true;
                StartCoroutine("Waitforit");
            }
        } else if (enablePlayMode) 
            if (Input.GetKeyDown(KeyCode.Alpha2)) StopSound();            
    }




    IEnumerator Waitforit() {

        // tiempo de espera aleatorio en el intervalo [minTime,maxTime]
        float waitTime = UnityEngine.Random.Range(minTime, maxTime);
        int id = -1;
        bool hasChanged = false;
        int oldestChannel = 0;
        DateTime oldestTime = DateTime.Now;
        //Bucle de política de sustitución
        for (int i = 0; i < _speakParameters.Length; ++i)
        {
            int result = DateTime.Compare(_speakParameters[i].endTime, DateTime.Now);
            if(result >= 0)
            {
                _speakParameters[i].isEnable = true;
            }

            // miramos si hay un clip asignado al source (sirve para la primera vez q se ejecuta)
            if (_speakParameters[i].isEnable)
            {
                id = i;
                _speakParameters[i].isEnable = false;
                hasChanged = true;
                break;
            }

            else
            {
                int r = DateTime.Compare(_speakParameters[i].startTime, oldestTime);
                if (r < 0)
                {
                    oldestTime = _speakParameters[i].startTime;
                    oldestChannel = i;
                }

            }


        }

        if(hasChanged == false)
        {
            if(oldSustitutionPoliticy) id = oldestChannel;
        }


        yield return new WaitForSeconds(waitTime);
        // si esta activado reproducimos sonido
        if (enablePlayMode) PlaySound(id);
    }
    
    void PlaySound(int channel) {
        if(channel >= 0)
        {
            SetSourceProperties(channel, pcmData[UnityEngine.Random.Range(0, pcmData.Length)], minVol, maxVol, distRand, maxDist, spatialBlend);
            _speakParameters[channel]._speaker.Play();
            _speakParameters[channel].startTime = DateTime.Now;
            _speakParameters[channel].endTime = _speakParameters[channel].startTime;
            _speakParameters[channel].endTime.AddSeconds(_speakParameters[channel]._speaker.clip.length);
        }
        StartCoroutine("Waitforit");
    }



    public void SetSourceProperties(int channel,AudioClip audioData, float minVol, float maxVol,
                                    int minDist, int maxDist, float SpatialBlend) {
        _speakParameters[channel]._speaker.pitch = UnityEngine.Random.Range(1 - pitchVar, 1 + pitchVar);
        _speakParameters[channel]._speaker.panStereo = UnityEngine.Random.Range(minPan, maxPan);
        _speakParameters[channel]._speaker.loop = false;
        _speakParameters[channel]._speaker.maxDistance = maxDist - UnityEngine.Random.Range(0f, distRand);
        _speakParameters[channel]._speaker.spatialBlend = spatialBlend;
        _speakParameters[channel]._speaker.clip = audioData;
        _speakParameters[channel]._speaker.volume = SourceVol + UnityEngine.Random.Range(minVol, maxVol);
    }




    void StopSound() {
        enablePlayMode = false;
    }
}    
    
