using System.Collections;
using UnityEngine;

namespace View
{
    public class ShieldView : GameObjectView
    {
        public Collider sphereCollider;
        public float effectAlpha;
        public float effectSpeed;
        public Renderer render;
        private Material _material;
        private Coroutine _coroutine;

        private void Awake()
        {
            _material = render.material;
        }
        
        public void SetCollider(bool value)
        {
            sphereCollider.enabled = value;
        }

        public void ShowDamageEffect()
        {
            ShowEffect(new Color(1f, 0.0f, 0.0f, _material.color.a));
        }

        public void ShowRecoveryEffect()
        {
            ShowEffect(new Color(0.0f, 1f, 1f, _material.color.a));
        }
        
        private void ShowEffect(Color color)
        {
            _material.color = color;
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            _coroutine = StartCoroutine(ShieldEffect());
        }

        public void SetActive(bool value)
        {
            render.enabled = value;
        }

        private IEnumerator ShieldEffect()
        {
            float alpha = 0;
            _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, alpha);
            
            while (_material.color.a < effectAlpha)
            {
                alpha += Time.deltaTime * effectSpeed;
                _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, alpha);
                yield return null;
            }
            
            while (_material.color.a >= 0)
            {
                alpha -= Time.deltaTime * effectSpeed;
                _material.color = new Color(_material.color.r, _material.color.g, _material.color.b, alpha);
                yield return null;
            }
        }
    }
}