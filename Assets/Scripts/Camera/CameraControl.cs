using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    //观察目标
    public Transform Target;
    //观察距离
    public float Distance = 500F;
    //旋转速度
    private float SpeedX = 240;
    private float SpeedY = 120;
    //角度限制
    private float MinLimitY = 5;
    private float MaxLimitY = 180;

    //旋转角度
    private float mX = 0.0F;
    private float mY = 0.0F;

    //鼠标缩放距离最值
    private float MaxDistance = 100;
    private float MinDistance = 15F;
    //鼠标缩放速率
    private float ZoomSpeed = 2F;

    //是否启用差值
    public bool isNeedDamping = true;
    //速度
    public float Damping = 10F;

    private Quaternion mRotation = Quaternion.identity;

    void Start()
    {
        //初始化旋转角度
        // mX = transform.eulerAngles.x;
        // mY = transform.eulerAngles.y;
        //transform.position = new Vector3(-12.2319355f, 31.3339634f, 21.7906647f);
        
        mRotation = Quaternion.Euler(mY, mX, 0);
        //根据是否插值采取不同的角度计算方式
        if (isNeedDamping)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, mRotation, Time.deltaTime * Damping);
        }
        else
        {
            transform.rotation = mRotation;
        }
        
        Vector3 mPosition = mRotation * new Vector3(0.0F, 0.0F, -Distance) + Target.position;

        //设置相机的角度和位置
        if (isNeedDamping)
        {
            transform.position = Vector3.Lerp(transform.position, mPosition, Time.deltaTime * Damping);
        }
        else
        {
            transform.position = mPosition;
        }
    }

    void LateUpdate()
    {
        //鼠标右键旋转
        if (Target != null && Input.GetMouseButton(1))
        {
            //获取鼠标输入
            mX += Input.GetAxis("Mouse X") * SpeedX * 0.02F;
            mY -= Input.GetAxis("Mouse Y") * SpeedY * 0.02F;
            //范围限制
            mY = ClampAngle(mY, MinLimitY, MaxLimitY);
            //计算旋转
            //我们可以通过Quaternion.Euler()方法将一个Vector3类型的值转化为一个四元数
            //，进而通过修改Transform.Rotation来实现相同的目的
            mRotation = Quaternion.Euler(mY, mX, 0);
            //根据是否插值采取不同的角度计算方式
            if (isNeedDamping)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, mRotation, Time.deltaTime * Damping);
            }
            else
            {
                transform.rotation = mRotation;
            }
        }

        //鼠标滚轮缩放
        Distance -= Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        Distance = Mathf.Clamp(Distance, MinDistance, MaxDistance);

        //重新计算位置
        Vector3 mPosition = mRotation * new Vector3(0.0F, 0.0F, -Distance) + Target.position;

        //设置相机的角度和位置
        if (isNeedDamping)
        {
            transform.position = Vector3.Lerp(transform.position, mPosition, Time.deltaTime * Damping);
        }
        else
        {
            transform.position = mPosition;
        }

    }


    //角度限制
    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
}
