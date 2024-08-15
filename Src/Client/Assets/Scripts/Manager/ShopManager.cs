using System;
using Common.Data;
using Services;

namespace Managers
{
    class ShopManager : Singleton<ShopManager>
    {
        public void Init()
        {
            NPCManager.Instance.RegisterNpcEnvent(NPCFunctional.InvokeShop, OnOpenShop);
        }

        private bool OnOpenShop(NPCDefine npc)
        {
            this.ShowShop(npc.Param);
            return true;
        }

        private void ShowShop(int shopId)
        {
            ShopDefine shop;
            if(DataManager.Instance.Shops.TryGetValue(shopId,out shop))
            {
                UIShop uiShop = UIManager.Instance.Show<UIShop>();
                if (uiShop != null)
                    uiShop.SetShop(shop);
            }
        }

        internal bool BuyItem(int shopId, int shopItemID)
        {
            ItemService.Instance.SendBuyItem(shopId, shopItemID);
            return true;
        }
    }
}
