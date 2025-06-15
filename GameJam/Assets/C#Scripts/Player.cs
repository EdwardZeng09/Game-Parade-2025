using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D PlayerRB;
    public SpriteRenderer SpriteRenderer;
    [SerializeField]public float PlayerMoveSpeed=7f;
    void Start()
    {
        PlayerRB = GetComponent<Rigidbody2D>();
        SpriteRenderer= GetComponent<SpriteRenderer>();

    }

    void Update()
    {
        Move();
    }
    void Move()
    {
        float Horizontal = Input.GetAxisRaw("Horizontal");//����unity�������ȡˮƽ�ƶ�����
        float Vertical = Input.GetAxisRaw("Vertical");

        PlayerRB.velocity = new Vector2(Horizontal * PlayerMoveSpeed, Vertical*PlayerMoveSpeed);//�˴�ֻ�Ǹ�Rigidbody2D����ṩ��һ���ٶȣ����Բ���Ҫ����ʱ��

        if (Horizontal > 0)
            SpriteRenderer.flipX = false;//����SpriteRenderer���ʵ����ת������Horizontal>0��Ϊ�����ߣ�false���Ǳ���Ĭ�Ϸ�����С���㣬���������ߣ�true����X����ߵ�
        if (Horizontal < 0)
            SpriteRenderer.flipX = true;





    }
}
