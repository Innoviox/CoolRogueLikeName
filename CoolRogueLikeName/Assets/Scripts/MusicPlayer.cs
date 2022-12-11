using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private List<AudioSource> sources;

    public AudioClip mainMenuMusic;
    public AudioClip music;
    public AudioClip bossMusic;
    public AudioClip deathScreenMusic;
    public float maxVolume = 0.5f;

    public int state = 0; // 0 => main menu, 1 => in game, 2 => boss fight, 3 => death screen
    private int oldState = -1;

    // Start is called before the first frame update
    void Start()
    {
        sources = new List<AudioSource>();
        for (int i = 0; i < 4; i++)
        {
            sources.Add(gameObject.AddComponent<AudioSource>());
            sources[i].loop = true;
            sources[i].volume = maxVolume;
        }

        sources[0].clip = mainMenuMusic;
        sources[1].clip = music;
        sources[2].clip = bossMusic;
        sources[3].clip = deathScreenMusic;
    }

    // Update is called once per frame
    void Update()
    {
        if (oldState != state)
        {
            StartCoroutine(SwitchTo(state));
            oldState = state;
        }
    }

    IEnumerator SwitchTo(int state)
    {
        yield return CoolDown();
        foreach (AudioSource source in sources)
        {
            source.Stop();
        }
        FullVolume();

        sources[state].Play();

        yield return null;
    }

    IEnumerator CoolDown()
    {
        for (float i = maxVolume * 10f; i > 0; i--)
        {
            foreach (AudioSource source in sources)
            {
                source.volume = i / 10.0f;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    void FullVolume()
    {
        foreach (AudioSource source in sources)
        {
            source.volume = maxVolume;
        }
    }
}
