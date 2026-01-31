using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 仅修改类名：PlayerInput → PlayerInputui（最小改动）
public class PlayerInputui : MonoBehaviour
{
    [Header("===   角色移动按键   ===")]
    public KeyCode KeyUp = KeyCode.W;
    public KeyCode KeyDown = KeyCode.S;
    public KeyCode KeyLeft = KeyCode.A;
    public KeyCode KeyRight = KeyCode.D;
    public KeyCode KeyA;

    [Header("===   摄像机控制按键   ===")]
    public KeyCode KeyJup;
    public KeyCode KeyJdown;
    public KeyCode KeyJleft;
    public KeyCode KeyJright;

    [Header("===  输入信号  ===")]
    public float Dup;
    public float Dturn;
    public float TargetDup;
    public float TargetDturn;
    public float VelocityDup;
    public float VelocityDturn;
    public bool InputEnable = true;

    // 摄像机输入值
    public float Jup;
    public float Jright;

    // 跑步信号
    public bool run;

    // 移动向量相关
    public float dL;
    public Vector3 dV;

    void Update()
    {
        // ================== 摄像机输入 ==================
        Jup = ((Input.GetKey(KeyJup) ? 1.0f : 0) - (Input.GetKey(KeyJdown) ? 1.0f : 0));
        Jright = ((Input.GetKey(KeyJright) ? 1.0f : 0) - (Input.GetKey(KeyJleft) ? 1.0f : 0));

        // ================== 角色移动输入 ==================
        TargetDup = ((Input.GetKey(KeyUp) ? 1.0f : 0) - (Input.GetKey(KeyDown) ? 1.0f : 0));
        TargetDturn = ((Input.GetKey(KeyRight) ? 1.0f : 0) - (Input.GetKey(KeyLeft) ? 1.0f : 0));

        if (InputEnable == false)
        {
            TargetDturn = 0;
            TargetDup = 0;
        }

        Dup = Mathf.SmoothDamp(Dup, TargetDup, ref VelocityDup, 0.1f);
        Dturn = Mathf.SmoothDamp(Dturn, TargetDturn, ref VelocityDturn, 0.1f);

        Vector2 TempVc = SqureToCircle(new Vector2(Dturn, Dup));
        float Dturn2 = TempVc.x;
        float Dup2 = TempVc.y;

        dL = Mathf.Sqrt((Dup2 * Dup2) + (Dturn2 * Dturn2));
        dV = Dup2 * Vector3.forward + Dturn2 * Vector3.right;

        run = Input.GetKey(KeyA); // 保持你原代码的跑步键（KeyA）
    }

    public Vector2 SqureToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - input.y * input.y / 2);
        output.y = input.y * Mathf.Sqrt(1 - input.x * input.x / 2);
        return output;
    }
}