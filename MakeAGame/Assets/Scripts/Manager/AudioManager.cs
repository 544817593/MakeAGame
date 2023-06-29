using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFramework;
using UnityEngine.UI;
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
    public AudioClip door_sound;
    public AudioClip intro_bgm; // 第一次进入战斗场景前的BGM
    public AudioClip combat_bgm; // 战斗场景BGM
    public AudioClip room_bgm; // 其它房间BGM
    public AudioClip itemUse_sound;
    public AudioClip cursor_click_invalid_sound;
    public AudioClip cursor_click_success_sound;
    public AudioClip cursor_click_cancel_sound;
    public AudioClip hide_bag_sound;
    public AudioClip drag_card_sound;
    public AudioClip place_piece_sound;
    public AudioClip wheel_hover_sound;
    public AudioClip ui_click_success_sound;
    public AudioClip ui_click_castSpell;
   
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
    public void Play_Door_sound()
    {
        AudioKit.PlaySound(door_sound);
    }
    public void Play_itemUse_sound()
    {
        AudioKit.PlaySound(itemUse_sound);
    }
    public void Play_cursor_click_invalid_sound()
    {
        AudioKit.PlaySound(cursor_click_invalid_sound);
    }
    public void Play_cursor_click_success_sound()
    {
        AudioKit.PlaySound(cursor_click_success_sound);
    }
    public void Play_cursor_click_cancel_sound()
    {
        AudioKit.PlaySound(cursor_click_cancel_sound);
    }
    public void Play_hide_bag_sound()
    {
        AudioKit.PlaySound(hide_bag_sound);
    }
    public void Play_drag_card_sound()
    {
        AudioKit.PlaySound(drag_card_sound);
    }
    public void Play_place_piece_sound()
    {
        AudioKit.PlaySound(place_piece_sound);
    }

    public void Play_click_sound()
    {
        AudioKit.PlaySound(clickSound);
    }

    public void Play_wheel_hover_sound()
    {
        AudioKit.PlaySound(wheel_hover_sound);
    }
    public void Play_ui_click_success_sound()
    {
        AudioKit.PlaySound(ui_click_success_sound);
    }
    public void Play_UI_Click_CastSpell()
    {
        AudioKit.PlaySound(ui_click_castSpell); 
    }
}