using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace WaterSort
{
    [CreateAssetMenu(fileName = "DataColorManager", menuName = "Data/DataColorManager", order = 1)]
    public class DataColorManager : ScriptableSingleton<DataColorManager>
    {
        [SerializeField,TableList,Searchable] private DataColor[] colors;
    }

    [System.Serializable]
    internal class DataColor
    {
        [SerializeField] private ItemID idColor;
        [SerializeField] private Color colorMain;
        [SerializeField] private Color colorShadow;
    }
}