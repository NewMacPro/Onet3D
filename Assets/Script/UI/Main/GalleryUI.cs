using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

 class GalleryUI: UIBase{
     private Text goldText;
     
     public static void Create()
     {
         GalleryUI ui = new GalleryUI();
         ui.Init();
     }
     
     void Init()
     {
         this.CurrentUIType.UIForms_Type = UIFormsType.PopUp;
         this.CurrentUIType.UIForms_ShowMode = UIFormsShowMode.PopUp;
         Redisplay();
     }
     
     public override void Redisplay()
     {
         CreateAndAttachRoot("GalleryUI");
         Attach();
         Refresh();
     }
     
     void Attach()
     {
         ViewUtils.AddButtonClick(root, "CloseBtn", Close);
         ViewUtils.SetText(root, "HintText", "Unlock more pictures");
         goldText = root.FindAChild<Text>("Gold/Text");
         InitItems();
     }
     
     void InitItems(){
         Transform content = root.FindAChild("Content");
         for (int i = 0; i < GalleryModel.galleryData.Length; i++)
         {
             GalleryData gd = GalleryModel.galleryData[i];
             GameObject item = ViewUtils.CreatePrefabAndSetParent(content, "GalleryItem");
             InitSingleItem(item.transform, gd);
         }
     }

     void InitSingleItem(Transform item, GalleryData gd)
     {
        string imageName = gd.imgName;
        string name = gd.name;

        ViewUtils.SetText(item.transform, "Title", name);
        for (int j = 1; j <= 5; j++)
        {
            ViewUtils.SetImage(item.transform, "ItemGroup/Item" + j + "/Bg/Image", "img_" + imageName + "_" + j);
        }

        bool payUnlock = gd.unlockType == Const.UNLOCK_TYPE_PAY;
        ViewUtils.SetActive(item.transform, "Choose", GalleryModel.GalleryUsed(gd.id));
        ViewUtils.SetActive(item.transform, "ButtonGroup/UnlockBtn", GalleryModel.HaveThisGallery(gd.id));
        ViewUtils.SetActive(item.transform, "ButtonGroup/AdBtn", !GalleryModel.HaveThisGallery(gd.id) && !payUnlock);
        ViewUtils.SetActive(item.transform, "ButtonGroup/PayBtn", !GalleryModel.HaveThisGallery(gd.id) && payUnlock);
        ViewUtils.SetText(item.transform, "PayBtn/Value", gd.gold.ToString());
        if (!GalleryModel.HaveThisGallery(gd.id) && payUnlock)
        {
            if (SaveModel.player.gold < gd.gold)
            {
                item.transform.FindAChild<Text>("PayBtn/Value").color = Color.red;
            }
            else
            {
                item.transform.FindAChild<Text>("PayBtn/Value").color = Color.white;
            }
        }
             

        ViewUtils.AddButtonClick(item.transform, "AdBtn", delegate ()
        {
            OnClickAdBtn(item, gd);
        });
        ViewUtils.AddButtonClick(item.transform, "PayBtn", delegate()
        {
            OnClickPay(item, gd);
        });
        ViewUtils.AddButtonClick(item.transform, "", delegate()
        {
            OnClickItem(item, gd);
        });
             
     }
     
     void Refresh()
     {
         goldText.text = SaveModel.player.gold.ToString();
     }

     void OnClickItem(Transform item, GalleryData gd)
     {
         Debug.Log(GalleryModel.GalleryUsed(gd.id));
         if (GalleryModel.GalleryUsed(gd.id))
         {
             GalleryModel.UnloadGallery(gd.id);
         }
         else
         {
             Dictionary<string, object> param = new Dictionary<string, object>();
             param["name"] = gd.name;
             FBstatistics.LogEvent("chooseatlas", param);
             GalleryModel.UseGallery(gd.id);
         }
         InitSingleItem(item, gd);
     }

     void OnClickAdBtn(Transform item, GalleryData gd)
     {
         GalleryModel.UnlockGallery(gd.id);
         InitSingleItem(item, gd);
     }

     void OnClickPay(Transform item, GalleryData gd)
     {
         if (!SaveModel.CheckGold(gd.gold))
         {
             return;
         }
         SaveModel.UseGold(gd.gold);
         GalleryModel.UnlockGallery(gd.id);
         InitSingleItem(item, gd);
     }
}

