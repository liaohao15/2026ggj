using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;//抓取要控制的模型
    public PlayerInput pi;//调用PlayerInput脚本
    [SerializeField]
    private Animator anim;//获取组件Animator
    [SerializeField]
    private Rigidbody rigid;//获取刚体

    public float movingSpeed = 1.0f; //基础速度
    private float RunMultiplier = 2.0f;//当跑步键按下时，乘以这个速度倍率

    private Vector3 CharacterTurn;//为角色转向而设计的变量
    private float RunTurn;//为动画切换而设计的变量
    private Vector3 planVc;//角色移动的最终量
    private bool PlanLock;

    // 【仅新增这1行】用于SmoothDamp的速度缓存（解决卡顿核心）
    private float velocityForward;

    void Awake()
    {
        anim = model.GetComponent<Animator>();
        pi = GetComponent<PlayerInput>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        //1.动画转换缓冲
        RunTurn = ((pi.run) ? 2.0f : 1.0f);

        anim.SetFloat("forward", pi.dL * Mathf.Lerp(anim.GetFloat("forward"), RunTurn, 0.3f));//Mathf.Lerp(线性插值)让动画参数"forward"在走路和跑步之间平滑过渡的，实际上是由1增加到2;

        //2.人物转向缓冲
        if (pi.dL > 0.1f)
        {
            CharacterTurn = Vector3.Slerp(model.transform.forward, pi.dV, 0.1f);
            model.transform.forward = CharacterTurn;
        }

        //3.移动向量计算
        if (PlanLock == false)
        {
            planVc = pi.dL * model.transform.forward * movingSpeed * ((pi.run) ? RunMultiplier : 1.0f);
        }
    }

    private void FixedUpdate()
    {
        rigid.velocity = new Vector3(planVc.x, rigid.velocity.y, planVc.z) ;
    }

    //      ======   用来检测Animator的层级   ======
    public bool CheckState(string stateName, string layerName = "Base Layer")//（传进来的名字，名字是不是Base layer）
    {
        int layerIndex = anim.GetLayerIndex(layerName);
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        return result;
    }
}