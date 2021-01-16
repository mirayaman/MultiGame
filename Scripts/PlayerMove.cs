using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [Header("Перемещение")]
    [SerializeField, Range(1.0f, 10.0f)] private float speed = 1.5f; //Скорость перемещения персонажа
    [SerializeField, Range(1.0f, 10.0f)] private float JumpForce = 5.0f; //Сила придаваемая для прыжка
    [SerializeField, Range(0.2f, 10.0f)] private float jumpDistance = 1.2f; //Минимальная дистанция для прыжка
    public KeyCode keyJump = KeyCode.Space;

    [Header("Камера")]
    [Range(1.0f, 100.0f)] public float sensetivity = 5.0f; //Чувствительность мыши игрока
    [SerializeField, Range(-100, -10)] private int minYrotation = -40; //Нижний порог мыши по оси у
    [SerializeField, Range(10, 100)] private int maxYrotation = 40;//Верхний порог мыши по оси у

    [Header("Отладочные данные")]
    [SerializeField] private Transform cameraTransform = null; //Ссылка на обьект камеры
    [SerializeField] private Vector3 moveDerect = Vector3.zero; //Вектор перемещения
    [SerializeField] private Rigidbody rigidBody = null; //Ссылка на rigidbody
    [SerializeField] private float rotationX = 0.0f; //Поворот по оси х
    [SerializeField] private float rotationY = 0.0f; //Поворот по оси у



    private bool isGround() //Переменная для проверки наличия земли под ногами!
    {
        RaycastHit hit; //Переменная столкновений
        Ray ray = new Ray(transform.position + new Vector3(0, 1), Vector3.down); //Луч пущеный из центра игрока
        if (Physics.Raycast(ray, out hit, jumpDistance)) //Передаём данные из луча в переменную hit. Если есть столкновение и обьект находиться на дистанции меньшей или равной jumpDistance возврощаем true
        {
            return true;
        }
        return false;
    }


    private void ControlePlayerCamera(float mouseX = 0.0f, float mouseY = 0.0f) //Метод управления камерой. Принимает позицию мышки.
    {
        rotationX = cameraTransform.localEulerAngles.y + mouseX * sensetivity * Time.deltaTime; //Меняем значение положения в пространстве по оси х
        rotationY += mouseY * sensetivity * Time.deltaTime; //Меняем значение положения в пространстве по оси y
        rotationY = Mathf.Clamp(rotationY, minYrotation, maxYrotation); //Не допускаем что-бы позиция была выше/ниже наших порогов

        cameraTransform.localEulerAngles = new Vector3(-rotationY, rotationX);//Меняем  положение в пространстве
    }

    private void ControlePlayerMove(float horizontalInput = 0.0f, float verticalInput = 0.0f)//Метод перемещения игрока. Принимает в себя wasd или же стик джойстика
    {
        rigidBody.AddForce(moveDerect * speed, ForceMode.VelocityChange); //Двигаем персонажа

        /*Тут две проверки на скорость игрока. В видео об этом было сказанно.*/
        if (Mathf.Abs(rigidBody.velocity.x) > speed)
        {
            rigidBody.velocity = new Vector3(Mathf.Sign(rigidBody.velocity.x) * speed, rigidBody.velocity.y, rigidBody.velocity.z);
        }
        if (Mathf.Abs(rigidBody.velocity.z) > speed)
        {
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, rigidBody.velocity.y, Mathf.Sign(rigidBody.velocity.z) * speed);
        }

        if (isGround())
        {
            //В этом участке кода мы меняем наш вектор. А именно задаём туда куда мы должны идти и поворачиваем наше тело за мышкой!
            moveDerect = new Vector3(horizontalInput, 0.0f, verticalInput);
            moveDerect = cameraTransform.TransformDirection(moveDerect);
            moveDerect = new Vector3(moveDerect.x, 0.0f, moveDerect.z);

            if (Input.GetKey(keyJump))//Проверяем нажатие клавиши прыжка!
            {
                rigidBody.velocity = new Vector2(0, JumpForce);//Прыгаем.
            }
        }

    }

    public void Tick() //Метод для отдельных скриптов.
    {
        ControlePlayerMove(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        ControlePlayerCamera(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    private void Start() //В этом методе идёт поиск обьектов.
    {
        if(rigidBody == null)
        {
            rigidBody = GetComponent<Rigidbody>();
        }
        rigidBody.freezeRotation = true;
        
        if (cameraTransform == null)
            cameraTransform = transform.Find("CameraBox"); cameraTransform = cameraTransform.Find("PlayerCamera"); //Это костыль. Так делать плохо, но иногда можно.
        
    }
}