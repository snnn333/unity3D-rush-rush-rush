using System;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class ThirdPersonCharacter : MonoBehaviour
{
    [SerializeField] float m_MovingTurnSpeed = 360; //移动中转向的速度
    [SerializeField] float m_StationaryTurnSpeed = 180; //站立时转向的速度
    [SerializeField] float m_JumpPower = 12f; //跳跃产生的力量
    [Range(1f, 4f)] [SerializeField] float m_GravityMultiplier = 2f; //重力乘子
    [SerializeField] float m_RunCycleLegOffset = 0.2f; //腿偏移值
    [SerializeField] float m_MoveSpeedMultiplier = 1f; //移动速度
    [SerializeField] float m_AnimSpeedMultiplier = 1f; //动画播放速度
    [SerializeField] float m_GroundCheckDistance = 0.1f; //地面检查距离
    [SerializeField] float m_AnimationForwardSpeed = 0.7f; //前进速度

    Rigidbody m_Rigidbody;
    Animator m_Animator;
    bool m_IsGrounded;
    float m_OrigGroundCheckDistance;
    const float k_Half = 0.5f;
    float m_TurnAmount;
    float m_ForwardAmount;
    Vector3 m_GroundNormal;
    float m_CapsuleHeight;
    Vector3 m_CapsuleCenter;
    CapsuleCollider m_Capsule;
    bool m_Crouching;
    float scale;
    float capsuleRadius;



    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;
        scale = transform.localScale.x;
        m_GroundCheckDistance = m_Capsule.radius * scale;


        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;
    }


    public void Move(Vector3 move, bool crouch, bool jump, float speed)
    {

        // 将一个世界坐标的输入转换为本地相关的转向和前进速度，需要考虑到角色头部的方向
        if (move.magnitude > 1f) move.Normalize();
        move = move * speed;
        Vector3 moveWorld = move;
        moveWorld.y = 0;
        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, m_GroundNormal); //根据地面的法向量，产生一个对应平面的速度方向.
        m_TurnAmount = Mathf.Atan2(move.x, move.z); //与z轴夹角，表示转向角度
        m_ForwardAmount = move.z * m_AnimationForwardSpeed;

        ApplyExtraTurnRotation();

        // 分地面和空中两种情况考虑
        if (m_IsGrounded)
        {
            HandleGroundedMovement(crouch, jump); //根据是否蹲伏确定是否跳跃，如果可以起跳添加向上的速度
        }
        else
        {
            HandleAirborneMovement(moveWorld); //处理下降速度，并且根据上升与下降设置不同的地面监测距离；空中跳跃和下蹲不起作用
        }

        ScaleCapsuleForCrouching(crouch); //根据蹲伏减少胶囊体高度，并判断能否站立
        PreventStandingInLowHeadroom(); //在不能起立的地方保持下蹲

        // 将所有参数交给动画实现
        //UpdateAnimator(move);
    }


    void ScaleCapsuleForCrouching(bool crouch)
    {
        //蹲下的一瞬间把胶囊高度和中心高度减半
        if (m_IsGrounded && crouch)
        {
            if (m_Crouching) return; //只能在蹲下的瞬间处理一次
            m_Capsule.height = m_Capsule.height / 2f;
            m_Capsule.center = m_Capsule.center / 2f;
            m_Crouching = true;
        }
        else
        {
            Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
            float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
            if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                //从角色底部向上丢一个球，k_Half是为了防止在丢的时候就碰到了地面，而做的向上偏移  
                m_Crouching = true; //碰到了就说明无法回到站立状态
                return;
            }
            m_Capsule.height = m_CapsuleHeight; //如果松开了c键，即起立，就立刻恢复到原始高度
            m_Capsule.center = m_CapsuleCenter;
            m_Crouching = false;
        }
    }

    void PreventStandingInLowHeadroom()
    {
        // 如果下蹲进入一个矮山洞，就不能起立
        if (!m_Crouching)
        {
            Ray crouchRay = new Ray(m_Rigidbody.position + Vector3.up * m_Capsule.radius * k_Half, Vector3.up);
            float crouchRayLength = m_CapsuleHeight - m_Capsule.radius * k_Half;
            if (Physics.SphereCast(crouchRay, m_Capsule.radius * k_Half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                m_Crouching = true;
            }
        }
    }


    void UpdateAnimator(Vector3 move)
    {
        //更新动画参数
        m_Animator.SetFloat("Forward", m_ForwardAmount, 0.1f, Time.deltaTime);
        m_Animator.SetFloat("Turn", m_TurnAmount, 0.1f, Time.deltaTime);
        m_Animator.SetBool("Crouch", m_Crouching);
        m_Animator.SetBool("OnGround", m_IsGrounded);
        if (!m_IsGrounded)
        {
            m_Animator.SetFloat("Jump", m_Rigidbody.velocity.y);
        }

        // 计算哪只脚是在后面的，所以可以判断跳跃动画中哪只脚先离开地面  
        // 这里的代码依赖于动画中特殊的跑步循环，假设某只脚会在未来的0到0.5秒内超越另一只脚  
        float runCycle =
            Mathf.Repeat(//获取当前是在哪个脚，Repeat相当于取模
                m_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + m_RunCycleLegOffset, 1);
        float jumpLeg = (runCycle < k_Half ? 1 : -1) * m_ForwardAmount;
        if (m_IsGrounded)
        {
            m_Animator.SetFloat("JumpLeg", jumpLeg);
        }

        // 这边的方法允许我们在inspector视图中调整动画的walking/running速率，他会因为根运动影响移动的速度  
        if (m_IsGrounded && move.magnitude > 0)
        {
            m_Animator.speed = m_AnimSpeedMultiplier;
        }
        else
        {
            // 在空中时不用
            m_Animator.speed = 1;
        }
    }


    void HandleAirborneMovement(Vector3 moveAir)
    {
        // 下跳时添加额外的重力
        Vector3 extraGravityForce = (Physics.gravity * m_GravityMultiplier) - Physics.gravity;
        m_Rigidbody.AddForce(extraGravityForce);
        moveAir.Normalize();
        //Debug.Log(moveAir);
        if (Mathf.Sqrt(m_Rigidbody.velocity.x * m_Rigidbody.velocity.x + m_Rigidbody.velocity.z * m_Rigidbody.velocity.z) < 2.0f)
            m_Rigidbody.AddForce(moveAir * 7.5f);

        m_GroundCheckDistance = m_Rigidbody.velocity.y < 0 ? m_OrigGroundCheckDistance : 0.01f; //上升的时候不判断是否在地面上
    }


    void HandleGroundedMovement(bool crouch, bool jump)
    {
        // 只有在地面且站立情况下才能起跳
        if (jump && !crouch && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            // 根据跳跃能量给人物向上的速度，x、z轴速度由人物当前速度决定
            m_Rigidbody.velocity = new Vector3(m_Rigidbody.velocity.x, m_JumpPower, m_Rigidbody.velocity.z);
            m_IsGrounded = false;
            m_Animator.applyRootMotion = false;
            m_GroundCheckDistance = 0.1f;
            //Debug.Log(m_Rigidbody.velocity.x+" "+m_Rigidbody.velocity.z+" "+Mathf.Sqrt(m_Rigidbody.velocity.x*m_Rigidbody.velocity.x+m_Rigidbody.velocity.z*m_Rigidbody.velocity.z)+" "+m_Rigidbody.velocity.magnitude);
        }
    }

    void ApplyExtraTurnRotation()
    {
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);//根据移动的速度计算出转向的速度;转向越大,转身速度增加得越慢
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);//人物转向
    }


    public void OnAnimatorMove()
    {
        // 使用这个方法来代替基础的移动，这个方法允许我们移除位置的速度
        if (m_IsGrounded && Time.deltaTime > 0)
        {
            Vector3 v = (m_Animator.deltaPosition * m_MoveSpeedMultiplier) / Time.deltaTime;

            // 保护y轴的移动速度 
            v.y = m_Rigidbody.velocity.y;
            m_Rigidbody.velocity = v;
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            m_Animator.SetInteger("motion", 1);
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            m_Animator.SetInteger("motion", -1);
        }
        else
        {
            m_Animator.SetInteger("motion", 0);
        }
    }


    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        float actualRadius = m_Capsule.radius * scale;
        float offset = actualRadius * 1.8f;
        bool forward, back, right, left;
        bool leftForward, rightForward, leftBack, rightBack;
        Vector3 forwardVec, backVec, rightVec, leftVec;
        Vector3 leftForwardVec, rightForwardVec, leftBackVec, rightBackVec;
        forwardVec = transform.position + (Vector3.up * actualRadius) + (transform.forward * offset);
        backVec = transform.position + (Vector3.up * actualRadius) - (transform.forward * offset);
        rightVec = transform.position + (Vector3.up * actualRadius) + (transform.right * offset);
        leftVec = transform.position + (Vector3.up * actualRadius) - (transform.right * offset);
        leftForwardVec = transform.position + (Vector3.up * actualRadius) + (transform.forward * offset * 0.72f) - (transform.right * offset * 0.72f);
        rightForwardVec = transform.position + (Vector3.up * actualRadius) + (transform.forward * offset * 0.72f) + (transform.right * offset * 0.72f);
        leftBackVec = transform.position + (Vector3.up * actualRadius) - (transform.forward * offset * 0.72f) - (transform.right * offset * 0.72f);
        rightBackVec = transform.position + (Vector3.up * actualRadius) - (transform.forward * offset * 0.72f) + (transform.right * offset * 0.72f);

        forward = Physics.Raycast(forwardVec, Vector3.down, out hitInfo, m_GroundCheckDistance + actualRadius);
        back = Physics.Raycast(backVec, Vector3.down, out hitInfo, m_GroundCheckDistance + actualRadius);
        right = Physics.Raycast(rightVec, Vector3.down, out hitInfo, m_GroundCheckDistance + actualRadius);
        left = Physics.Raycast(leftVec, Vector3.down, out hitInfo, m_GroundCheckDistance + actualRadius);
        leftForward = Physics.Raycast(leftForwardVec, Vector3.down, out hitInfo, m_GroundCheckDistance + actualRadius);
        rightForward = Physics.Raycast(rightForwardVec, Vector3.down, out hitInfo, m_GroundCheckDistance + actualRadius);
        leftBack = Physics.Raycast(leftBackVec, Vector3.down, out hitInfo, m_GroundCheckDistance + actualRadius);
        rightBack = Physics.Raycast(rightBackVec, Vector3.down, out hitInfo, m_GroundCheckDistance + actualRadius);

        //bool sphereCast;
        //Ray groundRay = new Ray(transform.position + (Vector3.up * actualRadius), Vector3.down);
        //float rayLength = actualRadius*1.5f;
        //sphereCast = Physics.SphereCast(groundRay, actualRadius, rayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore);
#if UNITY_EDITOR
        Debug.DrawLine(forwardVec, forwardVec + (Vector3.down * (m_GroundCheckDistance + actualRadius)));
        Debug.DrawLine(backVec, backVec + (Vector3.down * (m_GroundCheckDistance + actualRadius)));
        Debug.DrawLine(rightVec, rightVec + (Vector3.down * (m_GroundCheckDistance + actualRadius)));
        Debug.DrawLine(leftVec, leftVec + (Vector3.down * (m_GroundCheckDistance + actualRadius)));
        Debug.DrawLine(leftForwardVec, leftForwardVec + (Vector3.down * (m_GroundCheckDistance + actualRadius)));
        Debug.DrawLine(rightForwardVec, rightForwardVec + (Vector3.down * (m_GroundCheckDistance + actualRadius)));
        Debug.DrawLine(leftBackVec, leftBackVec + (Vector3.down * (m_GroundCheckDistance + actualRadius)));
        Debug.DrawLine(rightBackVec, rightBackVec + (Vector3.down * (m_GroundCheckDistance + actualRadius)));
#endif
        //八个方向都脱离才判断为坠落
        //if (sphereCast)
        if (forward || back || right || left || leftForward || rightForward || leftBack || rightBack)
        {
            m_GroundNormal = hitInfo.normal;
            m_IsGrounded = true;
            m_Animator.applyRootMotion = true;

        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
            m_Animator.applyRootMotion = false;
        }
    }
}

