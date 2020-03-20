using TMPro;
using UnityEngine;

namespace Utils
{
    public class NumberOrder : MonoBehaviour
    {

        TextMeshProUGUI[] _array;
        private int _newNumber;
        public int _numberAmount;

        void Awake()
        {
            _array = transform.GetComponentsInChildren<TextMeshProUGUI>();
        }

        private void Start()
        {
            _numberAmount -= 1;
        }

        public int NewNumber { get { return _newNumber; } set { _newNumber = value; } }

        int _currentNumber = 0;
        int index = 0;
        string str;

        private void Update()
        {
            if (_currentNumber != _newNumber)
            {
                index = _numberAmount;
                str = _newNumber.ToString();
                for (int i = _numberAmount; i >= 0; --i)
                {
                    _array[index].text = str[i].ToString();
                    --index;
                }
                _currentNumber = _newNumber;
            }
        }

    }
}