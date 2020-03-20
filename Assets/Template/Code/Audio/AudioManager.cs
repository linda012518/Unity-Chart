using System.Collections.Generic;
using UnityEngine;
using Framework;

namespace Audio
{
    public class AudioManager : BaseManager
    {
        static AudioManager instance = new AudioManager();//1

        public static AudioManager Instance { get { return instance; } }

        public AudioManager() : base(AreaCode.Audio)//2
        {//10
            audioSource = Camera.main.GetComponent<AudioSource>();
            dic = new Dictionary<string, AudioClip>();
            Add(0, this);
        }

        public void Init() { }

        AudioSource audioSource;
        Dictionary<string, AudioClip> dic;

        public override void Execute(int eventCode, params object[] message)
        {
            if (eventCode != 0) return;
            PlayAudio(message[0].ToString());
        }

        void PlayAudio(string audioName)
        {
            audioSource.clip = Get(audioName);
            audioSource.Play();
        }

        public AudioClip Get(string path)
        {
            if (!dic.ContainsKey(path))
                dic.Add(path, Load("Sound/" + path));
            return dic[path];
        }

        AudioClip Load(string path)
        {
            return Resources.Load<AudioClip>(path);
        }

    }
}