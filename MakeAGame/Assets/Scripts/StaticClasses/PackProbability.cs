using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PackProbability
{
    static List<int> rarity = new List<int> { 54, 73, 82, 94, 100 }; // 每个稀有度的概率，54%白，73-54%绿等等（不计算技能影响等偏差）
    static List<int> numCardsInPack = new List<int> { 15 }; // 每个卡包有多少卡

    /// <summary>
    /// 从一个卡包里抽卡，返回抽到的卡牌ID
    /// </summary>
    /// <param name="cardPack">卡包ID</param>
    /// <param name="isGreen">是否一定为绿卡</param>
    /// <param name="isBlue">是否一定为蓝卡</param>
    /// <returns>卡牌ID</returns>
    public static int DrawCard(int cardPack, bool isGreen = false, bool isBlue = false)
    {
        int cardId;
        int number = Random.Range(1, 101); // 随机数
        int drawResult = 0; // 结果稀有度，0为白，1为绿等等

        foreach (int rarityValue in rarity)
        {
            if (number > rarityValue)
            {
                drawResult++;
                continue;
            }
            break;
        }

        if (isGreen)
        {
            drawResult = 1; // 绿色稀有度
        }
        else if (isBlue)
        {
            drawResult = 2; // 蓝色稀有度
        }


        while (true) // 循环抽卡，直到抽到的卡牌符合稀有度要求
        {
            number = Random.Range(1, numCardsInPack[cardPack] + 1); // 在卡包的卡牌数量内选择卡牌

            // 使卡牌ID对应csv文档行数
            if (cardPack == 0)
            {
                cardId = number + 1;
            }
            else
            {
                cardId = number + 1 + numCardsInPack[cardPack];
            }

            // 卡牌id对应到卡牌稀有度，匹配需要的稀有度跳出循环；TODO
            // if (GameManager.Instance.cardMan.cardBook[cardId].rarity == drawResult) break;
            break;
        }

        return cardId;
    }

}
