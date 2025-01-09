using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WaterSort
{
    public class ControlBottle : MonoBehaviour
    {
        private Stack<ItemID> colorInBottle = new Stack<ItemID>();
        [SerializeField] private SpriteRenderer renderMask;

        private float timeRotation = 5f;
        [SerializeField] private AnimationCurve scaleAndRorationCurve;
        [SerializeField] private AnimationCurve fillAmountCurve;
        [SerializeField] private AnimationCurve shadowCurve;
        [SerializeField] private AnimationCurve thickCurve;

        private static float[] fillAmounts = new[] { -0.5f, -0.3f, -0.1f, 0.1f, 0.3f };
        private static float[] rotationValues = new[] { 54f, 71f, 83f, 90f };

        private void Start()
        {
            SetColor();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(IE_RotateBottle());
            }
        }

        private void SetColor()
        {
            ItemID[] array = colorInBottle.ToArray();
            int length = array.Length;
            for (int i = 0; i < length; i++)
            {
                renderMask.material.SetColor($"_Color0{i + 1}", DataColorManager.Instance.GetData(array[i]).ColorMain);
            }
            renderMask.material.SetColor("_ColorShadow", DataColorManager.Instance.GetData(array[length-1]).ColorMain);
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