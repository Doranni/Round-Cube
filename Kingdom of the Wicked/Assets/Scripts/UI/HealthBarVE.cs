using UnityEngine.UIElements;

public class HealthBarVE : VisualElement
{
    public new class UxmlFactory : UxmlFactory<HealthBarVE> { }

    public Health ChHealth { get; private set; }
    protected Label healthValue;
    const string k_healthbar_class = "health_bar";

    public HealthBarVE() 
    {
        healthValue = new Label();
        healthValue.AddToClassList(k_healthbar_class);
        healthValue.text = "Health:";
        hierarchy.Add(healthValue);
    }

    public void Init(Health character)
    {
        ChHealth = character;
        ChHealth.HealthChanged += UpdateHealth;
        UpdateHealth((ChHealth.CurrentHealth, ChHealth.MaxHealth));
    }

    public void UpdateHealth((float currentHealth, float maxHealth) value)
    {
        healthValue.text = "Health: " + value.currentHealth + "/" + value.maxHealth;
    }
}
