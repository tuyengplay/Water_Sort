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
        public ItemID CanReceive
        {
            get
            {
                if (CountColor >= 4 || CountColor <= 0)
                {
                    return ItemID.None;
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

        private static float[] fillAmounts = new[] { -0.5f, -0.3f, -0.1f, 0.1f, 0.3f };
        private static float[] rotationValues = new[] { 54f, 71f, 83f, 90f };

        private void Start()
        {
            Init();
        }

        public void Init()
        {
            posRoot = transform.position;
            colorInBottle.Clear();
            ItemID[] coloraa = new[]
            {
                ItemID.ColorBrown,
                ItemID.ColorBlue,
                ItemID.ColorBrown,
                ItemID.ColorGreen,
            };
            foreach (ItemID color in coloraa)
            {
                AddColor(color);
            }
        }

        private void AddColor(ItemID _id)
        {
            colorInBottle.Push(_id);
            renderMask.material.SetColor($"_Color0{colorInBottle.Count}", DataColorManager.Instance.GetData(colorInBottle.Peek()).ColorMain);
            renderMask.material.SetColor("_ColorShadow", DataColorManager.Instance.GetData(colorInBottle.Peek()).ColorShadow);
        }

        public void OnSelect()
        {
            Vector3 pos = transform.localPosition;
            pos.y += 1;
            transform.DOLocalMoveY(pos.y, 0.3f);
        }
        public void OnNoSelect()
        {
            transform.DOMove(posRoot, 0.3f);
        }

        private IEnumerator IE_RotateBottle()
        {
            float time = 0;
            float lerpValue = 0;
            float angleValue = 0;
            while (time < timeRotation)
            {
                lerpValue = time / timeRotation;
                angleValue = Mathf.Lerp(0.0f, 90.0f, lerpValue);
                transform.eulerAngles = new Vector3(0, 0, angleValue);
                renderMask.material.SetFloat("_ScaleAndRotation", scaleAndRorationCurve.Evaluate(angleValue));
                renderMask.material.SetFloat("_FillAmount", fillAmountCurve.Evaluate(angleValue));
                renderMask.material.SetFloat("_Shadow", shadowCurve.Evaluate(angleValue));
                renderMask.material.SetFloat("_Thick", thickCurve.Evaluate(angleValue));
                time += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            angleValue = 90f;
            transform.eulerAngles = new Vector3(0, 0, angleValue);
            renderMask.material.SetFloat("_ScaleAndRotation", scaleAndRorationCurve.Evaluate(angleValue));
            renderMask.material.SetFloat("_FillAmount", fillAmountCurve.Evaluate(angleValue));
            renderMask.material.SetFloat("_Shadow", shadowCurve.Evaluate(angleValue));
            renderMask.material.SetFloat("_Thick", thickCurve.Evaluate(angleValue));
        }
    }
}