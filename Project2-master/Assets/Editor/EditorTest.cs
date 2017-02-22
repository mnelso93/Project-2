using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorTest : EditorWindow
{
    public List<Enemies> enemyList = new List<Enemies>();
    public string enemyName;
    public int enemyHealth, enemyAtk, enemyDef, enemyAgi;
    public float enemyAtkSpeed;
    public bool enemyMagicUser = false;
    public int enemyMana;
    public Sprite enemySprite;
    private bool nameFlag = false;
    private bool spriteFlag = false;
    List<string> enemyNameList = new List<string>();
    string[] enemyNames;
    private int currentChoice = 0;
    int lastChoice;
    [MenuItem("Custom Tools/Enemy Editor %g")]
    private static void Enemy()
    {
        EditorWindow.GetWindow<EditorTest>();
    }

    void Awake()
    {
        GetEnemies();
    }

    private void OnGUI()
    {
        currentChoice = EditorGUILayout.Popup(currentChoice, enemyNames);

        enemySprite = (Sprite)EditorGUILayout.ObjectField("Sprite: ", enemySprite, typeof(Sprite), true);
        enemyName = EditorGUILayout.TextField("Name: ", enemyName);
        enemyHealth = EditorGUILayout.IntSlider("Health: ", enemyHealth, 1, 300);
        enemyAtk = EditorGUILayout.IntSlider("Attack: ", enemyAtk, 1, 100);
        enemyDef = EditorGUILayout.IntSlider("Defense: ", enemyDef, 1, 100);
        enemyAgi = EditorGUILayout.IntSlider("Agility: ", enemyAgi, 1, 100);
        enemyAtkSpeed = EditorGUILayout.Slider("Attack Speed: ", enemyAtkSpeed, 1, 20);
        enemyMagicUser = EditorGUILayout.Toggle("Magic User: ", enemyMagicUser);
        if(enemyMagicUser)
        {
            enemyMana = EditorGUILayout.IntSlider("Mana Pool: ", enemyMana, 1, 100);
        }

        if (nameFlag)
        {
            EditorGUILayout.HelpBox("Name can not be blank", MessageType.Error);
        }

        if (spriteFlag)
        {
            EditorGUILayout.HelpBox("Sprite can not be empty", MessageType.Error);
        }

        if (currentChoice == 0)
        {
            if (GUILayout.Button("Create"))
            {
                createEnemy();
            }
        }

        if (currentChoice != 0)
        {
            if (GUILayout.Button("Save"))
            {
                alterEnemy();
            }
        }

        if (currentChoice != lastChoice)
        {
            if (currentChoice == 0)
            {
                // blank out fields for new enemy
                enemySprite = null;
                enemyName = "";
                enemyHealth = 1;
                enemyAtk = 1;
                enemyDef = 1;
                enemyAgi = 1;
                enemyAtkSpeed = 1.0f;
                enemyMagicUser = false;
                enemyMana = 1;
            }
                  
            else
            {
                //load fields with enemy data
                enemySprite = enemyList[currentChoice - 1].mySprite;
                enemyName = enemyList[currentChoice - 1].emname;
                enemyHealth = enemyList[currentChoice - 1].health;
                enemyAtk = enemyList[currentChoice - 1].atk;
                enemyDef = enemyList[currentChoice - 1].def;
                enemyAgi = enemyList[currentChoice - 1].agi;
                enemyAtkSpeed = enemyList[currentChoice - 1].atkTime;
                enemyMagicUser = enemyList[currentChoice - 1].isMagic;
                enemyMana = enemyList[currentChoice - 1].manaPool;
            }
        }
            lastChoice = currentChoice;
    }

    private void GetEnemies()
    {
        enemyList.Clear();
        string[] guids = AssetDatabase.FindAssets("t:Enemies");
        foreach (string guid in guids)
        {
            string enemyAsset = AssetDatabase.GUIDToAssetPath(guid);
            Enemies enemyInst = AssetDatabase.LoadAssetAtPath(enemyAsset, typeof(Enemies)) as Enemies;
            enemyList.Add(enemyInst);
        }


        foreach (Enemies e in enemyList)
        {
            enemyNameList.Add(e.emname);
        }
        enemyNameList.Insert(0, "New");
        enemyNames = enemyNameList.ToArray();
    }

    private void createEnemy()
    {
        if (enemyName == "")
        {
            nameFlag = true;
        }

        else if (enemySprite == null)
        {
            spriteFlag = true;
        }

        else
        {
            Enemies newEnemy = ScriptableObject.CreateInstance<Enemies>();
            newEnemy.mySprite = enemySprite;
            newEnemy.emname = enemyName;
            newEnemy.health = enemyHealth;
            newEnemy.atk = enemyAtk;
            newEnemy.def = enemyDef;
            newEnemy.agi = enemyAgi;
            newEnemy.atkTime = enemyAtkSpeed;
            newEnemy.isMagic = enemyMagicUser;
            newEnemy.manaPool = enemyMana;
            AssetDatabase.CreateAsset(newEnemy, "Assets/Resources/Data/EnemyData/" + newEnemy.emname.Replace(" ", "_") + ".asset");
            nameFlag = false;
            spriteFlag = false;
            GetEnemies();
        }


    }

    private void alterEnemy()
    {
        if (enemyName == "")
        {
            nameFlag = true;
        }

        else if (enemySprite == null)
        {
            spriteFlag = true;
        }

        else
        {
			string path = AssetDatabase.GetAssetPath (currentChoice);
			Enemies updateEnemy = AssetDatabase.LoadAssetAtPath (path);
			updateEnemy.emname = enemyName;
			updateEnemy.health = enemyHealth;
			updateEnemy.atk = enemyAtk;
			updateEnemy.def = enemyDef;
			updateEnemy.agi = enemyAgi;
			updateEnemy.atkTime = enemyAtkSpeed;
			updateEnemy.isMagic = enemyMagicUser;
			updateEnemy.manaPool = enemyMana;
			updateEnemy.mySprite = enemySprite;
            nameFlag = false;
            spriteFlag = false;
            GetEnemies();
        }
    }

}
