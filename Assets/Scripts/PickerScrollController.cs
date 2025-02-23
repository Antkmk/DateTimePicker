// -------------------------------------------------------------------------------
//
// PickerScrollController.cs
// Description:   Control the scrolling and targeting the the scrollview items
//                for a specific scrollview
// Created:       2 / 14 / 25 © One Bad Ant
//
// -------------------------------------------------------------------------------

// Basic library requirements
// -------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PickerScrollController : MonoBehaviour
{
    // Scrollview attributes
    public GameObject datePickerPanel;
    public GameObject timePickerPanel;
    public GameObject pickerItem; // prefab
    private GameObject scrollViewItem; // instatiated prefab items
    public ScrollRect scrollViewRect;
    public RectTransform scrollContentArea;
    public RectTransform contentListItem;
    public VerticalLayoutGroup verticalLayoutGroup;

    // Access to month and year scrollviews required for days scroll view updates
    public GameObject ddContentArea;
    public GameObject mmScrollView;
    public GameObject yyyyScrollView;

    public int currentItem = 0; // currently targeted item in the scrollview

    private bool targeted;
    float targetingSpeed;
    float targetingForce = 2;
    int scrollMagnitude = 20;


    // -------------------------------------------------------------------------------
    // The scrollview has not yet been set to its value
    // -------------------------------------------------------------------------------
    void Start()
    {
        targeted = false;
        SetScrollViewInitialValues();
    }

    // -------------------------------------------------------------------------------
    // Handle scrolling and targeting the selected scroll list item
    // -------------------------------------------------------------------------------
    void Update()
    {
        // make sure there's date in the scrollview before animating anything
        if (scrollContentArea.transform.childCount > 0)
        {
            contentListItem = (RectTransform)scrollContentArea.transform.GetChild(0);
            currentItem = Mathf.RoundToInt(scrollContentArea.localPosition.y / (contentListItem.rect.height + verticalLayoutGroup.spacing));

            // target the scrolled to item
            if (scrollViewRect.velocity.magnitude < scrollMagnitude && !targeted)
            {
                scrollViewRect.velocity = Vector2.zero;
                targetingSpeed += targetingForce * Time.deltaTime;

                scrollContentArea.localPosition = new Vector2(
                    scrollContentArea.localPosition.x,
                    Mathf.MoveTowards(scrollContentArea.localPosition.y, currentItem * (contentListItem.rect.height + verticalLayoutGroup.spacing), targetingSpeed));

                if (scrollContentArea.localPosition.y == currentItem * (contentListItem.rect.height + verticalLayoutGroup.spacing))
                {
                    targeted = true;

                    // send the currently targeted month and year values to the day picker manager 
                    // to adjust number of days and handle leap years
                    int changedYear;
                    int changedMonth;
                    switch (this.name) // name of the month or year scroll view as it has re-targeted
                    {
                        case "MMScrollView":
                            int currYYYYIndex = yyyyScrollView.GetComponent<PickerScrollController>().currentItem;
                            changedYear = Convert.ToInt32(yyyyScrollView.GetComponent<PickerScrollController>().scrollContentArea.transform.GetChild(currYYYYIndex).name);
                            UpdateDaysList(currentItem, changedYear);
                            break;
                        case "YYYYScrollView":
                            changedMonth = mmScrollView.GetComponent<PickerScrollController>().currentItem;
                            changedYear = Convert.ToInt32(this.scrollContentArea.transform.GetChild(currentItem).name);
                            UpdateDaysList(changedMonth, changedYear);
                            break;
                    }
                }
            }

            // scrolling in action, prepare for targeting
            if (scrollViewRect.velocity.magnitude > scrollMagnitude)
            {
                targeted = false;
                targetingSpeed = 0;
            }
        }
    }

    // -------------------------------------------------------------------------------
    // This routine will be called if the scrollview has an initial value in which
    // case the scrollview defaults to it.
    // -------------------------------------------------------------------------------
    public void SetInitialTargetedItem(int setItem)
    {
        scrollContentArea.localPosition = new Vector2(
                    scrollContentArea.localPosition.x,
                    setItem * (contentListItem.rect.height + verticalLayoutGroup.spacing));
    }

    // -------------------------------------------------------------------------------
    // Set the initial values for the scrollview this script is attached to 
    // -------------------------------------------------------------------------------
    private void SetScrollViewInitialValues()
    {
        if (datePickerPanel.activeSelf == true)
        {
            // The Month Scrollview
            // -------------------------------------------------------------------------------
            if (this.name == "MMScrollView")
            {
                List<string> months = new List<string> { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
                for (int i = 0; i < 12; i++)
                {
                    scrollViewItem = Instantiate(pickerItem, scrollContentArea.transform);
                    scrollViewItem.name = months[i];
                    scrollViewItem.GetComponent<TextMeshProUGUI>().text = months[i];
                }
            }

            // The Year Scrollview
            // -------------------------------------------------------------------------------
            if (this.name == "YYYYScrollView")
            {
                int startYear = 1999;
                int endYear = DateTime.Now.Year + 5;
                for (int i = startYear; i < endYear; i++)
                {
                    scrollViewItem = Instantiate(pickerItem, scrollContentArea.transform);
                    scrollViewItem.name = i.ToString();
                    scrollViewItem.GetComponent<TextMeshProUGUI>().text = i.ToString();
                }
            }

            // The Day Scrollview
            // -------------------------------------------------------------------------------
            if (this.name == "DDScrollView")
            {
                UpdateDaysList(0, 1999);
            }
        }

        if (timePickerPanel.activeSelf == true)
        {
            // The Hour Scrollview
            // -------------------------------------------------------------------------------
            if (this.name == "HHScrollView")
            {
                for (int i = 1; i < 25; i++)
                {
                    scrollViewItem = Instantiate(pickerItem, scrollContentArea.transform);
                    if (i < 10)
                    {
                        scrollViewItem.name = "0" + i.ToString();
                        scrollViewItem.GetComponent<TextMeshProUGUI>().text = "0" + i.ToString();
                    }
                    else
                    {
                        scrollViewItem.name = i.ToString();
                        scrollViewItem.GetComponent<TextMeshProUGUI>().text = i.ToString();
                    }
                }
            }

            // The Minute Scrollview
            // -------------------------------------------------------------------------------
            if (this.name == "MinScrollView")
            {
                for (int i = 1; i < 61; i++)
                {
                    scrollViewItem = Instantiate(pickerItem, scrollContentArea.transform);
                    if (i < 10)
                    {
                        scrollViewItem.name = "0" + i.ToString();
                        scrollViewItem.GetComponent<TextMeshProUGUI>().text = "0" + i.ToString();
                    }
                    else
                    {
                        scrollViewItem.name = i.ToString();
                        scrollViewItem.GetComponent<TextMeshProUGUI>().text = i.ToString();
                    }
                }
            }
        }
    }

    // -------------------------------------------------------------------------------
    // Given the currently selected month and year set the correct range of days for the 
    // day scrollview and account for leap years.
    // -------------------------------------------------------------------------------
    void UpdateDaysList(int month, int year)
    {
        List<int> daysInMonth = new List<int> { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        // start clean
        for (int cc = 0; cc < ddContentArea.transform.childCount; cc++)
        {
            Destroy(ddContentArea.transform.GetChild(cc).gameObject);
        }

        var maxDays = daysInMonth[month];

        // check for leap year
        if (month == 1 && DateTime.IsLeapYear(year))
        {
            maxDays = 29;
        }

        // rebuild the day scrollview items
        for (int i = 1; i <= maxDays; i++)
        {
            scrollViewItem = Instantiate(pickerItem, ddContentArea.transform);
            scrollViewItem.name = i.ToString();
            scrollViewItem.GetComponent<TextMeshProUGUI>().text = i.ToString();
        }
    }
}
