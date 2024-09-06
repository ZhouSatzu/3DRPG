using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 定义一个静态类，扩展方法必须定义在静态类中
public static class ExtensionMethod
{
    private const float dotThreshold = 0.5f;

    // 这是一个扩展方法，用于判断当前物体是否正面朝向目标物体
    // 扩展方法的第一个参数前使用 "this" 关键字，表示这是对 Transform 类的扩展
    public static bool IsFacingTarget(this Transform transform, Transform target)
    {
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();

        // 计算当前物体的前方向与目标方向的点乘
        // 点乘结果表示两个向量之间的夹角的余弦值，结果在 -1 到 1 之间
        // 若点乘值接近 1，则表示两个向量接近平行（即物体面向目标）
        float dot = Vector3.Dot(vectorToTarget, transform.forward.normalized);

        // 如果点乘值大于等于定义的阈值，则认为物体正在面向目标
        return dot >= dotThreshold;
    }

    /*
    扩展方法简介：
    扩展方法是 C# 的一个强大特性，允许你向现有的类型添加新方法，而无需修改原有类型的代码。
    它的作用类似于给某个类“附加”功能，而不必继承或修改该类。
    
    在这个例子中，IsFacingTarget 是一个扩展方法，扩展了 Unity 引擎中的 Transform 类。
    通过这个扩展方法，你可以轻松判断当前物体是否面向另一个目标物体，而不必每次手动编写这些计算逻辑。
    
    扩展方法的语法要求：
    1. 方法必须定义在静态类中。
    2. 第一个参数前必须加上 "this" 关键字，并指明扩展的是哪个类型（这里是 Transform）。
    
    调用方式：
    通过扩展方法后，你可以像调用 Transform 类本身的方法一样调用它：
    bool isFacing = transform.IsFacingTarget(target);

    这种方式使得代码更具可读性和可维护性，避免了重复代码的出现。
    */
}
