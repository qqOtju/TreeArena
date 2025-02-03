using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Project.Scripts.UI.Game
{
    public class UIJoystick: MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private RectTransform _inputArea; // Посилання на область вводу
        [SerializeField] private RectTransform _background; // Посилання на фон джойстика
        [SerializeField] private RectTransform _handle;     // Посилання на ручку джойстика
        [ReadOnly] private Vector2 _inputVector;      // Результуючий вектор

        private Vector2 _startPos;
        private float _radius;

        private void Start()
        {
            _radius = _background.sizeDelta.x / 2f; // Радіус фону джойстика
        }

        public void OnPointerDown(PointerEventData eventData)
        {        
            Vector2 localPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _inputArea, 
                    eventData.position, 
                    eventData.pressEventCamera, 
                    out localPoint))
            {
                OnDrag(eventData); // Обробляємо натискання як початкове перетягування
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _background, 
                eventData.position, 
                eventData.pressEventCamera, 
                out pos
            );

            pos = Vector2.ClampMagnitude(pos, _radius); // Обмежуємо радіус
            _handle.anchoredPosition = pos; // Переміщуємо ручку
            _inputVector = pos / _radius;    // Розраховуємо вектор
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _handle.anchoredPosition = Vector2.zero; // Повертаємо ручку в центр
            _inputVector = Vector2.zero;            // Обнуляємо вектор
        }

        public Vector2 GetInput()
        {
            return _inputVector; // Повертаємо вектор для використання
        }
        
    }
}