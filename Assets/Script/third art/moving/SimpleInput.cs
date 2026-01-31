using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleInput : MonoBehaviour
{
    [Header("===   角色移动按键   ===")]
    public KeyCode KeyUp = KeyCode.W;
    public KeyCode KeyDown = KeyCode.S;
    public KeyCode KeyLeft = KeyCode.A;
    public KeyCode KeyRight = KeyCode.D;
    public KeyCode KeyRun = KeyCode.LeftShift;

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

    public float Jup;
    public float Jright;
    public bool run;
    public float dL;
    public Vector3 dV;

    // ========== 仅新增这1行（无其他改动） ==========
    public bool attack; // 攻击/破坏物体的输入信号

    void Update()
    {
        // 摄像机输入（原有逻辑，无改动）
        Jup = ((Input.GetKey(KeyJup) ? 1.0f : 0) - (Input.GetKey(KeyJdown) ? 1.0f : 0));
        Jright = ((Input.GetKey(KeyJright) ? 1.0f : 0) - (Input.GetKey(KeyJleft) ? 1.0f : 0));

        // 角色移动输入（原有逻辑，无改动）
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

        run = Input.GetKey(KeyRun);

        // ========== 仅新增这3行（无其他改动） ==========
        // 鼠标左键触发攻击/破坏
        if (Input.GetMouseButtonDown(0))
        {
            attack = true;
        }
        else
        {
            attack = false;
        }
    }

    public Vector2 SqureToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - input.y * input.y / 2);
        output.y = input.y * Mathf.Sqrt(1 - input.x * input.x / 2);
        return output;
    }
}