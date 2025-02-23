// -------------------------------------------------------------------------------
//
// DateTimeFieldController.cs
// Description: Call the DateTimePickerController when the field is selected.
// Assumptions: (1) Each date or time field is tagged with "dateField" or "timeField" respectively
//              (2) The actual picker object is tagged with "dateTimePicker"
// Created:     2 / 13 / 25 © One Bad Ant
//
// -------------------------------------------------------------------------------

// Basic library requirements
// -------------------------------------------------------------------------------
using TMPro;
using UnityEngine;

public class DateTimeFieldController : MonoBehaviour
{
    // -------------------------------------------------------------------------------
    // OnFieldSelected: get the fields value, call the picker
    // -------------------------------------------------------------------------------
    public void OnFieldSelected()
    {
        GameObject picker = GameObject.FindWithTag("dateTimePicker");

        string fieldValue = this.GetComponent<TMP_InputField>().text;
        picker.GetComponent<PickerPanelController>().ShowPickerPanel(this.gameObject);
    }
}
