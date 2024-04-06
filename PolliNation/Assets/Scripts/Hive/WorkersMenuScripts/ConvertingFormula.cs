using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formula
{
    private Dictionary<ResourceType, int> resourceQuantities;
    private bool cancel = false;

    public Formula(Dictionary<ResourceType, int> resourceQuantities)
    {
        this.resourceQuantities = resourceQuantities;
    }


    public bool resourceEnough(ResourceType resourceType, int quantity)
    {   
        bool result = false;
        switch (resourceType) 
        {
            case ResourceType.Honey:
                result = Honey(quantity);
                break;
            case ResourceType.Propolis:
                result = Propolis(quantity);
                break;
            case ResourceType.RoyalJelly:
                result = RoyalJelly(quantity);
                break;
        }

        return result;
    }

    public void Cancel(ResourceType resourceType)
    { 
        cancel = true;
        int quantity = 1;
        switch(resourceType)
        {
            case ResourceType.Honey:
                Honey(quantity);
                break;
            case ResourceType.Propolis:
                Propolis(quantity);
                break;
            case ResourceType.RoyalJelly:
                RoyalJelly(quantity);
                break;
        }
    }

    private bool Honey(int quantity)
    {
        int nectarRequired = 2 * quantity;
        int pollenRequired = 1 * quantity;

        if (cancel)
        {
            resourceQuantities[ResourceType.Nectar] += nectarRequired;
            resourceQuantities[ResourceType.Pollen] += pollenRequired;
            return true;
        }

        cancel = false;
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

    private bool Propolis(int quantity)
    {
        int nectarRequired = 3 * quantity;
        int budsRequired = 3 * quantity;

        if (cancel)
        {
            resourceQuantities[ResourceType.Nectar] += nectarRequired;
            resourceQuantities[ResourceType.Buds] += budsRequired;
            return true;
        }

        cancel = false;

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

    private bool RoyalJelly(int quantity)
    {
        int nectarRequired = 2 * quantity;
        int pollenRequired = 2 * quantity;
        int waterRequired = 2 * quantity;

        if (cancel)
        {
            resourceQuantities[ResourceType.Nectar] += nectarRequired;
            resourceQuantities[ResourceType.Pollen] += pollenRequired;
            resourceQuantities[ResourceType.Water] += waterRequired;
            return true;
        }

        cancel = false;

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