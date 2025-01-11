using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace WaterSort
{
    public class ControlBottle : MonoBehaviour
    {
        private Stack<ItemID> colorInBottle = new Stack<ItemID>();
        [SerializeField] private SpriteRenderer renderMask;

        private float timeRotation = 1f;
        [SerializeField] private AnimationCurve scaleAndRorationCurve;
        [SerializeField] private AnimationCurve fillAmountCurve;
        [SerializeField] private AnimationCurve shadowCurve;
        [SerializeField] private AnimationCurve thickCurve;
        private Vector3 posRoot;
        private static float[] fillAmounts = new[] { -0.55f, -0.25f, -0.05f, 0.15f, 0.35f };
        private static float[] rotationValues = new[] { 54f, 71f, 83f, 90f };
        private int idAnimAdd = 0;
        private bool animRunning;
        [SerializeField] private Collider2D coll;

        public ControlBottle Target
        {
            set
            {
                if (value != null)
                {
                    int countAdd = 0;
                    ItemID top = ColorTop;
                    foreach (ItemID temp in colorInBottle)
                    {
                        if (top == temp && value.CountCanReceive > 0)
                        {
                            value.AddColor(temp);
                            countAdd++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    MoveTarget(value, countAdd);
                }
            }
        }

        public ItemID CanReceive
        {
            get
            {
                if (CountColor >= 4)
                {
                    return ItemID.None;
                }
                else if (CountColor <= 0)
                {
                    return ItemID.ColorEnd;
                }
                else
                {
                    return colorInBottle.Peek();
                }
            }
        }

        public ItemID ColorTop
        {
            get
            {
                if (CountColor > 0)
                {
                    return colorInBottle.Peek();
                }
                else
                {
                    return ItemID.None;
                }
            }
        }

        private int CountColor
        {
            get => colorInBottle.Count;
        }

        public int CountCanReceive
        {
            get => 4 - CountColor;
        }

        public void Init(ItemID[] colors)
        {
            animRunning = false;
            posRoot = transform.position;
            colorInBottle.Clear();
            foreach (ItemID color in colors)
            {
                AddColor(color);
            }

            UpdateFillamount();
        }

        private void AddColor(ItemID _id)
        {
            colorInBottle.Push(_id);
            renderMask.material.SetColor($"_Color0{colorInBottle.Count}", DataColorManager.Instance.GetData(colorInBottle.Peek()).ColorMain);
            renderMask.material.SetColor("_ColorShadow", DataColorManager.Instance.GetData(colorInBottle.Peek()).ColorShadow);
        }

        public void OnSelect()
        {
            coll.enabled = false;
            Vector3 pos = transform.localPosition;
            pos.y += 1;
            transform.DOLocalMoveY(pos.y, 0.3f);
        }

        private void MoveTarget(ControlBottle _target, int _countAdd)
        {
            Vector3 posTarget = _target.transform.position + Vector3.up;
            transform.DOMove(posTarget, 0.3f).OnComplete(
                () => { StartCoroutine(IE_WaitDropWater(_target, _countAdd)); });
        }

        private IEnumerator IE_WaitDropWater(ControlBottle _target, int _countAdd)
        {
            float timeDrop = RhythmManager.TimeDrop(_countAdd);
            _target.UpdateFillamountAnim(timeDrop);
            yield return IE_RotateBottle();
            yield return new WaitForSeconds(timeDrop);
            OnNoSelect();
            yield break;
        }

        public void OnNoSelect()
        {
            StartCoroutine(RotateBottleBack(0.3f));
            transform.DOMove(posRoot, 0.3f).OnComplete(() => { coll.enabled = true; });
        }

        private void UpdateFillamount()
        {
            renderMask.material.SetFloat("_FillAmount", fillAmounts[CountColor]);
        }

        private void UpdateFillamountAnim(float _timeDrop)
        {
            float current = renderMask.material.GetFloat("_FillAmount");
            animRunning = true;
            Tween tween = DOVirtual.Float(current, fillAmounts[CountColor], _timeDrop,
                    (value) => { renderMask.material.SetFloat("_FillAmount", value); })
                .OnComplete(() => { animRunning = false; });
            idAnimAdd = tween.intId;
        }

        private IEnumerator IE_RotateBottle()
        {
            float time = 0;
            float lerpValue = 0;
            float angleValue = 0;
            while (time < timeRotation)
            {
                lerpValue = time / timeRotation;
                angleValue = Mathf.Lerp(0, 90, lerpValue);
                transform.eulerAngles = new Vector3(0, 0, angleValue);
                renderMask.material.SetFloat("_ScaleAndRotation", scaleAndRorationCurve.Evaluate(angleValue));
                renderMask.material.SetFloat("_FillAmount", fillAmountCurve.Evaluate(angleValue));
                renderMask.material.SetFloat("_Shadow", shadowCurve.Evaluate(angleValue));
                renderMask.material.SetFloat("_Thick", thickCurve.Evaluate(angleValue));
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            angleValue = 90;
            transform.eulerAngles = new Vector3(0, 0, angleValue);
            renderMask.material.SetFloat("_FillAmount", fillAmountCurve.Evaluate(angleValue));
            renderMask.material.SetFloat("_ScaleAndRotation", scaleAndRorationCurve.Evaluate(angleValue));
            renderMask.material.SetFloat("_Shadow", shadowCurve.Evaluate(angleValue));
            renderMask.material.SetFloat("_Thick", thickCurve.Evaluate(angleValue));
        }
        IEnumerator RotateBottleBack(float _timeback)
        {
            float t = 0;
            float lerpValue;
            float angleValue;
            while (t < _timeback)
            {
                lerpValue = t / _timeback;
                angleValue = Mathf.Lerp(90,0, lerpValue);
                transform.eulerAngles = new Vector3(0, 0, angleValue);
                renderMask.material.SetFloat("_ScaleAndRotation", scaleAndRorationCurve.Evaluate(angleValue));
                renderMask.material.SetFloat("_Shadow", shadowCurve.Evaluate(angleValue));
                renderMask.material.SetFloat("_Thick", thickCurve.Evaluate(angleValue));
                t += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            angleValue = 0;
            transform.eulerAngles = new Vector3(0, 0, angleValue);
            renderMask.material.SetFloat("_ScaleAndRotation", scaleAndRorationCurve.Evaluate(angleValue));
            renderMask.material.SetFloat("_Shadow", shadowCurve.Evaluate(angleValue));
            renderMask.material.SetFloat("_Thick", thickCurve.Evaluate(angleValue));
        }
#if UNITY_EDITOR
        private void OnValidate()
        {
            coll = GetComponent<Collider2D>();
        }
#endif
        private void OnDestroy()
        {
            if (animRunning)
            {
                DOTween.Kill(idAnimAdd);
            }
        }
    }
}