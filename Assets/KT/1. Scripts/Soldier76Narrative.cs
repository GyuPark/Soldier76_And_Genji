using UnityEngine;
using Photon.Pun;

namespace Gyu
{
    public class Soldier76Narrative : MonoBehaviourPun
    {
        AudioSource audioSource;
        public AudioClip spawn;
        public AudioClip[] killAudioClip;

        PhotonView _photonView;


        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }
        private void OnEnable()
        {
            _photonView = GetComponentInParent<PhotonView>();

            audioSource.clip = spawn;
            PlayAudio(audioSource.clip);
        }

        public void PlayAudio(AudioClip clip)
        {
            if (!_photonView.IsMine)
                return;

            audioSource.clip = clip;
            audioSource.PlayOneShot(audioSource.clip);
        }
    } 
}
