using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private List<AudioSource> sources;

    public AudioClip[] A; // choral music notes in inspector don't ask why
    public AudioClip[] pianoNotes; // todo
    private AudioClip[] choralNotes;
    private string[] noteNames = { "A", "A#", "B", "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#" };

    // Start is called before the first frame update
    void Start()
    {
        choralNotes = A;

        sources = new List<AudioSource>();
        for (int i = 0; i < 3; i++)
        {
            sources.Add(gameObject.AddComponent<AudioSource>());
        }

        // PlayChord("C", 4, true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    int NoteIndex(string note, int octave)
    {
        return Array.IndexOf(noteNames, note) + (octave - 1) * 12;
    }

    void PlayNote(AudioSource source, int idx, bool choral)
    {
        AudioClip clip = choral ? choralNotes[idx] : pianoNotes[idx];

        source.clip = clip;
        source.Play();
    }

    void PlayChord(string startNote, int octave, bool major)
    {
        int idx = NoteIndex(startNote, octave);
        int[] chord = major ? new int[] { 0, 4, 7 } : new int[] { 0, 3, 7 };

        for (int i = 0; i < chord.Length; i++)
        {
            PlayNote(sources[i], idx + chord[i], true);
        }
    }
}
