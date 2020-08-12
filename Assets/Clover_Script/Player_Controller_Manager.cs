using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Controller_Manager : MonoBehaviour
{
    // 入力感度
    public float sensitiviController = 0.01f;

    // キャラクターのローカル正面ベクトル
    public Vector3 frontVec = new Vector3(0, 1, 0);


    // キャラクターコントローラ（カプセルコライダ）の参照
    private CapsuleCollider col;
    private Rigidbody rb;
    // キャラクターコントローラ（カプセルコライダ）の移動量
    private Vector3 velocity;

    // アニメーター各ステートへの参照
    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int locoState = Animator.StringToHash("Base Layer.Locomotion");
    static int jumpState = Animator.StringToHash("Base Layer.Jump");
    static int restState = Animator.StringToHash("Base Layer.Rest");
    static int attackState = Animator.StringToHash("Base Layer.Attack1");
    

    //外部から値が変わらないようにPrivateで定義
    private CharacterController characterController;
    private Animator animator;
    private Vector3 moveDirection = Vector3.zero;

    private AnimatorStateInfo currentBaseState;         // base layerで使われる、アニメーターの現在の状態の参照

    // ジャンプ威力
    public float jumpPower = 3.0f;
    // 前進速度
    public float forwardSpeed = 7.0f;
    // 後退速度
    public float backwardSpeed = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //前進処理
        this.SetStickVectorToCharacter();


        //transform.Rotate(0, acos, 0);

        velocity = transform.TransformDirection(velocity); // ローカルベクトルをワールドベクトルに変換
        velocity *= forwardSpeed;       // 移動速度を掛ける

        // 上下のキー入力でキャラクターを移動させる
        transform.localPosition += velocity * Time.fixedDeltaTime;


        currentBaseState = animator.GetCurrentAnimatorStateInfo(0); // 参照用のステート変数にBase Layer (0)の現在のステートを設定する
        if (Input.GetButtonDown("Jump"))
        {   // スペースキーを入力したら

            //アニメーションのステートがLocomotionの最中のみジャンプできる
            if (currentBaseState.nameHash == locoState)
            {
                //ステート遷移中でなかったらジャンプできる
                if (!animator.IsInTransition(0))
                {
                    rb.AddForce(Vector3.up * jumpPower, ForceMode.VelocityChange);
                    animator.SetBool("Jump", true);     // Animatorにジャンプに切り替えるフラグを送る
                }
            }
        }

        if (Input.GetButtonDown("Action"))
        {
            Debug.Log("攻撃");
            animator.SetBool("Attack1", true);
        }
    }

    // スティックを倒した方向にキャラクターを回転させる
    public void SetStickVectorToCharacter()
    {
        float h = Input.GetAxis("Horizontal");              // 入力デバイスの水平軸をhで定義
        float v = Input.GetAxis("Vertical");                // 入力デバイスの垂直軸をvで定義

        velocity = new Vector3(h, 0, v);        // 上下のキー入力からZ軸方向の移動量を取得

        if (Mathf.Abs(h) >= sensitiviController || Mathf.Abs(v) >= sensitiviController)
        {
            transform.localRotation = Quaternion.LookRotation(velocity);
        }

        SetAnimationForMove(v, h);
    }

    public void SetAnimationForMove(float v, float h)
    { 
        if 
        animator.SetFloat("Speed", v);                          // Animator側で設定している"Speed"パラメタにvを渡す
        //animator.SetFloat("Direction", h);                      // Animator側で設定している"Direction"パラメタにhを渡す
    }
}
