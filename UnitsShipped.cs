/* 
 
Author: Angelica Kuik #100849912
 Date: February 22, 2022
 Course: .NET
 Application: UnitsShippedCalculator - VERSION 2
 Description: UnitsShippedCalculator VERSION 2 is a Windows Forms application that enables the user to
              enter the number of units shipped each day of the week for 3 different regions, validates the user input, 
              calculates the average units per region and the overall average units shipped for all three regions.

              UnitsShippedCalculator VERSION 2 is an updated version of the application created for Lab 1 that has gained
              two more regions and incorporated the use of 2 dimensional arrays following the requirements of Lab 3.
 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UnitsShippedCalculator
{
    public partial class formUnitsShipped : Form
    {
        //Variable Declarations:
        int rowIndexArray = 0;
        int columnIndexArray = 0;


        const int maxUnitsShipped = 5000;
        const int minUnitsShipped = 0;
        const int numberRows = 3;
        const int numberDays = 7;
        double averageShippedUnits = 0.0;
        int sum = 0;

        //Create a two dimensional array to hold the units shipped from each region.
        int[,] unitsShippedArray = new int[numberRows, numberDays];

        //Create an array of controls for the regions and for the average labels
        TextBox[] outputTextboxesArray;
        Label[] outputRegionAverageArray;

        //Create an array to hold the average from each region
        double[] averageRegionArray = new double[numberRows];

        public formUnitsShipped()
        {
            InitializeComponent();

            outputTextboxesArray = new TextBox[] { textBoxPastEntriesRegion1, textBoxPastEntriesRegion2, textBoxPastEntriesRegion3 };

            outputRegionAverageArray = new Label[] { labelRegion1Output, labelRegion2Output, labelRegion3Output };

        }
        #region Functions
        /// <summary>
        /// Given an array of controls, this function clears their text.
        /// </summary>
        /// <param name="controlArray">An array of controls with a text property to clear</param>
        private void ClearControls(Control[] controlArray)
        {
            foreach (Control controlToClear in controlArray)
            {
                controlToClear.Text = String.Empty;
            }
        }

        /// <summary>
        /// Clears all controls and sets form to its default state.
        /// </summary>
        public void SetDefaults()
        {
            // Clear all input & output fields:
            ClearControls(outputRegionAverageArray);
            ClearControls(outputTextboxesArray);
            labelGeneralAverageOutput.Text = String.Empty;

            // Reset focus to the input textbox:
            textBoxUnitsInput.Focus();

            // Reset the variable that holds the average of shipped units:
            averageShippedUnits = 0.0;

            // Reset the variable that holds the count of each input entered by the user:
            rowIndexArray = 0;
            columnIndexArray = 0;

            // Reset the variable that holds the sum of inputs:
            sum = 0;

            // Reset buttonEnter to enable user to enter new set of inputs:
            buttonEnter.Enabled = true;

            // Disable textBoxUnitsInput from ReadOnly mode:
            textBoxUnitsInput.ReadOnly = false;

            //Set labels background color to default
            foreach (Label label in outputRegionAverageArray)
            {
                label.BackColor = Color.LightGray;
            }
        }

        #endregion

        /// <summary>
        /// Closes the application.
        /// </summary>
        private void buttonExit_Click(object sender, EventArgs e)
        {
            Close();
        }
        /// <summary>
        /// Clears the input and output fields and resets form to its default state.
        /// </summary>
        private void buttonReset_Click(object sender, EventArgs e)
        {
            SetDefaults();
        }
        /// <summary>
        /// Accepts inputs, calculate average and display output to the user.
        /// </summary>
        private void buttonEnter_Click(object sender, EventArgs e)
        {
            //Local Variable Declarations
            int shippedUnitsInput;


            //Validate user input:
            if (int.TryParse(textBoxUnitsInput.Text, out shippedUnitsInput))
            {
                // Validate if user input is within range:
                if (shippedUnitsInput >= minUnitsShipped && shippedUnitsInput <= maxUnitsShipped)
                {
                    unitsShippedArray[rowIndexArray, columnIndexArray] = shippedUnitsInput;
                    outputTextboxesArray[rowIndexArray].Text += shippedUnitsInput + Environment.NewLine;


                    sum += unitsShippedArray[rowIndexArray, columnIndexArray];

                    columnIndexArray++;


                    // Clear textBoxUnitInput after each entry:
                    textBoxUnitsInput.Clear();

                    labelGeneralAverageOutput.Text = String.Empty;


                    // Enables user to enter inputs for all 7 days of the week:
                    if (columnIndexArray == unitsShippedArray.GetLength(1))
                    {
                        // Calculate and display region average 
                        averageShippedUnits = (double)sum / unitsShippedArray.GetLength(1);

                        // Display average shipped units for region the region.
                        outputRegionAverageArray[rowIndexArray].Text = string.Format("Average: {0}", Math.Round(averageShippedUnits, 2, MidpointRounding.ToEven));

                        //Add value to the array with all region averages
                        averageRegionArray[rowIndexArray] = averageShippedUnits;

                        //Increment row index
                        rowIndexArray++;

                        //Reset column index 
                        columnIndexArray = 0;

                        //Reset sum
                        sum = 0;

                        if (rowIndexArray == unitsShippedArray.GetLength(0))
                        {
                            //Calculate overall average
                            for (int row = 0; row < numberRows; row++)
                            {
                                for (int column = 0; column < numberDays; column++)
                                {
                                    sum += unitsShippedArray[row, column];
                                }
                            }

                            averageShippedUnits = sum / unitsShippedArray.Length;

                            // Display overall average shipped units for all 3 regions.
                            labelGeneralAverageOutput.Text = string.Format("Average: {0}", Math.Round(averageShippedUnits, 2, MidpointRounding.ToEven));


                            // Disable buttonEnter after inputs for all 7 days were entered.
                            buttonEnter.Enabled = false;

                            // Set input textbox to read only so the user can't enter new inputs:
                            textBoxUnitsInput.ReadOnly = true;

                            //Set backgroung color of region average output to red if < than overall average.
                            for (int i = 0; i < numberRows; i++)
                            {
                                if (averageRegionArray[i] < averageShippedUnits)
                                {
                                    outputRegionAverageArray[i].BackColor = Color.Red;
                                }

                                if (averageRegionArray[i] > averageShippedUnits)
                                {
                                    outputRegionAverageArray[i].BackColor = Color.LightGreen;
                                }
                            }
                         
                        }

                    }
                }
                else
                {
                    // If range validation fails display the error message below:
                    labelGeneralAverageOutput.Text = "Please enter a number between " + minUnitsShipped + " and " + maxUnitsShipped;

                    // If validation fails reset focus on the input textbox so user can enter new input. 
                    textBoxUnitsInput.SelectAll();
                    textBoxUnitsInput.Focus();
                }
            }
            else
            {
                // If range validation fails display the error message below:
                labelGeneralAverageOutput.Text = "Please enter a whole number.";

                // If validation fails reset focus on the input textbox so user can enter new input.
                textBoxUnitsInput.SelectAll();
                textBoxUnitsInput.Focus();
            }

        }
    }

}

