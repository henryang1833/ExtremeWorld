using Common.Data;
using UnityEngine;
using SkillBridge.Message;
using Services;
namespace Models
{
    class User : Singleton<User>
    {
        NUserInfo userInfo;
        public NUserInfo Info
        {
            get
            {
                return userInfo;
            }
        }

        public void SetupUserInfo(NUserInfo info)
        {
            this.userInfo = info;
        }

        public MapDefine CurrentMapData { get; set; }
        public NCharacterInfo CurrentCharacter { get; set; }
        public PlayerInputController CurrentCharacterObject { get; set; }
        public NTeamInfo TeamInfo { get; set; }
        public void AddGold(int gold)
        {
            this.CurrentCharacter.Gold += gold;
        }

        public int CurrentRide = 0;
        public int LastRide = 0;
        public void Ride(int id)
        {
            if(CurrentRide != id)
            {
                CurrentRide = id;
                CurrentCharacterObject.SendEntityEvent(EntityEvent.Ride, CurrentRide);
                LastRide = CurrentRide;
            }
            else
            {
                CurrentRide = 0;
                CurrentCharacterObject.SendEntityEvent(EntityEvent.Ride, 0);
            }
        }
    }
}
