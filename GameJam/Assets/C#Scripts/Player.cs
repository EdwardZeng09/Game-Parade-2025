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
        float Horizontal = Input.GetAxisRaw("Horizontal");//利用unity中组件读取水平移动向量
        float Vertical = Input.GetAxisRaw("Vertical");

        PlayerRB.velocity = new Vector2(Horizontal * PlayerMoveSpeed, Vertical*PlayerMoveSpeed);//此处只是给Rigidbody2D组件提供了一个速度，所以不需要乘上时间

        if (Horizontal > 0)
            SpriteRenderer.flipX = false;//利用SpriteRenderer组件实现旋转，这里Horizontal>0意为向右走，false即是保持默认方向，若小于零，即是向左走，true即是X方向颠倒
        if (Horizontal < 0)
            SpriteRenderer.flipX = true;





    }
}
