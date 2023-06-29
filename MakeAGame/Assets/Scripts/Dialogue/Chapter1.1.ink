->Chapter1

VAR CheckControl = true
==Chapter1 ==
哦？看来你的运气很不好。#speaker: 引导者 

廷达罗斯猎犬，这可是个可爱的小家伙。这个游戏里充满了这种不讲道理的怪物，和你们的世界一样。 #speaker: 引导者 #Camera: 视角

这太可怕了......一只疯狗！#player: Dead


哈哈，*可怕的东西*还多得是呢！现在，让我们看看要怎么保住你的小命。#speaker: 引导者 

还记得之前你拿到的初始牌组吗？卡牌分为两种：【生死牌】与【混沌牌】。
【生死牌】生时是你的斥候，死时它们是最强力的武器，帮你抵御怪物。
【混沌牌】，它们处于生死的交界处，能对生死牌造成各种影响。#speaker: 引导者 


现在，*左键*拖拽你的调查员*弗朗西斯*登场！#speaker: 引导者 #controlG: 操作
{
-CheckControl == true:  
->FirstCondition 
-CheckControl == false:
->EndCondition
}

==FirstCondition
瞧这个蠢货，他看起来迷了路。现在，*左键*拖拽他，为他指引前进的方向吧。#speaker: 引导者 #controlG: 操作
{
 -CheckControl == true:
->SecondCondition 
-CheckControl == false:
->EndCondition
}
==SecondCondition==
记住：每个【调查员】都是一场战斗的核心，他们能发挥出各种强大的力量影响战场。。#speaker: 引导者   

他们的能力你可以将鼠标悬浮在其上看到，其余的你就要自己摸索了——喂！实习生！你有没有在认真听讲？。#speaker: 引导者

调查员是你最珍贵的朋友，只有让他们存活着探索完整个屋子，到达右上角的门时，你才算胜利，也就是——通关！#speaker: 引导者 #wait:等待
弗朗西斯看起来陷入了苦战。现在，*右键*拖拽你的卡牌放置过去。#speaker: 引导者  #controlG: 操作
 {
 -CheckControl == true:
->ThirdCondition
-CheckControl == false:
->EndCondition
}
==EndCondition==
哦？看来你很不服气我的指挥？那你自己玩吧！以后也别叫我。#speaker: 引导者 #pass: 通关
->ContinueStory
==ThirdCondition==
发现了吗？左键拖拽你的卡牌，它们就是可移动的生物；右键拖拽，它们就是你的死亡魔法！#speaker: 引导者 

它们......是活着的吗？为什么会变成卡牌？#player: Dead

叹气*哦，孩子，这不是你需要操心的事儿。#speaker: 引导者

现在，用你剩余的卡牌支援弗朗西斯，干掉这群可悲的杂种！#speaker: 引导者 #pass: 通关
->ContinueStory



==ContinueStory==
没受伤吧？#NPC: 弗朗西斯 

感谢您的出手相助，我的名字应该叫……亡灵。#player: Dead

笑*弗朗西斯·维兰德·瑟斯顿，我是一名调查员。我会成为你的助力，帮助你完成生死对调。 #NPC: 弗朗西斯
感谢您，但是，为什么呢？#player: Dead

我是一名生者，对*陨星*和诡异的好奇驱使着我来到了这个地方，探索这片*陨星坠落之地*。#NPC: 弗朗西斯

这个地方还有不少我这样的调查员。但房主的房门只有充满了死气的亡灵才能打开，所以我才会来帮助你互惠互利，很不错吧，嗯？ #NPC: 弗朗西斯

那便谢谢你了。#player: Dead #reward: 奖励


->END
