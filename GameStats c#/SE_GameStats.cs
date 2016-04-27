using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SE_GameStats {
    
    //set the list of stats in the Unity inspector or through another initialization script
    [SerializeField]  
    List<SE_GameStat> _stats = new List<SE_GameStat>();
    public List<SE_GameStat> stats { get { return _stats; } }

    public SE_GameStat GetStat(string ID)
    {
        if(stats.Count == 0 || stats == null)
            return null;

        foreach(SE_GameStat stat in _stats)
        {
            if (stat.id == ID)
                return stat;
        }

        return null;
    }
    
    SE_GameStat AddStat(string ID)
    {
        if(_stats == null)
            _stats = new List<SE_GameStat>();
        
        if(GetStat(ID) != null)
        {
            Debug.Log("Warning, stat with this ID already exists!");
            return null;
        }
         
        SE_GameStat newStat = new SE_GameStat(ID);
        _stats.Add(newStat);
        return newStat;        
    }  
    
    	
}

[System.Serializable]
public class SE_GameStat
{    
    [SerializeField]
    string _id = "";
    public string id { get { return _id; } }
    
    //if the stat is numerically based, use the score field.
    [SerializeField]
    float _score = 0;
    public float score { get { return _score; } }
    public void SetScore(float newScore) { _score = newScore; }
    public void IncreaseScore(float amnt) { _score += amnt; }
    public void SubtractScore(float amnt) { _score -= amnt; }

    //if the stat is based on some other meta data, use the string meta field.
    //tranlating this data into useful information should be done on the output to the user.
    [SerializeField]
    string _meta = "";
    public string meta { get { return _meta; } }
    public void SetMeta(string newMeta) { _meta = newMeta; }

    //A description of this stat.  Could be used in the UI to tell the player what the stat is about.
    [SerializeField]
    string _description = "";
    public string description { get{ return _description; } }
    public void SetDescription(string newDesc) { _description = newDesc;  }
    
    
    public SE_GameStat(string ID)
    {
        _id = ID;
    }

}