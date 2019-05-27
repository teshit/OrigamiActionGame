using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    Rigidbody2D rb;
    Animator animator;

    public float jumpForce = 22f;       // ジャンプ時に加える力
    public float jumpThreshold = 1f;    // ジャンプ中か判定するための閾値
    public float runForce = 1.5f;       // 走りに加える力
    public float moveForceMultiplier;    // 移動速度の入力に対する追従度
    public GameObject bullet;

    bool isGround = true;        // 地面と接地しているか管理するフラグ
    int key = 0;                 // 左右の入力管理
    string state;                // プレイヤーの状態管理
    string prevState;            // 前の状態を保存
    float stateEffect = 1;       // 状態に応じて横移動速度を変えるための係数
    Vector3 mousePos;



    void Start()
    {
        this.rb = GetComponent<Rigidbody2D>();
        this.animator = GetComponent<Animator>();
        mousePos.z = 10.0f;
    }

    void Update()
    {
        GetInputKey();          // ① 入力を取得
        ChangeState();          // ② 状態を変更する
        ChangeAnimation();      // ③ 状態に応じてアニメーションを変更する
        Move();                 // ④ 入力に応じて移動する
    }

    void GetInputKey()
    {
        key = 0;
        if (Input.GetKey("right") || Input.GetKey("d"))
            key = 1;
        else if (Input.GetKey("left") || Input.GetKey("a"))
            key = -1;
        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(bullet, this.transform.position, Quaternion.identity);
        }
    }

    void ChangeState()
    {
        // 空中にいるかどうかの判定。上下の速度(rigidbody.velocity)が一定の値を超えている場合、空中とみなす
        if (Mathf.Abs(rb.velocity.y) > jumpThreshold)
        {
            isGround = false;
        }

        // 接地している場合
        if (isGround)
        {
            // 走行中
            if (key != 0)
            {
                state = "RUN";
                //待機状態
            }
            else
            {
                state = "IDLE";
            }
            // 空中にいる場合
        }
        else
        {
            // 上昇中
            if (rb.velocity.y > 0)
            {
                state = "JUMP";
                // 下降中
            }
            else if (rb.velocity.y < 0)
            {
                state = "FALL";
            }
        }
    }

    void ChangeAnimation()
    {
        // 状態が変わった場合のみアニメーションを変更する
        if (prevState != state)
        {
            switch (state)
            {
                case "JUMP":
                    animator.SetBool("isJump", true);
                    animator.SetBool("isFall", false);
                    animator.SetBool("isRun", false);
                    animator.SetBool("isIdle", false);
                    stateEffect = 0.5f;
                    break;
                case "FALL":
                    animator.SetBool("isFall", true);
                    animator.SetBool("isJump", false);
                    animator.SetBool("isRun", false);
                    animator.SetBool("isIdle", false);
                    stateEffect = 0.5f;
                    break;
                case "RUN":
                    animator.SetBool("isRun", true);
                    animator.SetBool("isFall", false);
                    animator.SetBool("isJump", false);
                    animator.SetBool("isIdle", false);
                    stateEffect = 1f;
                    //GetComponent<SpriteRenderer> ().flipX = true;
                    //transform.localScale = new Vector3(key, 1, 1); // 向きに応じてキャラクターのspriteを反転
                    break;
                default:
                    animator.SetBool("isIdle", true);
                    animator.SetBool("isFall", false);
                    animator.SetBool("isRun", false);
                    animator.SetBool("isJump", false);
                    stateEffect = 1f;
                    break;
            }
            // 状態の変更を判定するために状態を保存しておく
            prevState = state;
        }

        mousePos = Input.mousePosition;
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(mousePos);
        if (this.transform.position.x > targetPos.x) transform.localScale = new Vector3(-1, 1, 1);
        else transform.localScale = new Vector3(1, 1, 1);
    }

    void Move()
    {
        // 設置している時にupキー押下でジャンプ
        if (isGround)
        {
            if (Input.GetKeyDown("up") || Input.GetKeyDown("w"))
            {
                this.rb.AddForce(transform.up * this.jumpForce);
                isGround = false;
            }
        }
        Vector2 moveVector = Vector2.zero;    // 移動速度の入力
        moveVector.x = runForce * key;
        rb.AddForce(new Vector3(moveForceMultiplier * (moveVector.x - rb.velocity.x), 0));
    }

    // 着地判定
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (!isGround)
                isGround = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (!isGround)
                isGround = true;
        }
    }
}
