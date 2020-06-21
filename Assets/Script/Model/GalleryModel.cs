
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
    public static List<GalleryData> alreadyGalleryData = Config.Instance.GetGalleryDataByIds(SaveModel.player.galleryIds);
    public static List<GalleryData> currentGalleryData = Config.Instance.GetGalleryDataByIds(SaveModel.player.currentGalleryIds);

    //是否有这个图集
    public static bool HaveThisGallery(int id) {
        return alreadyGalleryData.Find((GalleryData gData) =>
        {
            return gData.id == id;
        }) != null;
    }

    //是否使用这个图集
    public static bool GalleryUsed(int id)
    {
        return currentGalleryData.Find((GalleryData gData) =>
        {
            return gData.id == id;
        }) != null;
    }

    //解锁这个图集
    public static void UnlockGallery(int id)
    {
        SaveModel.player.galleryIds.Add(id);
        SaveModel.player.currentGalleryIds.Add(id);
        SaveModel.ForceStorageSave();
        GalleryModel.alreadyGalleryData = Config.Instance.GetGalleryDataByIds(SaveModel.player.galleryIds);
        GalleryModel.currentGalleryData = Config.Instance.GetGalleryDataByIds(SaveModel.player.currentGalleryIds);
    }

    //使用图集
    public static void UseGallery(int id)
    {
        Debug.Log(!GalleryModel.HaveThisGallery(id));
        Debug.Log(GalleryModel.GalleryUsed(id));
        if (!GalleryModel.HaveThisGallery(id))
        {
            return;
        }
        if (GalleryModel.GalleryUsed(id))
        {
            return;
        }
        Debug.Log(SaveModel.player.currentGalleryIds.Count);

        SaveModel.player.currentGalleryIds.Add(id);
        GalleryModel.currentGalleryData = Config.Instance.GetGalleryDataByIds(SaveModel.player.currentGalleryIds);
        SaveModel.ForceStorageSave();
    }

    //卸下图集
    public static void UnloadGallery(int id)
    {
        if (!GalleryModel.HaveThisGallery(id))
        {
            return;
        }
        if (SaveModel.player.currentGalleryIds.Count <= Config.Instance.commonNode.GetInt("minGalleryCount"))
        {
            return;
        }
        SaveModel.player.currentGalleryIds.Remove(id);
        GalleryModel.currentGalleryData = Config.Instance.GetGalleryDataByIds(SaveModel.player.currentGalleryIds);
        SaveModel.ForceStorageSave();
    }

    //随机已有图集
    public static int GetRandomGallery() { 
        int index = Random.Range(0, currentGalleryData.Count);
        return currentGalleryData[index].id;
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
    
    

