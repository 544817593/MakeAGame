->Chapter1Part3
VAR CheckControl = true
==Chapter1Part3 ==

咦？它怎么好像变快了？#player: Dead


发现了吗？其实，这个宅子因为经常被*挤压*，所以时空会被挤得很混乱。我想想......就像是被两片面包夹住的芝士火腿！#speaker: 引导者 

也就是说，不同地方的时间流速会不一样，对吧？#player: Dead

Bingo！时间流速可能会快也可能会慢，可能对我们有利也可能不利，一切都看你自己的本领——或许，某些手牌可以给你启发。#speaker: 引导者 

哦，这只可爱的猫头鹰，看到小老鼠就耐不住本能了吧。这是它的【特性】，孩子，让它去吧。#speaker: 引导者 #controlG: 操作 #pop: 卡牌高亮
{
-CheckControl == true:  
->FirstCondition 
-CheckControl == false:

}
==FirstCondition==
看！果然对付小老鼠就要用鹰。你的其它卡牌也会有不同的特性，灵活运用，或许你在这里的存活率会更高些。#speaker: 引导者 

是妖鬼！它的速度很快，一定是时间被倍速了。#speaker: 引导者

我知道你舍不得，但有得必有失——现在，听我的，让猫头鹰去死！#speaker: 引导者 #controlG: 操作
{
-CheckControl == true:  
->continuenext
CheckControl == false:

}
==continuenext==
感谢猫头鹰。现在，到你交作业的时候了。#speaker: 引导者

->END