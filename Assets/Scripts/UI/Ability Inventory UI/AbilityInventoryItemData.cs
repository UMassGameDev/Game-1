using UnityEngine;
using System.Collections.Generic;

/** \brief
Holds the data of the drag and drop ability icons in the ability inventory in the skyhub.
This includes all data that the UI needs to display, as well as what slot the ability icon is in.

Documentation updated 9/19/2024
\author Stephen Nuttall
*/
public class AbilityInventoryItemData : MonoBehaviour
{
    /// The name of the ability in this slot.
    public string abilityName;
    /// The level of the ability in this slot.
    public int abilityLevel;
    /// The sprite of the ability in this slot.
    public Sprite sprite;
    /// The quote to display in the detailed view.
    public string quote;

    /// The offense, defense, utility, and passive icons to display in the detailed view.
    public List<Sprite> abilityIcons;
    /// The names of the offense, defense, utility, and passive abilities to display in the detailed view.
    public string[] abilityNames;
    /// The descriptions of the offense, defense, utility, and passive abilities to display in the detailed view.
    public string[] abilityDescriptions;

    /// \brief The ID of the slot the item was in before it was dragged.
    /// Used to return the icon to its current slot if it was dragged to a place that isn't another open slot.
    public int startingSlotKey;

    /// Moves the icon to the given position.
    public void MoveIcon(Vector2 newPos) { gameObject.transform.position = newPos; }
}
