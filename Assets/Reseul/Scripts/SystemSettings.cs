// Copyright (c) 2024 Takahiro Miyaura
// Released under the MIT license
// http://opensource.org/licenses/mit-license.php

using UnityEngine;

public class SystemSettings : MonoBehaviour
{
    private static SystemSettings instance;

    public static SystemSettings Instance
    {
        get
        {
            if (instance == null) instance = FindObjectOfType<SystemSettings>();
            return instance;
        }
    }

    public bool AutoStartOnDisplayConnected
    {
        get
        {
            if (PlayerPrefs.HasKey("AutoStartOnDisplayConnected"))
            {
                return PlayerPrefs.GetInt("AutoStartOnDisplayConnected") == 1;
            }

            PlayerPrefs.SetInt("AutoStartOnDisplayConnected", 0);
            return false;
        }
        set => PlayerPrefs.SetInt("AutoStartOnDisplayConnected", value ? 1 : 0);
    }

    public bool AutoManageXRCamera
    {
        get
        {
            if (PlayerPrefs.HasKey("AutoManageXRCamera"))
            {
                return PlayerPrefs.GetInt("AutoManageXRCamera") == 1;
            }

            PlayerPrefs.SetInt("AutoManageXRCamera", 1);
            return true;
        }
        set => PlayerPrefs.SetInt("AutoManageXRCamera", value ? 1 : 0);
    }
}