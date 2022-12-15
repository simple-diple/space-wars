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
        private bool _isDamageEffect;

        private void Awake()
        {
            _material = render.material;
        }

        public void ShowDamageEffect()
        {
            StartCoroutine(ShieldEffect(Color.red));
        }

        public void ShowRecoveryEffect()
        {
            StartCoroutine(ShieldEffect(Color.cyan));
        }

        private IEnumerator ShieldEffect(Color effectColor)
        {
            if (_isDamageEffect)
            {
                yield break;
            }

            _material.color = effectColor;
            Color color = _material.color;
            color.a = 0;
            _material.color = color;
            _isDamageEffect = true;
            
            while (_material.color.a < 0.3f)
            {
                color = _material.color;
                color.a += Time.deltaTime * effectSpeed;
                _material.color = color;
                yield return null;
            }
            
            while (_material.color.a >= 0)
            {
                color = _material.color;
                color.a -= Time.deltaTime * effectSpeed;
                _material.color = color;
                yield return null;
            }

            _isDamageEffect = false;
        }

        public void SetActive(bool value)
        {
            render.enabled = value;
        }

        public void SetCollider(bool value)
        {
            sphereCollider.enabled = value;
        }
    }
}