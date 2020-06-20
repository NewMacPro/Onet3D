
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GalleryData
{
    public string imgName = "";
    public string name = "";
    public int gold = 0;
    public int typeCount = 0;
}

class GalleryModel
{
    public static GalleryData[] galleryData = Config.Instance.GetGalleryData();

    //是否有这个图集
    public static bool HaveThisGallery(string name) {
        return true;
    }

    //是否使用这个图集
    public static bool GalleryUsed(string name) {
        return true;
    }

    //解锁这个图集
    public static void UnlockGallery(string name)
    { 
    
    }

    //使用图集
    public static void UseGallery(string name ,bool isUse)
    {
        
    }

    //随机已有图集
    public static int GetRandomGallery() { 
        // TODO
        return Random.Range(0, galleryData.Length - 1);
    }
    public static string GetImgByType(int typeIndex, int type, out int itemType)
    {
        typeIndex = typeIndex % GalleryModel.galleryData.Length;
        GalleryData gd = GalleryModel.galleryData[typeIndex];
        string name = gd.imgName;
        itemType = type;
        if (type == -1)
        {
            return "";
        }
        type = type % gd.typeCount + 1;
        itemType = type;
        return "img_" + name + "_" + type;
    }
}
    
    

