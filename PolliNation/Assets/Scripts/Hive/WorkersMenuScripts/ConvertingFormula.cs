using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formula
{
    private Dictionary<ResourceType, int> resourceQuantities;

    public Formula(Dictionary<ResourceType, int> resourceQuantities)
    {
        this.resourceQuantities = resourceQuantities;
    }

    public bool Honey(int quantity)
    {
        int nectarRequired = 2 * quantity;
        int pollenRequired = 1 * quantity;

        if (resourceQuantities.Count != 0)
        {
            resourceQuantities.TryGetValue(ResourceType.Nectar, out int nectarInventory);
            resourceQuantities.TryGetValue(ResourceType.Pollen, out int pollenInventory);

            if (nectarInventory >= nectarRequired && pollenInventory >= pollenRequired)
            {
                resourceQuantities[ResourceType.Nectar] = resourceQuantities[ResourceType.Nectar] - nectarRequired;
                resourceQuantities[ResourceType.Pollen] = resourceQuantities[ResourceType.Pollen] - pollenRequired;
                return true;
            }
        }

        return false;
    }

    public bool Propolis(int quantity)
    {
        int nectarRequired = 3 * quantity;
        int budsRequired = 3 * quantity;

        if (resourceQuantities.Count != 0)
        {
            resourceQuantities.TryGetValue(ResourceType.Nectar, out int nectarInventory);
            resourceQuantities.TryGetValue(ResourceType.Buds, out int budsInventory);

            if (nectarInventory >= nectarRequired && budsInventory >= budsRequired)
            {
                resourceQuantities[ResourceType.Nectar] = resourceQuantities[ResourceType.Nectar] - nectarRequired;
                resourceQuantities[ResourceType.Buds] = resourceQuantities[ResourceType.Buds] - budsRequired;
                return true;
            }
        }

        return false;
    }

    public bool RoyalJelly(int quantity)
    {
        int nectarRequired = 2 * quantity;
        int pollenRequired = 2 * quantity;
        int waterRequired = 2 * quantity;

        if (resourceQuantities.Count != 0)
        {
            resourceQuantities.TryGetValue(ResourceType.Nectar, out int nectarInventory);
            resourceQuantities.TryGetValue(ResourceType.Pollen, out int pollenInventory);
            resourceQuantities.TryGetValue(ResourceType.Water, out int waterInventory);

            if (nectarInventory >= nectarRequired && pollenInventory >= pollenRequired && waterInventory >= waterRequired)
            {
                resourceQuantities[ResourceType.Nectar] = resourceQuantities[ResourceType.Nectar] - nectarRequired;
                resourceQuantities[ResourceType.Pollen] = resourceQuantities[ResourceType.Pollen] - pollenRequired;
                resourceQuantities[ResourceType.Water] = resourceQuantities[ResourceType.Water] - waterRequired;
                return true;
            }
        }

        return false;
    }
}