using UnityEngine;
using System.Collections;

public class ParticleEffect: MonoBehaviour
{
    public SkinnedMeshRenderer skin;
    Mesh baked;
    ParticleSystem particle;
    ParticleSystemRenderer render;
    public bool emit;
    public float coolDown = 0.5f;
    float interval = 0;

    // Use this for initialization
    void Start()
    {
        //元となるメッシュが指定されていなければ本スクリプトを停止
        if (!skin)
            this.enabled = false;
        //使用するパーティクルシステムへのアクセス
        particle = GetComponent<ParticleSystem>();
        render = GetComponent<ParticleSystemRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        //emitが[true]なら...
        if (emit)
        {
            //待機時間の計算
            interval -= Time.deltaTime;
            //必要分の待機時間が経過したなら...
            if (interval < 0)
            {
                //シーンに新たなGameObjectを再生（本GameObjectのコピー）
                GameObject newEmitter = Instantiate(gameObject, transform.position, transform.rotation) as GameObject;
                //再生したGameObjectにEmitMesh()を指示
                newEmitter.GetComponent<ParticleEffect>().EmitMesh();
                //待機時間のリセット
                interval = coolDown;
            }
            //emitが[false]なら...
        }
        else
        {
            //待機時間のリセット
            interval = coolDown;
        }
    }

    //外部からのアクセスでパーティクルの再生をさせる
    public void EmitMesh()
    {
        //Update ()させない為に[false]
        emit = false;
        //メッシュの型をリセット
        baked = new Mesh();
        //使用するメッシュの現在の形を保存
        skin.BakeMesh(baked);
        //自身のパーティクルシステムへのアクセス
        particle = GetComponent<ParticleSystem>();
        render = GetComponent<ParticleSystemRenderer>();
        //パーティクルシステムで使用するメッシュを指定
        render.mesh = baked;
        //パーティクルの再生
        particle.Play();
        //本GameObjectの破棄の指示
        Destroy(gameObject, particle.main.duration);
    }
}
