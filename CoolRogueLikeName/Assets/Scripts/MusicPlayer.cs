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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // PlayChordSequence("A", 4, "m4M");
        }
    }

    int NoteIndex(string note, int octave)
    {
        return Array.IndexOf(noteNames, note) + (octave - 1) * 12;
    }

    void Stop()
    {
        foreach (AudioSource source in sources)
        {
            source.Stop();
        }
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
        PlayChord(idx, major);
    }

    void PlayChord(int idx, bool major)
    {
        int[] chord = major ? new int[] { 0, 4, 7 } : new int[] { 0, 3, 7 };

        for (int i = 0; i < chord.Length; i++)
        {
            PlayNote(sources[i], idx + chord[i], true);
        }
    }

    void PlayChordSequence(string startNote, int octave, string chords)
    {
        int idx = NoteIndex(startNote, octave);
        bool major1 = chords[0] == 'M';
        int diff = chords[1] - '0';
        bool major2 = chords[0] == 'M';

        int[] idxs = { idx, idx + diff, idx };
        bool[] majors = { major1, major2, major1 };

        StartCoroutine(PlayChordSequenceCoroutine(idxs, majors));
    }

    IEnumerator PlayChordSequenceCoroutine(int[] idxs, bool[] majors)
    {
        for (int i = 0; i < idxs.Length; i++)
        {
            FullVolume();
            PlayChord(idxs[i], majors[i]);
            yield return new WaitForSeconds(1.0f);
            yield return CoolDown();
            yield return new WaitForSeconds(0.1f);
        }

        Stop();
    }

    IEnumerator CoolDown()
    {
        for (int i = 10; i > 0; i--)
        {
            foreach (AudioSource source in sources)
            {
                source.volume = i / 10.0f;
            }
            yield return new WaitForSeconds(0.01f);
        }
    }

    void FullVolume()
    {
        foreach (AudioSource source in sources)
        {
            source.volume = 1.0f;
        }
    }
}
