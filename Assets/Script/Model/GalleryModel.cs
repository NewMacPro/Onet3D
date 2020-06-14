
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class GalleryModel
{
    public static string[] galleryImageName = new string[] { "yi" , "che" , "mao" , "cai" };
    public static string[] galleryName = new string[] { "Sofa illustrated book", "Car", "Birthday hat", "Vegetables" };
    public static int[] galleryGold = new int[] { 0, 0, 0, 0 };
    public static int[] galleryTypeCount = new int[] { 24, 16, 25, 20 };

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
        return Random.Range(0 , galleryName.Length);
    }
}
    
    

