using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioClip hoverSound;
    public AudioClip clickSound;
    public AudioClip click_Dialogue;
    public AudioClip add_sound;
    public AudioClip minus_sound;
    public AudioClip pack_open_sound;
    public AudioClip hover_Zoom;
    public AudioClip[] flip_card;
    public AudioClip random_audio;
    public AudioClip bag_openSound;
    public AudioClip intro_bgm; // 第一次进入战斗场景前的BGM
    public AudioClip combat_bgm; // 战斗场景BGM
    public AudioClip room_bgm; // 其它房间BGM

    public void Play_Room_Bgm()
    {
        AudioKit.PlayMusic(room_bgm);
    }
    public void Play_Combat_Bgm()
    {
        AudioKit.PlayMusic(combat_bgm);
    }

    public void Play_Intro_Bgm()
    {
        AudioKit.PlayMusic(intro_bgm);
    }

    public void Play_Click_Dialogue()
    {
        AudioKit.PlaySound(click_Dialogue);
    }
    public void Play_Add()
    {
        AudioKit.PlaySound(add_sound);
    }
    public void Play_Minus()
    {
        AudioKit.PlaySound(minus_sound);
    }
    public void Play_Open_pack()
    {
        AudioKit.PlaySound(pack_open_sound);
    }
    public void Play_Flip()
    {
        random_audio= flip_card[Random.Range(0, flip_card.Length)];

        AudioKit.PlaySound(random_audio);
    }
    public void Play_Hover_Zoom()
    {
        AudioKit.PlaySound(hover_Zoom);
    }
    public void Play_Open_Bag()
    {
        AudioKit.PlaySound(bag_openSound);
    }
}
