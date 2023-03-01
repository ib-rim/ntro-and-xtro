using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PulsingPlatform : MonoBehaviour
{
    private BoxCollider2D boxCollider;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    public bool startsVisible;
    public float steps;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        if (startsVisible)
        {
            StartCoroutine(Disappear());
            StartCoroutine(FadeOut());
        }
        else
        {
            spriteRenderer.color -= new Color(1f, 1f, 1f, 1f);
            StartCoroutine(Appear());
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeOut()
    {
        for (int i = 1; i <= steps; i++)
        {
            yield return new WaitForSeconds(1f);
            spriteRenderer.color -= new Color(0, 0, 0, 1 / steps);
        }
        spriteRenderer.color = originalColor - new Color(1f, 1f, 1f, 1f);
    }

    IEnumerator FadeIn()
    {
        for (int i = 1; i <= steps; i++)
        {
            yield return new WaitForSeconds(1f);
            spriteRenderer.color += new Color(1 / steps, 1 / steps, 1 / steps, 1 / steps);
        }
        spriteRenderer.color = originalColor;
    }

    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(steps);
        boxCollider.enabled = false;
        StartCoroutine(Appear());
        StartCoroutine(FadeIn());
    }

    IEnumerator Appear()
    {
        yield return new WaitForSeconds(steps);
        boxCollider.enabled = true;
        StartCoroutine(Disappear());
        StartCoroutine(FadeOut());
    }
}
