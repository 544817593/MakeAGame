->NPC2
VAR CheckStamina = "false"
==NPC2==
你看到了一个东西，一个好奇怪的东西。#speaker:
你不禁感到疑惑，这到底是什么东西呢？长得四四方方的奇怪东西。#speaker:
->Choice
==Choice==
*[拿起来]
->PickUp
*[无视]
->Ignore

==PickUp==
{
-CheckStamina == true:
<>“滋滋——你上当了！你中了老化烟雾，哈，哈，哈！” 四四方方的怪东西中发出了更怪的电子音，随即，一阵灰色的烟雾喷射而出，你被笼罩在了其中。#speaker: #show: 显示 #showNPC: NPC显示 

-CheckStamina ==false:
 <>你想拿起来，但不愧是个怪东西，手一滑它掉到了地上，你不想再捡起来它了。#speaker: #show: 显示 #showNPC: NPC显示 

}
->END
==Ignore==
无论是什么怪东西都无法引起你的好奇心，你相信你就是最谨慎的人类。#speaker: #show: 显示 #showNPC: NPC显示 


->END