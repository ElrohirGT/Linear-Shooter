using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;

namespace Utilities.AudioSystem
{
    /// <summary>
    /// Manages all the sound effects or background music in the game.
    /// </summary>
    public static class AudioManager
    {

        static bool _alreadyInitialized = false;

        static Dictionary<Audios, AudioClip> _audios;

        static string _audioPath = "Sound Effects";

        /// <summary>
        /// Get's the AudioSource that will be used to play audios.
        /// </summary>
        private static AudioSource _audioSource;

        /// <summary>
        /// Initializes the AudioManager, filling the <c>audios</c> dictionary with all the corresponding audios.
        /// </summary>
        public static void Initialize(AudioSource audioSource)
        {
            if (_alreadyInitialized)
                return;

            ForceInitialize(audioSource);
        }

        /// <summary>
        /// Forces the initialization, possibly replacing previous values and references.
        /// </summary>
        public static void ForceInitialize(AudioSource audioSource)
        {
            _alreadyInitialized = true;
            _audioSource = audioSource;
            Type enumType = typeof(Audios);
            foreach (var audioName in Enum.GetNames(enumType))
            {
                _audios.Add(
                    (Audios)Enum.Parse(enumType, audioName),
                    Resources.Load<AudioClip>($"{_audioPath}/{audioName}")
                );
            }
        }

        /// <summary>
        /// Plays the background song in a loop using the <paramref param="audio"> supplied.
        /// </summary>
        public static void PlayBackgroundMusic(Audios audio)
        {
            _audioSource.clip = _audios[audio];
            _audioSource.Play();
        }

        /// <summary>
        /// Plays the supplied <paramref param="audio"> once.
        /// </summary>
        public static void PlayOneShot(Audios audio)
        {
            _audioSource.PlayOneShot(_audios[audio]);
        }
    }
}