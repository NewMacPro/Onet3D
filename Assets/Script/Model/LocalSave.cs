using System;
using System.Collections.Generic;
using UnityEngine;
using LitJson;


    //系统本地存储
    public class LocalSave
    {
        private const string KEY_LOCAL_SAVE_USER = "USER_SAVE";
        public static int client_id = 0;

        //保存本地ID
        public static int GetLocalID()
        {
            if (client_id != 0)
                return client_id;

            int id = PlayerPrefs.GetInt("LocalID");

            if (id == 0)
            {
                DateTime dt = DateTime.Now;
                System.Random rand = new System.Random((int)dt.Ticks);
                client_id = rand.Next() + 1;
                PlayerPrefs.SetInt("LocalID", client_id);
            }

            return id;
        }

        public static string LoadUser()
        {
            return PlayerPrefs.GetString(KEY_LOCAL_SAVE_USER);
        }

        public static void SaveUser(string userInfoStr)
        {
            PlayerPrefs.SetString(KEY_LOCAL_SAVE_USER, userInfoStr);
            PlayerPrefs.Save();
        }

        public static void SaveSomething<T>(T value, string key)
        {
            string str = JsonMapper.ToJson(value);
            PlayerPrefs.SetString(key, str);
            PlayerPrefs.Save();
        }

        public static T LoadSomething<T>(string key)
        {
            string str = PlayerPrefs.GetString(key);
            T info = JsonMapper.ToObject<T>(str);
            return info;
        }

        public static bool SoundOn()
        {
            string yes_or_no = PlayerPrefs.GetString("UseSound");
            if (yes_or_no.Equals(""))       //尚未创建，默认出声音
                return true;

            return yes_or_no.Equals("yes");
        }

        public static bool MusicOn()
        {
            string yes_or_no = PlayerPrefs.GetString("UseMusic");
            if (yes_or_no.Equals(""))
                return true;

            return yes_or_no.Equals("yes");
        }

        public static bool AudioOn()
        {
            string yes_or_no = PlayerPrefs.GetString("UseAudio");
            if (yes_or_no.Equals(""))       //尚未创建，默认出声音
                return true;

            return yes_or_no.Equals("yes");
        }


        public static void SetSound(bool yes_or_no)
        {
            PlayerPrefs.SetString("UseSound", yes_or_no ? "yes" : "no");
        }

        public static void SetMusic(bool yes_or_no)
        {
            PlayerPrefs.SetString("UseMusic", yes_or_no ? "yes" : "no");
        }

        public static void SetAudio(bool yes_or_no)
        {
            PlayerPrefs.SetString("UseAudio", yes_or_no ? "yes" : "no");
        }

        public static void ClearSave()
        {
            PlayerPrefs.DeleteAll();
        }

        public static bool HasKey(string key)
        {
            return PlayerPrefs.HasKey(key);
        }

        public static void SaveInt(int value, string key)
        {
            PlayerPrefs.SetInt(key, value);
            PlayerPrefs.Save();
        }

        public static int GetInt(string key)
        {
            if (!PlayerPrefs.HasKey(key))
                return 0;
            return PlayerPrefs.GetInt(key);
        }

        public static void DeleteInt(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }


        public static void SaveString(string value, string key)
        {
            PlayerPrefs.SetString(key, value);
            PlayerPrefs.Save();
        }

        public static string GetString(string key)
        {
            if(PlayerPrefs.HasKey(key))
                return PlayerPrefs.GetString(key);
            return "";
        }

        public static void DeleteString(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public static void SaveFloat(float value, string key)
        {
            PlayerPrefs.SetFloat(key, value);
            PlayerPrefs.Save();
        }

        public static void SaveBool(bool value, string key)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            PlayerPrefs.Save();
        }

        public static bool GetBool(string key)
        {
            // LuaUtil.Log("GetBool:" + PlayerPrefs.GetInt(key));
            return PlayerPrefs.GetInt(key) == 1;
        }

        public static float GetFloat(string key)
        {
            return PlayerPrefs.GetFloat(key);
        }

        public static void DeleteFloat(string key)
        {
            PlayerPrefs.DeleteKey(key);
        }

        public static void SaveBaseObj(object value, string key)
        {
            if (value is int)
                PlayerPrefs.SetInt(key, (int)value);
            else if (value is string)
                PlayerPrefs.SetString(key, (string)value);
            else if (value is float)
                PlayerPrefs.SetFloat(key, (float)value);

            PlayerPrefs.Save();
        }

        public static string LoadString(string key)
        {
            if (PlayerPrefs.HasKey(key))
                return PlayerPrefs.GetString(key);
            
            return "";
        }
    }

