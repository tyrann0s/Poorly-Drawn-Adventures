using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class TestSuite
{
    [UnityTest]
    public IEnumerator TestMakingDamage()
    {
        GameObject mob1GO = MonoBehaviour.Instantiate
            ((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Mobs/Goblin.prefab", typeof(GameObject)));
        GameObject mob2GO = MonoBehaviour.Instantiate
            ((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Mobs/Skeleton.prefab", typeof(GameObject)));

        Mob mob1 = mob1GO.GetComponent<Mob>();
        Mob mob2 = mob2GO.GetComponent<Mob>();

        yield return new WaitForSeconds(1);

        mob2.GetDamage(mob1.MobData.Attack1Damage);

        Assert.That(mob2.MobHP, Is.EqualTo(mob2.MobData.MaxHP - mob1.MobData.Attack1Damage));

        GameObject.Destroy(mob1GO);
        GameObject.Destroy(mob2GO);
        GameObject.Destroy(mob1);
        GameObject.Destroy(mob1);
    }

    [Test]
    public void TestMobDeath()
    {
        GameObject mob1GO = MonoBehaviour.Instantiate
            ((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Mobs/Goblin.prefab", typeof(GameObject)));
        GameObject mob2GO = MonoBehaviour.Instantiate
            ((GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Mobs/Skeleton.prefab", typeof(GameObject)));

        Mob mob1 = mob1GO.GetComponent<Mob>();
        Mob mob2 = mob2GO.GetComponent<Mob>();

        mob2.GetDamage(1000);

        Assert.That(mob2.IsDead);

        GameObject.Destroy(mob1GO);
        GameObject.Destroy(mob2GO);
        GameObject.Destroy(mob1);
        GameObject.Destroy(mob1);
    }
}
