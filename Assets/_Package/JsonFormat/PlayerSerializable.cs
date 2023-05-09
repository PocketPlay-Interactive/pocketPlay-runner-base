using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerSerializable
{
    [System.Serializable]
    public class Renting
    {
        public string Id;
        public double Time;

        public Renting() { }

        public Renting(string id, double time)
        {
            Id = id;
            Time = time;
        }
    }

    public string Id = "hihidochoo";
    public string Name = "";
    public bool IsAds = true;
    public int Level;
    public int Gold;
    public int Cash;
    public int Gem;
    public int AvatarID = 2;
    public int TotalGames;
    public int TotalWins = 0;
    public int HighestSocre = 0;
    public List<string> Boughts;
    public List<Renting> Rentings;
    public string Language;

    public PlayerSerializable()
    {
        Id = SystemInfo.deviceUniqueIdentifier;
        IsAds = true;
        Level = 0;
        Gold = 0;
        Cash = 0;
        Gem = 0;
        AvatarID = 2;
        TotalGames = 0;
        TotalWins = 0;
        HighestSocre = 0;
        Name = "You";
        Boughts = new List<string>();
        Rentings = new List<Renting>();
    }
}

