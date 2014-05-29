﻿using Card.Server;
using System;
using System.Collections.Generic;

namespace Card.Client
{
    public static class RunAction
    {
        /// <summary>
        /// 获目标位置
        /// </summary>
        public static CardUtility.delegateGetPutPos GetPutPos;
        #region"开始动作"
        /// <summary>
        /// 开始一个动作
        /// </summary>
        /// <param name="game"></param>
        /// <param name="CardSn"></param>
        /// <param name="ConvertPosDirect">亡语的时候，需要倒置方向</param>
        /// <returns></returns>
        public static List<String> StartAction(GameManager game, String CardSn, Boolean ConvertPosDirect = false)
        {
            Card.CardBasicInfo card = Card.CardUtility.GetCardInfoBySN(CardSn);
            List<String> ActionCodeLst = new List<string>();
            switch (card.CardType)
            {
                case CardBasicInfo.CardTypeEnum.法术:
                    ActionCodeLst.Add(UseAbility(CardSn));
                    //初始化 Buff效果等等
                    Card.AbilityCard ablity = (Card.AbilityCard)CardUtility.GetCardInfoBySN(CardSn);
                    ablity.CardAbility.Init();
                    var ResultArg = game.UseAbility(ablity, ConvertPosDirect);
                    if (ResultArg.Count != 0) ActionCodeLst.AddRange(ResultArg);
                    break;
                case CardBasicInfo.CardTypeEnum.随从:
                    int MinionPos = GetPutPos(game);
                    ActionCodeLst.Add(UseMinion(CardSn, MinionPos));
                    var minion = (Card.MinionCard)card;
                    //初始化
                    minion.Init();
                    game.MySelf.RoleInfo.BattleField.PutToBattle(MinionPos, minion);
                    ActionCodeLst.AddRange(minion.发动战吼(game));
                    game.MySelf.RoleInfo.BattleField.ResetBuff();
                    break;
                case CardBasicInfo.CardTypeEnum.武器:
                    ActionCodeLst.Add(UseWeapon(CardSn));
                    game.MySelf.RoleInfo.Weapon = (Card.WeaponCard)card;
                    break;
                case CardBasicInfo.CardTypeEnum.奥秘:
                    ActionCodeLst.Add(UseSecret(CardSn));
                    game.MySelf.奥秘.Add((Card.SecretCard)card);
                    break;
                default:
                    break;
            }
            return ActionCodeLst;
        }
        /// <summary>
        /// 使用武器
        /// </summary>
        /// <param name="CardSn">卡牌号码</param>
        /// <returns></returns>
        public static String UseWeapon(String CardSn)
        {
            String actionCode = String.Empty;
            actionCode = ActionCode.strWeapon + CardUtility.strSplitMark + CardSn;
            return actionCode;
        }
        /// <summary>
        /// 使用奥秘
        /// </summary>
        /// <param name="CardSn"></param>
        /// <returns></returns>
        public static String UseSecret(String CardSn)
        {
            String actionCode = String.Empty;
            actionCode = ActionCode.strSecret + CardUtility.strSplitMark + CardSn;
            return actionCode;
        }
        /// <summary>
        /// 使用随从
        /// </summary>
        /// <param name="CardSn">卡牌号码</param>
        /// <param name="Position"></param>
        /// <returns></returns>
        public static String UseMinion(String CardSn, int Position)
        {
            String actionCode = String.Empty;
            actionCode = ActionCode.strMinion + CardUtility.strSplitMark + CardSn + CardUtility.strSplitMark + Position.ToString("D1");
            return actionCode;
        }
        /// <summary>
        /// 使用魔法
        /// </summary>
        /// <param name="CardSn">卡牌号码</param>
        /// <returns></returns>
        public static String UseAbility(String CardSn)
        {
            String actionCode = String.Empty;
            actionCode = ActionCode.strAbility + CardUtility.strSplitMark + CardSn;
            return actionCode;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="game"></param>
        /// <param name="MyPos"></param>
        /// <param name="YourPos"></param>
        /// <returns></returns>
        public static List<String> Fight(GameManager game, int MyPos, int YourPos)
        {
            String actionCode = String.Empty;
            //FIGHT#1#2
            actionCode = ActionCode.strFight + CardUtility.strSplitMark + MyPos + CardUtility.strSplitMark + YourPos;
            List<String> ActionCodeLst = new List<string>();
            ActionCodeLst.Add(actionCode);
            ActionCodeLst.AddRange(game.Fight(MyPos, YourPos, false));
            return ActionCodeLst;
        }
        #endregion
    }
}
