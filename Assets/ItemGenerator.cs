using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemGenerator : MonoBehaviour
{
    //carPrefabを入れる
    public GameObject carPrefab;
    //coinPrefabを入れる
    public GameObject coinPrefab;
    //cornPrefabを入れる
    public GameObject conePrefab;
    //スタート地点
    private int startPos = -160;
    //ゴール地点
    private int goalPos = 120;
    //アイテムを出すx方向の範囲
    private float posRange = 3.4f;

    //Unityちゃんのオブジェクト
    private GameObject unitychan;
    //生成済みのアイテムのPositionZの値のリスト
    private List<int> itemPositionZList = new List<int>();
    //アイテムの密度
    private int itemDensity = 15;
    //アイテム生成開始位置
    private int startItemGeneratePositionZ = 40;
    //アイテム生成終了位置
    private int endItemGeneratePositionZ = 50;

    /// <summary>
    /// スタート時に呼び出される処理。
    /// Unityオブジェクトを取得しておく。
    /// </summary>
    void Start()
    {
        //Unityちゃんのオブジェクトを取得
        this.unitychan = GameObject.Find("unitychan");
    }

    /// <summary>
    /// アップデート時に呼び出される処理。
    /// Unityちゃんの前方にアイテムを動的に生成する。
    /// </summary>
    void Update()
    {
        float unitychanPositionZ = this.unitychan.transform.position.z;

        //Unityちゃんの位置から、アイテム生成を開始する位置を計算する
        int firstCreateItemPositionZ = (int)(Mathf.Floor(unitychanPositionZ)) + startItemGeneratePositionZ;
        //Unityちゃんの位置から、アイテム生成を終了する位置を計算する
        int lastCreateItemPositonZ = (int)(Mathf.Floor(unitychanPositionZ)) + endItemGeneratePositionZ;

        for (int i = firstCreateItemPositionZ; i <= lastCreateItemPositonZ; i++)
        {
            if (IsToCreateItem(i))
            {
                CreateItem(i);
                //アイテムの重複生成を行わない為、既に生成を行ったZ座標の記録を行う。
                itemPositionZList.Add(i);
            }
        }
    }

    /// <summary>
    /// 指定されたZ座標の位置にアイテムをランダムで生成する。
    /// </summary>
    /// <param name="itemPosZ">アイテムを生成したい箇所のZ座標</param>
    void CreateItem(int itemPosZ)
    {
        //どのアイテムを出すのかをランダムに設定
        int num = Random.Range(0, 10);
        if (num <= 1)
        {
            //コーンをx軸方向に一直線に生成
            for (float j = -1; j <= 1; j += 0.4f)
            {
                GameObject cone = Instantiate(conePrefab) as GameObject;
                cone.transform.position = new Vector3(4 * j, cone.transform.position.y, itemPosZ);
            }
        }
        else
        {

            //レーンごとにアイテムを生成
            for (int j = -1; j < 2; j++)
            {
                //アイテムの種類を決める
                int item = Random.Range(1, 11);
                //アイテムを置くZ座標のオフセットをランダムに設定
                int offsetZ = Random.Range(-5, 6);
                //60%コイン配置:30%車配置:10%何もなし
                if (1 <= item && item <= 6)
                {
                    //コインを生成
                    GameObject coin = Instantiate(coinPrefab) as GameObject;
                    coin.transform.position = new Vector3(posRange * j, coin.transform.position.y, itemPosZ + offsetZ);
                }
                else if (7 <= item && item <= 9)
                {
                    //車を生成
                    GameObject car = Instantiate(carPrefab) as GameObject;
                    car.transform.position = new Vector3(posRange * j, car.transform.position.y, itemPosZ + offsetZ);
                }
            }
        }
    }

    /// <summary>
    /// 指定されたZ座標上にアイテムを作る必要があるかどうかを判定する。
    /// </summary>
    /// <param name="posZ">アイテムの生成を検討しているZ座標</param>
    /// <returns>アイテムを生成する必要があるかどうか。</returns>
    bool IsToCreateItem(int posZ)
    {
        if (posZ < this.startPos)
        {
            return false;
        }
        if (posZ >= this.goalPos)
        {
            return false;
        }
        //アイテムの密度で割り切れない値の場合はアイテム生成を行わないことで、アイテムの密度を管理している。
        if (posZ % itemDensity != 0)
        {
            return false;
        }
        //既にアイテム生成済みのZ座標の場合は、アイテムの生成を行わない。 
        if (this.itemPositionZList.Contains(posZ))
        {
            return false;
        }
        return true;
    }
}