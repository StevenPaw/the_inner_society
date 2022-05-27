using System;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

namespace farmingsim
{
    public class Inventory : MonoBehaviour
    {
        [SerializeField] private PlantablePlant item;
        private static Inventory instance;

        public static Inventory Instance => instance;
        public IItem Item => item;

        private void Awake()
        {
            instance = this;
        }
    }
}