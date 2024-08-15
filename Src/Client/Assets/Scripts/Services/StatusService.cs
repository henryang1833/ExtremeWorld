using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;
using System;
using SkillBridge.Message;
using Network;
namespace Services
{
    public class StatusService : Singleton<StatusService>, IDisposable
    {
        public delegate bool StatusNotifyHandler(NStatus status);

        Dictionary<StatusType, StatusNotifyHandler> eventMap = new Dictionary<StatusType, StatusNotifyHandler>();
        HashSet<StatusNotifyHandler> handlers = new HashSet<StatusNotifyHandler>();

        public void Init()
        {

        }

        public void RegisterStatusNotify(StatusType function,StatusNotifyHandler action)
        {
            if (handlers.Contains(action))
                return;
            if (!eventMap.ContainsKey(function))
            {
                eventMap[function] = action;
            }
            else
            {
                eventMap[function] += action;
            }

            handlers.Add(action);
        }

        public StatusService()
        {
            MessageDistributer.Instance.Subscribe<StatusNotify>(this.OnStatusNotify);
        }

        private void OnStatusNotify(object sender, StatusNotify notify)
        {
            foreach(NStatus status in notify.Status)
            {
                Notify(status);
            }
        }

        private void Notify(NStatus status)
        {
            if(status.Type == StatusType.Money)
            {
                if (status.Action == StatusAction.Add)
                    Models.User.Instance.AddGold(status.Value);
                else if (status.Action == StatusAction.Delete)
                    Models.User.Instance.AddGold(-status.Value);
            }

            StatusNotifyHandler handler;
            if(eventMap.TryGetValue(status.Type,out handler))
            {
                handler(status);
            }
        }

        public void Dispose()
        {
            throw new  Exception ();
        }
    }
}