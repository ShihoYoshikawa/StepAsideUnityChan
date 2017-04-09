using UnityEngine;
using System.Collections;

/// <summary>
/// アイテムを破棄するプログラム。
/// カメラの外に出たアイテムは直ちに破棄する。
/// </summary>
public class ItemDestroyer : MonoBehaviour {
    //カメラのオブジェクト
    private GameObject mainCamera;
    //カメラとItemDestroyerの距離
    private float difference;

    /// <summary>
    /// スタート時に呼び出される処理。
    /// カメラオブジェクトの取得と、初期状態でのカメラとの距離の計算を行う。
    /// </summary>
    void Start()
    {
        //カメラのオブジェクトを取得
        this.mainCamera = GameObject.Find("Main Camera");
        //カメラとItemDestroyerの位置（z座標）の差を求める
        this.difference = mainCamera.transform.position.z - this.transform.position.z;

    }

    /// <summary>
    /// アップデート時に呼び出される処理。
    /// カメラとの距離が初期状態と変わらないように、移動を行うことで、
    /// ItemDestroyerが常にカメラの背後に追尾するよにする。
    /// </summary>
    void Update()
    {
        //Unityちゃんの位置に合わせてItemDestroyerの位置を移動
        this.transform.position = new Vector3(0, this.transform.position.y, this.mainCamera.transform.position.z - difference);
    }

    /// <summary>
    /// 車、コーン、コインに衝突した場合は、衝突したオブジェクトの破棄を行う。
    /// </summary>
    /// <param name="other">衝突したオブジェクトのCollider</param>
    void OnTriggerEnter(Collider other)
    {
        //車、コーン、コインに衝突した場合
        if (other.gameObject.tag == "CarTag" ||
            other.gameObject.tag == "TrafficConeTag"||other.gameObject.tag == "CoinTag")
        {
            //接触したオブジェクトを破棄
            Destroy(other.gameObject);
        }
    }
}
