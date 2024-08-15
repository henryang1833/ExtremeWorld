using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using Network;
using SkillBridge.Message;
using GameServer.Entities;
using GameServer.Services;

namespace GameServer.Managers
{
    class EquipManager : Singleton<EquipManager>
    {
        internal Result EquipItem(NetConnection<NetSession> sender, int slot, int itemId, bool isEquip)
        {
            Character character = sender.Session.Character;
            if (!character.itemManager.Items.ContainsKey(itemId))
                return Result.Failed;

            UpdateEquip(character.Data.Equips, slot, itemId, isEquip);

            DBService.Instance.Save();
            return Result.Success;
        }

        unsafe void UpdateEquip(byte[] equipData, int slot, int itemId, bool isEquip)
        {
            fixed(byte * pt = equipData)
            {
                int* slotid = (int*)(pt + slot * sizeof(int));
                if (isEquip)
                    *slotid = itemId;
                else
                    *slotid = 0;
            }
        }
    }
}
