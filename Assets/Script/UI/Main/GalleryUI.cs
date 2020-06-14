using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

 class GalleryUI: UIBase{
     private Text goldText;
     private int galleryMaxCount = 4;
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
         for (int i = 0; i < galleryMaxCount; i++)
         {
             GameObject item = ViewUtils.CreatePrefabAndSetParent(content, "GalleryItem");
             ViewUtils.SetImage(item.transform, "Icon", "gallery_" + i);
             ViewUtils.SetText(item.transform, "Title", "Title");
         }
     }
     
     void Refresh()
     {
         goldText.text = SaveModel.player.gold.ToString();
     }
}

