
->NPC1
VAR CheckSprite = "false"
VAR CheckStrength = "False"
==NPC1==
嗷呜......呜呜......
你看到一只廷达罗斯猎犬趴在地板上。见到你的到来，它从喉咙深处发出了微弱的悲鸣声，间又夹杂着粗犷的低吼，显然是在警告你不要靠近，却又十分害怕。#speaker: 猎犬
->Choice
==Choice==
*[前进]
->forward
*[离开]
->Leave
*[杀了它 ]
->Kill

==forward==

{ 
- CheckSprite == true:
<>你靠近廷达罗斯猎犬，听到了它发出愈发慌乱的声音，心中得到了极大满足。
- CheckSprite == false:
<>你尝试想要靠近，但你的精神力量无法支持你这么做，你为自己的软弱而痛苦万分。
}
->END
==Leave==
你无视了它，离开了这里。

->END
==Kill==
{ 
- CheckStrength == true:
<>你看着它要死不活的样子，心想它与其残破不堪地活着，还不如一死了之。你抽出一把刀走向它。看出了你意图的廷达罗斯猎犬猎犬突然凶狠起来，它拼着最后一把力气冲上来，用剩下的最后一颗獠牙刺进了你的身体。这是来自处于绝境生命的最后反击。
- CheckStrength == false:
<>你看着它要死不活的样子，心想它与其残破不堪地活着，还不如一死了之。你抽出一把刀走向它,但或许是天意使然，刀在刺向它的一瞬间从刀柄处断裂。你想，或许是这个可悲的廷达罗斯猎犬命中还有数吧。你扔掉刀柄，离开了。
}
->END