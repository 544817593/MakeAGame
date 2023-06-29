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

    public AudioClip human_move_sound;
    public AudioClip bird_move_sound;
    public AudioClip flyBug_move_sound;
    public AudioClip mollusk_move_sound;
    public AudioClip twoLegs_move_sound;
    public AudioClip fourLegs_move_sound;
    public AudioClip move_fail_sound;
    public AudioClip piece_death_sound;

    public AudioClip alienation1_sound;
    public AudioClip alienation2_sound;
    public AudioClip focus1_sound;
    public AudioClip focus2_sound;

    public AudioClip hit_sound1;
    public AudioClip hit_sound2;
    public AudioClip hit_sound3;
    public AudioClip hit_sound4;
    public AudioClip hit_sound5;
    public AudioClip humanHit_sound1;
    public AudioClip humanHit_sound2;
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
    public AudioPlayer Play_move_sound(AudioTypeEnum audioTypeEnum)
    {
        if(audioTypeEnum == AudioTypeEnum.Human)
        {
            return AudioKit.PlaySound(human_move_sound);
        }
        else if(audioTypeEnum == AudioTypeEnum.Bird)
        {
            return AudioKit.PlaySound(bird_move_sound);
        }
        else if (audioTypeEnum == AudioTypeEnum.FlyBug)
        {
            return AudioKit.PlaySound(flyBug_move_sound);
        }
        else if (audioTypeEnum == AudioTypeEnum.Mollusk)
        {
            return AudioKit.PlaySound(mollusk_move_sound);
        }
        else if (audioTypeEnum == AudioTypeEnum.TwoLegs)
        {
            return AudioKit.PlaySound(twoLegs_move_sound);
        }
        else if (audioTypeEnum == AudioTypeEnum.FourLegs)
        {
            return AudioKit.PlaySound(fourLegs_move_sound);
        }
        return null;
    }
    public void Play_move_fail_sound()
    {
        AudioKit.PlaySound(move_fail_sound);
    }
    public void Play_piece_death_sound()
    {
        AudioKit.PlaySound(piece_death_sound);
    }
    public void Play_alienation1_sound()
    {
        AudioKit.PlaySound(alienation1_sound);
    }
    public void Play_alienation2_sound()
    {
        AudioKit.PlaySound(alienation2_sound);
    }
    public void Play_focus1_sound()
    {
        AudioKit.PlaySound(focus1_sound);
    }
    public void Play_focus2_sound()
    {
        AudioKit.PlaySound(focus2_sound);
    }

    public void Play_rand_hit_sound()
    {
        int num = UnityEngine.Random.Range(1, 6);
        if(num == 1)
        {
            Play_hit_sound1();
        }
        else if(num == 2)
        {
            Play_hit_sound2();
        }
        else if (num == 3)
        {
            Play_hit_sound3();
        }
        else if (num == 4)
        {
            Play_hit_sound4();
        }
        else if (num == 5)
        {
            Play_hit_sound5();
        }
    }
    public void Play_rand_humanHit_sound()
    {
        int num = UnityEngine.Random.Range(1, 3);
        if (num == 1)
        {
            Play_humanHit_sound1();
        }
        else if (num == 2)
        {
            Play_humanHit_sound2();
        }
    }
    public void Play_hit_sound1()
    {
        AudioKit.PlaySound(hit_sound1);
    }
    public void Play_hit_sound2()
    {
        AudioKit.PlaySound(hit_sound2);
    }
    public void Play_hit_sound3()
    {
        AudioKit.PlaySound(hit_sound3);
    }
    public void Play_hit_sound4()
    {
        AudioKit.PlaySound(hit_sound4);
    }
    public void Play_hit_sound5()
    {
        AudioKit.PlaySound(hit_sound5);
    }
    public void Play_humanHit_sound1()
    {
        AudioKit.PlaySound(humanHit_sound1);
    }
    public void Play_humanHit_sound2()
    {
        AudioKit.PlaySound(humanHit_sound2);
    }

    //public AudioClip hit_sound1;
    //public AudioClip hit_sound2;
    //public AudioClip hit_sound3;
    //public AudioClip hit_sound4;
    //public AudioClip hit_sound5;
    //public AudioClip humanHit_sound1;
    //public AudioClip humanHit_sound2;
}
