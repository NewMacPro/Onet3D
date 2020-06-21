
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GalleryData
{
    public int id = -1;
    public string imgName = "";
    public string name = "";
    public string unlockType = "";
    public int gold = 0;
    public int typeCount = 0;
}

class GalleryModel
{
    public static GalleryData[] galleryData = Config.Instance.GetGalleryData();
    public static List<GalleryData> alreadyGalleryData = Config.Instance.GetAlreadyGalleryData();

    //是否有这个图集
    public static bool HaveThisGallery(int id) {
        return alreadyGalleryData.Find((GalleryData gData) =>
        {
            return gData.id == id;
        }) != null;
    }

    //是否使用这个图集
    public static bool GalleryUsed(int id) {
        return true;
    }

    //解锁这个图集
    public static void UnlockGallery(int id)
    {
        SaveModel.player.galleryIds.Add(id);
        SaveModel.ForceStorageSave();
        GalleryModel.alreadyGalleryData = Config.Instance.GetAlreadyGalleryData();
    }

    //使用图集
    public static void UseGallery(string name ,bool isUse)
    {
        
    }

    //随机已有图集
    public static int GetRandomGallery() { 
        int index = Random.Range(0, alreadyGalleryData.Count);
        return alreadyGalleryData[index].id;
    }

    public static string GetImgByType(int galleryId, int type, out int itemType)
    {
        GalleryData gd = GalleryModel.GetGalleryById(galleryId);
        string name = gd.imgName;
        itemType = type;
        if (type == -1)
        {
            return "";
        }
        type = (type - 1) % gd.typeCount + 1;
        itemType = type;
        return "img_" + name + "_" + type;
    }

    public static GalleryData GetGalleryById(int id)
    {
        foreach (GalleryData item in galleryData)
        {
            if (item.id == id)
            {
                return item;
            }
        }
        return null;
    }
}
    
    

