using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XMLController : MonoBehaviour
{
    public static XMLController Instance = null;

    public bool isLevelLoaded = false;
    public bool isLetterLoaded = false;
    public bool isDressingLoaded = false;


    Level[] _levels;


    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SingletonLoader.CheckSingleton();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void init()
    {
        if (!isLevelLoaded)
        {
            StartCoroutine(XMLTool.LoadLevelsXML());
        }
        if (!isLetterLoaded)
        {
            StartCoroutine(XMLTool.LoadLettersXML());
        }
        if (!isDressingLoaded)
        {
            StartCoroutine(XMLTool.LoadDressingXML());
        }
    }


    private Level[] Levels 
    {
        get 
        {
            if (_levels == null)
            {
                _levels = new Level[GameAssets.Instance.NumOfLevels];
                for(int i = 0; i < _levels.Length; i++)
                {
                    _levels[i] = ScriptableObject.CreateInstance<Level>();  //Manually create instances of scriptable object in addition to making the array
                }
            }
            return _levels;
        }
    }


    public Level[] getLevelList()
    {
        return Levels;
    }

    public Level getLevel(int levelId)
    {
        Level lvl = null;

        if (Levels.Length > levelId)
        {
            if (Levels[levelId] == null)
            {
                Levels[levelId] = XMLTool.LoadLevelXML(levelId);
            }
            lvl = Levels[levelId];
        }
        else
        {
            lvl = null;
        }
        if (lvl == null || lvl.Segments == null)
        {
            lvl = generateRandomLevel(levelId);
        }
        return lvl;
    }


    Level generateRandomLevel(int levelId)
    {
        List<string> usedSegment = new List<string>();

        var values = Enum.GetValues(typeof(MonsterInputType));

        MonsterInputType randomType = (MonsterInputType)(values.GetValue(UnityEngine.Random.Range(0, values.Length)));

        Level level = new Level();
        level.levelId = levelId;
        level.lettersGroup = 1;
        level.CollectableMonster = null;
        level.monsterInputType = randomType;
        level.hideCallout = 1;
        level.shuffleSegment = false;
        level.SegmentTime = 13f;

        level.Segments = new Segment[5];

        Level[] filtedLevels = getFilteredLevels(randomType);

        int i = 0;
        while (i < 5)
        {
            Level l = filtedLevels[UnityEngine.Random.Range(0, filtedLevels.Length)];
            if (l != null)
            {
                int sId = UnityEngine.Random.Range(0, l.Segments.Length);

                string sKey = l.levelId + "_" + sId.ToString();
                if (!usedSegment.Contains(sKey))
                {
                    Segment s = l.Segments[sId];
                    if (s != null)
                    {
                        usedSegment.Add(sKey);

                        level.Segments[i] = s;
                        level.StoneType = l.StoneType;
                        i++;
                    }
                }
            }
        }
        level.LoadTemplete(GameAssets.Instance.DefaultLevelTempletes);

        return level;
    }

    Level[] getFilteredLevels(MonsterInputType type)
    {
        List<Level> filtedLevels = new List<Level>();
        foreach (Level lvl in Levels)
        {
            if (
                lvl.Segments != null
                &&
                lvl.monsterInputType == type
                &&
                lvl.lettersGroup <= UsersController.Instance.userData().LastLetterGroup
            )
            {
                filtedLevels.Add(lvl);
            }
        }
        return filtedLevels.ToArray();
    }
}
