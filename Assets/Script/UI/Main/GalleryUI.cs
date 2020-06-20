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
             string imageName = gd.imgName;
             string name = gd.name;
             GameObject item = ViewUtils.CreatePrefabAndSetParent(content, "GalleryItem");

             ViewUtils.SetText(item.transform, "Title", name);
             for (int j = 1; j <= 5; j++)
             {
                 ViewUtils.SetImage(item.transform, "ItemGroup/Item" + j + "/Bg/Image", "img_" + imageName + "_" + j);
                 Debug.Log("img_" + imageName + "_" + j);
             }

             ViewUtils.SetActive(item.transform, "ButtonGroup/UnlockBtn", GalleryModel.HaveThisGallery(name));
             ViewUtils.SetActive(item.transform, "ButtonGroup/AdBtn", !GalleryModel.HaveThisGallery(name) && i == 4);
             ViewUtils.SetActive(item.transform, "ButtonGroup/PayBtn", !GalleryModel.HaveThisGallery(name) && i != 4);
             ViewUtils.SetText(item.transform, "PayBtn/Text", gd.gold.ToString());
             if (!GalleryModel.HaveThisGallery(name) && i != 4)
             {
                 if (SaveModel.player.gold < gd.gold)
                 {
                     item.transform.FindAChild<Image>("PayBtn/Text").color = Color.red;
                 }
                 else
                 {
                     item.transform.FindAChild<Image>("PayBtn/Text").color = Color.white;
                 }
             }
             

             ViewUtils.AddButtonClick(item.transform, "AdBtn", delegate ()
             {
                OnClickAdBtn(i);
             } );
             ViewUtils.AddButtonClick(item.transform, "PayBtn", delegate()
             {
                 OnClickPay(i);
             });
             
         }
     }
     
     void Refresh()
     {
         goldText.text = SaveModel.player.gold.ToString();
     }

     void OnClickAdBtn(int index)
     {
         GalleryModel.UnlockGallery(GalleryModel.galleryData[index].name);
     }

     void OnClickPay(int index)
     {
         GalleryData gd = GalleryModel.galleryData[index];
         if (!SaveModel.CheckGold(gd.gold))
         {
             return;
         }
         SaveModel.UseGold(gd.gold);
         GalleryModel.UnlockGallery(gd.name);
     }
}

