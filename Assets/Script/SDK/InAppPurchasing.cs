//using UnityEngine;
//using UnityEngine.Purchasing;
//using LitJson;
//using System;

//public class InAppPurchasing : Singleton<InAppPurchasing>, IStoreListener
//{
//    //IAP组件相关的对象，m_Controller里存储着商品信息
//    private static IStoreController m_Controller;//存储商品信息
//    private static IAppleExtensions m_AppleExtensions;
//    private static IExtensionProvider m_Extensions;
    

//    private static bool PurchaseAvailable = true;//IAP可用状态
//    private static bool InternetAvailable;//是否初始化成功
//    public Action<PurchaseEventArgs> OnPaySuccessAction;
//    public Action OnPayFailedAction;

//    public static InAppPurchasing instance;
//    public void Init(string[] idArr)
//    {
//        //如果没有连击网络，关闭IAP功能
//        if (Application.internetReachability == NetworkReachability.NotReachable)
//        {
//            PurchaseAvailable = false;
//        }
//        else
//        {
//            PurchaseAvailable = true;
//            //如果没有初始化成功
//            if (InternetAvailable == false)
//            {
//                InitUnityPurchase(idArr);//初始化
//            }
//        }
//    }

//    public void InitUnityPurchase(string[] idArr)//初始化方法
//    {
//        var module = StandardPurchasingModule.Instance();//标准采购模块
//        var builder = ConfigurationBuilder.Instance(module);//配置模式
//        //添加多个ID到builder里，ID命名方式建议字母+数字结尾，比如ShuiJing_1,ShuiJing_2，
//        //注意ProductType的类型，Consumable是可以无限购买(比如水晶)，NonConsumable是只能购买一次(比如关卡)，Subscription是每月订阅(比如VIP)

//        for (int i = 0; i < idArr.Length; i++)
//        {
//            string productId = idArr[i].ToString();
//            builder.AddProduct(productId, ProductType.Consumable);
//        }
//        //开始初始化
//        UnityPurchasing.Initialize(this, builder);
//    }
//    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)//初始化成功回调
//    {
//        m_Controller = controller;
//        m_Extensions = extensions;
//        //m_Controller = controller;
//        //m_AppleExtensions = extensions.GetExtension<iappleextensions>();
//        //m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);//登记 购买延迟 监听器
//        InternetAvailable = true;//初始化成功
//        Debug.Log("IAP初始化成功");
//    }
//    public void OnInitializeFailed(InitializationFailureReason error)//初始化失败回调
//    {
//        InternetAvailable = false;
//        if (error == InitializationFailureReason.PurchasingUnavailable)
//        {
//            Debug.Log("手机设置了禁止APP内购");
//        }
//        Debug.Log("IAP初始化失败");
//        Debug.Log("失败原因：" + error);
//    }
//    public void OnPurchaseFailed(Product item, PurchaseFailureReason r)//购买失败回调
//    {
//        Debug.Log("购买失败");
//        if (OnPayFailedAction != null)
//        {
//            OnPayFailedAction();
//        }
//    }

//    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)//购买成功回调
//    {
//        Debug.Log("购买成功");
//        if (OnPaySuccessAction != null)
//        {
//            OnPaySuccessAction(e);
//        }

//        return PurchaseProcessingResult.Complete;
//        //return 0;//complete 购买完成
//        //return 1;//pending 未购买
//    }

//    private void OnDeferred(Product item)//购买延迟提示
//    {
//        Debug.Log("网速慢");
//    }

//    public void OnPurchaseClicked(string ProductId)//发起购买函数，在你的商品按钮上拖这个方法进入
//    {
//        m_Controller.InitiatePurchase(ProductId);
//    }

//    public string GetProductData()
//    {
//        if (InternetAvailable && PurchaseAvailable)
//        {
//            JsonData jData = new JsonData();

//            jData.SetJsonType(JsonType.Array);
//            int index = 0;
//            foreach (var product in m_Controller.products.all)
//            {
//                jData.Add(new JsonData());
//                jData[index]["title"] = product.metadata.localizedTitle;
//                jData[index]["des"] = product.metadata.localizedDescription;
//                jData[index]["price"] = product.metadata.localizedPriceString;
//                jData[index]["code"] = product.metadata.isoCurrencyCode;
//                jData[index]["id"] = product.transactionID;
//                index += 1;
//            }
//            return JsonMapper.ToJson(jData);
//        }
//        else
//        {
//            return "";
//        }
//    }

//    public string GetProductDataById(string id)
//    {
//        if (InternetAvailable && PurchaseAvailable)
//        {
//            JsonData jData = new JsonData();

//            var product = m_Controller.products.WithID(id);

//            jData["title"] = product.metadata.localizedTitle;
//            jData["des"] = product.metadata.localizedDescription;
//            jData["price"] = product.metadata.localizedPriceString;
//            jData["code"] = product.metadata.isoCurrencyCode;
//            jData["id"] = id;

//            return JsonMapper.ToJson(jData);
//        }
//        else
//        {
//            return "";
//        }
//    }


//}