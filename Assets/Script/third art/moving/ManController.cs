using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManController : MonoBehaviour
{
    public GameObject model;//抓取要控制的模型
    public simpleInput pi;//调用simpleInput脚本
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

    private float velocityForward;

    // ========== 无新增变量（仅复用原有变量） ==========

    void Awake()
    {
        // 原有逻辑，无改动
        anim = model.GetComponent<Animator>();
        pi = GetComponent<simpleInput>();
        rigid = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // 原有动画逻辑，无改动
        RunTurn = ((pi.run) ? 2.0f : 1.0f);
        anim.SetFloat("forward", pi.dL * Mathf.Lerp(anim.GetFloat("forward"), RunTurn, 0.3f));

        // 原有转向逻辑，无改动
        if (pi.dL > 0.1f)
        {
            CharacterTurn = Vector3.Slerp(model.transform.forward, pi.dV, 0.1f);
            model.transform.forward = CharacterTurn;
        }

        // 原有移动向量逻辑，无改动
        if (PlanLock == false)
        {
            planVc = pi.dL * model.transform.forward * movingSpeed * ((pi.run) ? RunMultiplier : 1.0f);
        }

        // ========== 仅新增这一段（破坏物体逻辑，无其他改动） ==========
        // 检测攻击输入，触发物体破坏
        if (pi.attack)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                BreakableObject breakable = hit.collider.GetComponent<BreakableObject>();
                if (breakable != null)
                {
                    breakable.BreakObject();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        // 原有移动逻辑，无改动
        rigid.velocity = new Vector3(planVc.x, rigid.velocity.y, planVc.z);
    }

    // 原有动画检测逻辑，无改动
    public bool CheckState(string stateName, string layerName = "Base Layer")
    {
        int layerIndex = anim.GetLayerIndex(layerName);
        bool result = anim.GetCurrentAnimatorStateInfo(layerIndex).IsName(stateName);
        return result;
    }
}