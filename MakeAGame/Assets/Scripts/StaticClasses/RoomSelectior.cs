using QFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class RoomSelector
{

    private static System.Random rng = new System.Random();

    /// <summary>
    /// 返回第一章随机抽出的房间组合
    /// 随机排列房间顺序：共4-5个战斗房、1个NPC房、1-2个探索房、1-2个商人房。
    /// 房间排列规则：
    /// 最少要有8个房间，最多可以有10个房间；
    /// 连续两个战斗房后第三个房间一定不是战斗房；
    /// 如果有两个商人房，两个商人房中间至少开两个房间，且其中至少一个为战斗房；
    /// 如果有两个探索房，两个探索房中间至少隔开一个房间；
    /// 连续三个房间里一定有至少一个战斗房间。
    /// 第一个商人房后面一定是NPC房。
    /// </summary>
    /// <returns>一个列表的房间组合</returns>
    public static List<RoomEnum> GenerateRooms()
    {
        // 总最小/最大房间数量
        int minRooms = 8;
        int maxRooms = 10;

        // 随机选择房间数量
        int numRooms = rng.Next(minRooms, maxRooms + 1);

        // 每个房间可最大最小的数量
        int maxExploreRoom = 2;
        int maxMerchantRoom = 2;
        int maxCombatRoom = 5;
        int minExploreRoom = 1;
        int minMerchantRoom = 1;
        int minCombatRoom = 4;

        // 生成房间列表
        List<RoomEnum> rooms = new List<RoomEnum>();

        // 记录NPC房间是否被生成
        bool npcRoomPlaced = false;

        // 记录各个房间数量
        int exploreRooms = 0;
        int combatRooms = 0;
        int merchantRooms = 0;

        // 商人房后是否有战斗房
        bool combatAfterMerchant;

        // 记录连续/非连续战斗房间
        int consecutiveCombatRooms;
        int consecutiveNonCombatRooms;

        // 记录商店房间是否被生成
        bool firstMerchantPlaced;


        // 有房间没被随机生成出来，重新随机
        while (exploreRooms < minExploreRoom || merchantRooms < minMerchantRoom || combatRooms < minCombatRoom || !npcRoomPlaced)
        {
            // 重置循环信息
            exploreRooms = 0;
            combatRooms = 0;
            merchantRooms = 0;
            npcRoomPlaced = false;
            combatAfterMerchant = false;
            firstMerchantPlaced = false;
            consecutiveCombatRooms = 0;
            consecutiveNonCombatRooms = 0;
            rooms.Clear();

            // 生成房间
            for (int i = 0; i < numRooms; i++)
            {
                // 第一个商人房后一定是NPC房
                if (i >= 1 && rooms[i - 1] == RoomEnum.Merchant && merchantRooms == 1 && !npcRoomPlaced)
                {
                    RoomEnum forceNPCRoom = RoomEnum.NPC;
                    rooms.Add(forceNPCRoom);
                    consecutiveNonCombatRooms++;
                    npcRoomPlaced = true;
                    continue;
                }

                // 确认可以生成的房间
                bool canExplore = exploreRooms < maxExploreRoom &&
                    // 两个探索房中间至少隔开一个房间
                    ((i == 0) || (rooms[i - 1] != RoomEnum.Explore) &&
                    // 连续三个房间里一定有至少一个战斗房间
                    consecutiveNonCombatRooms < 2);

                bool canCombat = combatRooms < maxCombatRoom &&
                    // 连续两个战斗房后第三个房间一定不是战斗房
                    consecutiveCombatRooms < 2;

                bool canMerchant = merchantRooms < maxMerchantRoom &&
                    // 连续三个房间里一定有至少一个战斗房间
                    consecutiveNonCombatRooms < 2 &&
                    // 如果有两个商人房，两个商人房中间至少开两个房间，且其中至少一个为战斗房；
                    (!firstMerchantPlaced || (i >= 3 && rooms[i - 1] != RoomEnum.Merchant && rooms[i - 2] != RoomEnum.Merchant && combatAfterMerchant));


                // 记录接下来可以生成的房间
                List<RoomEnum> allowedRooms = new List<RoomEnum>();

                if (canExplore) allowedRooms.Add(RoomEnum.Explore);
                if (canCombat) allowedRooms.Add(RoomEnum.Combat);
                if (canMerchant) allowedRooms.Add(RoomEnum.Merchant);

                // 随机选择一个房间，如果没有可以选择，重新循环，防止报错
                if (allowedRooms.Count == 0) break;
                int index = rng.Next(allowedRooms.Count);
                RoomEnum room = allowedRooms[index];

                // 更新各个房间数量
                if (room == RoomEnum.Explore)
                {
                    exploreRooms++;
                }
                else if (room == RoomEnum.Combat)
                {
                    combatRooms++;
                    consecutiveCombatRooms++;                       
                    consecutiveNonCombatRooms = 0;
                    if (firstMerchantPlaced) combatAfterMerchant = true;
                }
                else if (room == RoomEnum.Merchant)
                {
                    merchantRooms++;
                }


                // 战斗房间规则更新
                if (room != RoomEnum.Combat)
                {
                    consecutiveCombatRooms = 0;
                    consecutiveNonCombatRooms++;
                }

                // 商人房间规则更新
                if (room == RoomEnum.Merchant)
                {
                    firstMerchantPlaced = true;
                }

                // 添加房间
                rooms.Add(room);
            }
        }

        // Demo版本没有战斗场景池，暂时加到5个战斗房
        if (combatRooms == 4) rooms.Add(RoomEnum.Combat);
        return rooms;
    }
}
