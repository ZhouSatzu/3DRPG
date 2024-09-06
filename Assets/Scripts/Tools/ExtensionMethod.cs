using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ����һ����̬�࣬��չ�������붨���ھ�̬����
public static class ExtensionMethod
{
    private const float dotThreshold = 0.5f;

    // ����һ����չ�����������жϵ�ǰ�����Ƿ����泯��Ŀ������
    // ��չ�����ĵ�һ������ǰʹ�� "this" �ؼ��֣���ʾ���Ƕ� Transform �����չ
    public static bool IsFacingTarget(this Transform transform, Transform target)
    {
        var vectorToTarget = target.position - transform.position;
        vectorToTarget.Normalize();

        // ���㵱ǰ�����ǰ������Ŀ�귽��ĵ��
        // ��˽����ʾ��������֮��ļнǵ�����ֵ������� -1 �� 1 ֮��
        // �����ֵ�ӽ� 1�����ʾ���������ӽ�ƽ�У�����������Ŀ�꣩
        float dot = Vector3.Dot(vectorToTarget, transform.forward.normalized);

        // ������ֵ���ڵ��ڶ������ֵ������Ϊ������������Ŀ��
        return dot >= dotThreshold;
    }

    /*
    ��չ������飺
    ��չ������ C# ��һ��ǿ�����ԣ������������е���������·������������޸�ԭ�����͵Ĵ��롣
    �������������ڸ�ĳ���ࡰ���ӡ����ܣ������ؼ̳л��޸ĸ��ࡣ
    
    ����������У�IsFacingTarget ��һ����չ��������չ�� Unity �����е� Transform �ࡣ
    ͨ�������չ����������������жϵ�ǰ�����Ƿ�������һ��Ŀ�����壬������ÿ���ֶ���д��Щ�����߼���
    
    ��չ�������﷨Ҫ��
    1. �������붨���ھ�̬���С�
    2. ��һ������ǰ������� "this" �ؼ��֣���ָ����չ�����ĸ����ͣ������� Transform����
    
    ���÷�ʽ��
    ͨ����չ���������������� Transform �౾��ķ���һ����������
    bool isFacing = transform.IsFacingTarget(target);

    ���ַ�ʽʹ�ô�����߿ɶ��ԺͿ�ά���ԣ��������ظ�����ĳ��֡�
    */
}
