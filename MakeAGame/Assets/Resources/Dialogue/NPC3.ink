->NPC3
VAR CheckCharisma = "false"
VAR CheckP = "false"
==NPC3==
一个穿着西装的稻草人竖立在房间的正中间，叼着烟斗，摇摇晃晃地，让人不禁疑惑它是怎么立在那里的。#speaker: 稻草人
...... #speaker: 稻草人
你好。#speaker: Dead
...... #speaker: 稻草人
它一言不发，就像一具尸体。
->Choice
==Choice==
*[继续打招呼]
->continue
*[离开]
->Leave
==continue==
你好呀？你会说话吗？
{
-CheckCharisma == true:

<>稻草人沉默了片刻后终于有了反应。它微微弯折腰身，上半身以优雅的角度倾斜着，你认为它大概向你行了个礼。
你也向它回了个礼，离开了.#show: 显示 #showNPC: NPC显示 

-CheckCharisma == false:
<>你尝试向它继续打招呼，但是无论如何，稻草人都毫无反应。
难道它只是个普通的、没有生命的存在？真是遗憾。#show: 显示 #showNPC: NPC显示 

}

->END
==Leave==
{
-CheckP == true:
<>你转身刚想离开，还没走几步，稻草人便一蹦一跳地跟在了你的身后。
你回过头来注视着它，“你干什么？”
它一言不发，像是一具尸体。
你继续行走，“哒哒哒”，它继续在你身后蹦蹦跳跳。
你明白了......它大概是在和你玩木头人的游戏。#show: 显示 #showNPC: NPC显示 


-CheckP ==false:
<>你离开了，稻草人还是立在那里，岿然不动。#show: 显示 #showNPC: NPC显示 

}

->END