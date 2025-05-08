using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowC : MonoBehaviour
{

    private Transform player;

    private SpriteRenderer thisSprite;

    private SpriteRenderer playerSprite;

    private Color color;

    [Header("时间控制的参数")]
    public float activeTime;
    public float activeStart;

    [Header("不透明度控制")]
    public float alphaSet;
    public float alphaMultiplier;
    private float alpha;


    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();

        alpha = alphaSet;

        thisSprite.sprite = playerSprite.sprite;

        transform.position = player.position;
        transform.localScale = player.localScale;
        transform.rotation = player.rotation;

        activeStart = Time.time;
    }

    private void Update()
    {
        alpha *= alphaMultiplier;

        color = new Color(1f, 1f, 1, alpha);

        thisSprite.color = color;

        if (Time.time >= activeStart + activeTime)
        {
            GameObjcetPool.MainInstance.ReturnToPool(this.gameObject);
        }
    }


}
