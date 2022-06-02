using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Player_Controller : MonoBehaviour
{

    [Header("Variables")]
    [SerializeField] float m_maxSpeed = 4.5f;
    [SerializeField] float m_jumpForce = 7.5f;
    [SerializeField] float timer = 0;
    [SerializeField] int attackDamage = 20;
    private Animator m_animator;
    private Rigidbody2D m_body2d;
    private Sensor_Prototype m_groundSensor;
    private AudioSource m_audioSource;
    private AudioEffects m_audioManager;

    private bool m_grounded = false;
    private bool m_moving = false;
    public static int m_facingDirection = 1;

    private float m_disableMovementTimer = 0.0f;

    private bool isAttack1 = false;
    private bool isChargeAttack = false;

    public Transform attackPoint;
    public Transform sp_atk_point;
    public Transform throwPoint;
    public float attackRange = 0.5f;
    public Vector2 sp_atk_range = new Vector2(12f, 3f);
    public LayerMask enemyLayers;
    public GameObject dagger_throw;
    public GameObject skill_dagger;

    int stopMoving = 1;

    float currentDashTimmer;
    bool isRolling = false;
    public float rollDistance;
    public float startDashTimer;
    public float trapDistanceEvade;

    bool isTraping = false;
    bool isDelayAction = false;
    public float actionDelay = 0.5f;


    public float attackRate = 2f;

    public int max_hp = 100;
    public static int number_of_dagger = 15;
    public static int currentHp;


    // Use this for initialization
    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        m_audioSource = GetComponent<AudioSource>();
        m_audioManager = AudioEffects.instance;
        m_groundSensor = transform.Find("GroundSensor").GetComponent<Sensor_Prototype>();
        currentHp = max_hp;
    }

    // Update is called once per frame
    void Update()
    {
    }
}