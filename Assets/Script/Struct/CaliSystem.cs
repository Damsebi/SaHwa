using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Enemy;

public class CalliSystem : MonoBehaviour
{
    private List<char> paint = new List<char>();
    [SerializeField] private int maxPaintOver = 10; // �⺻ �ִ� �ѵ�
    [SerializeField] private char lastColor = ' '; // ������ ���� ���

    public float paintWhite { get; private set; }
    public float paintBlack { get; private set; }
    public float paintOver { get; private set; }

    private void Awake()
    {
        // ���� ������ ���� maxPaintOver�� ����
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            switch (enemy.enemyData)
            {
                case EnemyDataMelee meleeData:
                    maxPaintOver = 5;
                    break;
                case EnemyDataRange rangeData:
                    maxPaintOver = 10;
                    break;
                case EnemyDataHybird hybridData:
                    maxPaintOver = 20;
                    break;
                case EnemyDataBuffer bufferData:
                    maxPaintOver = 30;
                    break;
            }
        }
        /* ������
        Enemy enemy = GetComponent<Enemy>();
        if (enemy != null)
        {
            switch (enemy.enemyType)
            {
                case EnemyType.Normal:
                    maxPaintOver = 5;
                    break;
                case EnemyType.Elite:
                    maxPaintOver = 10;
                    break;
                case EnemyType.Epic:
                    maxPaintOver = 20;
                    break;
                case EnemyType.MiddleBoss:
                    maxPaintOver = 30;
                    break;
                case EnemyType.MainBoss:
                    maxPaintOver = 30;
                    break;
            }
        }
         */

    }

    public void Painting(char color, float value)
    {
        Debug.Log("���� �� : " + (paint.Count > 0 ? paint[paint.Count - 1].ToString() : "����") + " / ���� �� : " + color);

        // ���� ���� ���� ��� paint�� �߰��� ��
        if (paint.Count > 0 && paint[paint.Count - 1] == color)
        {
            for (var i = 0; i < value; i++)
            {
                paint.Add(color);
            }
        }
        else
        {
            // �ٸ� ���� ���� ��� ��ĥ ������ �����
            float previousValue = paint.Count > 0 ? paint[paint.Count - 1] : 0;

            // ���� ���� �ٸ� ���� �������� ���� ���� paintOver ����
            if (paint.Count > 0 && lastColor != ' ' && lastColor != color)
            {
                paintOver += value;
                paintOver = Mathf.Min(paintOver, maxPaintOver);
            }

            for (var i = 0; i < value; i++)
            {
                paint.Add(color);
                if (paint.Count > maxPaintOver)
                {
                    paint.RemoveAt(0); // ťó�� FIFO ������� ����
                }
            }
        }

        lastColor = color;
        UpdatePaint();
    }

    private void UpdatePaint()
    {
        paintWhite = 0;
        paintBlack = 0;

        foreach (var type in paint)
        {
            if (type == 'W')
            {
                paintWhite++;
            }
            if (type == 'B')
            {
                paintBlack++;
            }
        }

        Debug.Log("paintOver" + paintOver);
    }

    public float StackRatio() // (���� ���� / �ִ� ����)
    {
        return (float)paintOver / maxPaintOver;
    }

    public void ResetPaint()
    {
        paint.Clear();
        paintOver = 0;
        lastColor = ' ';
    }

    public bool IsPaintOverMax()
    {
        return paintOver >= maxPaintOver;
    }
}
