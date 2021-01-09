using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField, Range(1.0f, 10.0f)] private float speed = 1.5f; //Скорость перемещения персонажа
    [SerializeField, Range(1.0f, 10.0f)] private float JumpForce = 5.0f; //Сила придаваемая для прыжка
    [SerializeField, Range(0.2f, 10.0f)] private float jumpDistance = 1.2f; //Минимальная дистанция для прыжка

    [Range(1.0f, 100.0f)] public float sensetivity = 5.0f; //Чувствительность мыши игрока
    [SerializeField, Range(-100, -10)] private int minYrotation = -40; //Нижний порог мыши по оси у
    [SerializeField, Range(10, 100)] private int maxYrotation = 40;//Верхний порог мыши по оси у

    /*Debug*/
    [SerializeField] private Transform camera = null; //Ссылка на обьект камеры
    [SerializeField] private Vector3 moveDerect = Vector3.zero; //Вектор перемещения
    [SerializeField] private Rigidbody rigidbody = null; //Ссылка на rigidbody
    [SerializeField] private float rotationX = 0.0f; //Поворот по оси х
    [SerializeField] private float rotationY = 0.0f; //Поворот по оси у

    private bool isGround() //Переменная для проверки наличия земли под ногами!
    {
        RaycastHit hit; //Переменная столкновений
        Ray ray = new Ray(transform.position + new Vector3(0, 1), Vector3.down); //Луч пущеный из центра игрока
        if(Physics.Raycast(ray, out hit, jumpDistance)) //Передаём данные из луча в переменную hit. Если есть столкновение и обьект находиться на дистанции меньшей или равной jumpDistance возврощаем true
        {
            return true;
        }
        return false;
    }

    private void ControlePlayerCamera(float mouseX = 0.0f, float mouseY = 0.0f) //Метод управления камерой. Принимает позицию мышки.
    {
        rotationX = camera.localEulerAngles.y + mouseX * sensetivity * Time.deltaTime; //Меняем значение положения в пространстве по оси х
        rotationY += mouseY * sensetivity * Time.deltaTime; //Меняем значение положения в пространстве по оси y
        rotationY = Mathf.Clamp(rotationY, minYrotation, maxYrotation); //Не допускаем что-бы позиция была выше/ниже наших порогов

        camera.localEulerAngles = new Vector3(-rotationY, rotationX);//Меняем  положение в пространстве
    }

    private void ControlePlayerMove(float horizontal = 0.0f, float vertical = 0.0f)//Метод перемещения игрока. Принимает в себя wasd или же стик джойстика
    {
        rigidbody.AddForce(moveDerect * speed, ForceMode.VelocityChange); //Двигаем персонажа

		/*Тут две проверки на скорость игрока. В видео об этом было сказанно.*/
        if(Mathf.Abs(rigidbody.velocity.x) > speed)
        {
            rigidbody.velocity = new Vector3(Mathf.Sign(rigidbody.velocity.x) * speed, rigidbody.velocity.y, rigidbody.velocity.z);
        }
        if(Mathf.Abs(rigidbody.velocity.z) > speed)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, rigidbody.velocity.y, Mathf.Sign(rigidbody.velocity.z) * speed);
        }

		//В этом участке кода мы меняем наш вектор. А именно задаём туда куда мы должны идти и поворачиваем наше тело за мышкой!
        moveDerect = new Vector3(horizontal, 0.0f, vertical);
        moveDerect = camera.TransformDirection(moveDerect);
        moveDerect = new Vector3(moveDerect.x, 0.0f, moveDerect.z);

        if(isGround() && Input.GetKey(KeyCode.Space))//Проверяем землю (Объект игрок находится на земле) и нажатие клавиши прыжка!
        {
            rigidbody.velocity = new Vector2(0, JumpForce);//Прыгаем.
        }
    }

    public void Tick() //Метод для отдельных скриптов.
    {
        ControlePlayerMove(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        ControlePlayerCamera(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));
    }

    private void Start() //В этом методе идёт поиск обьектов.
    {
        if(rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
        rigidbody.freezeRotation = true;

		if(camera == null)
        {
            camera = transform.Find("CameraBox"); camera = camera.Find("PlayerCamera"); //Это костыль. Так делать плохо, но иногда можно.
        }
        
    }

    /*Debug*/
    private void Update()
    {
        Tick();
    }
}
