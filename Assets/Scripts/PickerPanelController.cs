// -------------------------------------------------------------------------------
//
// PickerPanelController.cs
// Description:   Control the appearance of the date/time picker.
//                Present the date or time picker.
//                Collect the selected data from the picker.
// Input:         (1) field name
//                (2) picker type: "date" or "time"
//                (3) field value
// Output:        (1) Selected date or time
// Created:       2 / 13 / 25 © One Bad Ant
//
// -------------------------------------------------------------------------------

// Basic library requirements
// -------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class PickerPanelController : MonoBehaviour
{
    // position and appearance vars
    public float appearanceSpeed = 20f;
    public float hiddenYPos = 11.5f;
    public float shownYPos = 4.0f;
    private float targetTopYPos;

    // Date panel objects
    public GameObject datePanel;
    public GameObject mmScrollView;
    public GameObject ddScrollView;
    public GameObject yyyyScrollView;

    // Time panel objects
    public GameObject timePanel;
    public GameObject hhScrollView;
    public GameObject minScrollView;

    public GameObject calledByField; // field that called for the date/time picker


    // -------------------------------------------------------------------------------
    // Start: picker hidden by default
    // -------------------------------------------------------------------------------
    void Start()
    {
        datePanel.SetActive(false);
        timePanel.SetActive(false);
        transform.position = new Vector2(transform.position.x, hiddenYPos);
    }

    // -------------------------------------------------------------------------------
    // The date/time picker has been requested for a field with these attributes
    // (1) field name
    // (2) picker type: "date" or "time"
    // (3) field value
    // -------------------------------------------------------------------------------
    public void ShowPickerPanel(GameObject callingField)
    {
        calledByField = callingField;

        if (callingField.tag == "dateField")
        {
            datePanel.SetActive(true);
            timePanel.SetActive(false);
        }
        else
        {
            datePanel.SetActive(false);
            timePanel.SetActive(true);
        }

        MovePickerPanel("show");
    }

    // -------------------------------------------------------------------------------
    // Picker's "Canel" button pressed: dismiss the picker
    // -------------------------------------------------------------------------------
    public void CancelPickerPanel()
    {
        MovePickerPanel("hide");
    }

    // -------------------------------------------------------------------------------
    // Picker's "Clear" button pressed: empty the time or date field and hide the picker
    // -------------------------------------------------------------------------------
    public void ClearlDateTime()
    {
        calledByField.GetComponent<TMP_InputField>().text = "";
        MovePickerPanel("hide");
    }

    // -------------------------------------------------------------------------------
    // Picker's "Save" button pressed: format the selected data and update the calling field
    // -------------------------------------------------------------------------------
    public void SaveDateTime()
    {
        var formattedData = "";
        if (calledByField.GetComponent<TMP_InputField>().tag == "dateField")
        {
            // get current date scrollview indices
            int mmi = mmScrollView.GetComponent<PickerScrollController>().currentItem + 1;
            int ddi = ddScrollView.GetComponent<PickerScrollController>().currentItem;
            int yyyyi = yyyyScrollView.GetComponent<PickerScrollController>().currentItem;
            var newMonth = mmi.ToString();

            if (mmi < 10)
            {
                newMonth = "0" + newMonth;
            }
            var newDay = ddScrollView.GetComponent<PickerScrollController>().scrollContentArea.transform.GetChild(ddi).name;
            if (ddi < 10)
            {
                newDay = "0" + newDay;
            }
            var newYear = yyyyScrollView.GetComponent<PickerScrollController>().scrollContentArea.transform.GetChild(yyyyi).name;

            formattedData = newMonth + "/" + newDay + "/" + newYear;
        }

        // Format and update time
        else
        {
            // get current date scrollview indices
            int hhi = hhScrollView.GetComponent<PickerScrollController>().currentItem + 1;
            int mini = minScrollView.GetComponent<PickerScrollController>().currentItem + 1;

            var newHour = hhi.ToString();
            if (hhi < 10)
            {
                newHour = "0" + newHour;
            }
            var newMin = mini.ToString();
            if (mini < 10)
            {
                newMin = "0" + newMin;
            }

            formattedData = newHour + ":" + newMin;
        }

        calledByField.GetComponent<TMP_InputField>().text = formattedData;
        MovePickerPanel("hide");
    }

    // -------------------------------------------------------------------------------
    // OnClick time or date field shows or hides the picker and takes:
    // -------------------------------------------------------------------------------
    public void MovePickerPanel(string direction)
    {
        targetTopYPos = shownYPos; // default is open the picker
        if (direction == "hide")
        {
            targetTopYPos = hiddenYPos;
        }

        StartCoroutine(AnimateMovement(direction));
    }

    IEnumerator AnimateMovement(string direction)
    {
        float verticalInput = -1 * (targetTopYPos * Time.deltaTime);

        while (direction == "show" && transform.position.y > targetTopYPos)
        {
            transform.Translate(new Vector3(0f, verticalInput, 0f));
            yield return null;
        }

        while (direction == "hide" && transform.position.y < targetTopYPos)
        {
            transform.Translate(new Vector3(0f, -verticalInput, 0f));
            yield return null;
        }
        initializeDateTimePickers();
    }

    // -------------------------------------------------------------------------------
    // Set the picker's initial values if present or to current values if empty
    // -------------------------------------------------------------------------------
    public void initializeDateTimePickers()
    {
        // Date picker
        if (calledByField.tag == "dateField")
        {
            var initialDateValue = calledByField.GetComponent<TMP_InputField>().text;
            DateTime currentDateValue = DateTime.Today; // default to today if date field is empty
            if (initialDateValue != "" && initialDateValue != "n/a")
            {
                // get the date field value if it's not empty
                currentDateValue = DateTime.ParseExact(initialDateValue, "MM/dd/yyyy", CultureInfo.InvariantCulture);
            }
            mmScrollView.GetComponent<PickerScrollController>().SetInitialTargetedItem(currentDateValue.Month - 1);
            yyyyScrollView.GetComponent<PickerScrollController>().SetInitialTargetedItem(currentDateValue.Year - 1999); // list start year is 1999
            ddScrollView.GetComponent<PickerScrollController>().SetInitialTargetedItem(currentDateValue.Day - 1);
        }

        // Time picker
        else
        {
            var initialTimeValue = calledByField.GetComponent<TMP_InputField>().text;
            DateTime currenTimeValue = DateTime.Now; // default to now if time field is empty
            if (initialTimeValue != "" && initialTimeValue != "n/a")
            {
                // get the time field value if it's not empty
                currenTimeValue = DateTime.ParseExact(initialTimeValue, "HH:mm", CultureInfo.InvariantCulture);
            }
            hhScrollView.GetComponent<PickerScrollController>().SetInitialTargetedItem(currenTimeValue.Hour - 1);
            minScrollView.GetComponent<PickerScrollController>().SetInitialTargetedItem(currenTimeValue.Minute - 1);
        }
    }
}
