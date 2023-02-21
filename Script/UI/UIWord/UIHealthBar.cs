using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Image HealthBarImage;

    public Enemy enemyData;
    public UIWorldCanvas UIWorldCanvas;
    public Transform HealthBarPosition;

    public void Init(Enemy enemy, Transform HealthBarPoint)
    {
        if(this.UIWorldCanvas == null)
        {
            this.UIWorldCanvas = FindObjectOfType<UIWorldCanvas>();
        }
        this.transform.SetParent(this.UIWorldCanvas.transform);
        this.enemyData = enemy;
        this.HealthBarPosition = HealthBarPoint;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHealthBar();
    }
    private void LateUpdate()
    {
        this.transform.forward = Camera.main.transform.forward;
        this.transform.position = this.HealthBarPosition.position;
    }
    public void UpdateHealthBar()
    {
        HealthBarImage.fillAmount = enemyData.EnemyState.CurrentHealth / enemyData.EnemyState.MaxHealth;
    }
}
