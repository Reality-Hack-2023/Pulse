using UnityEngine;
using System.IO;

// Raw file will have the raw data from the sensors. Each line will be a new data point and has the following format:
// heart_rate,oxygen,temp,velocity (all in string format, as their values)
// Example: 80,98,98,0.5

public class DataDriver : MonoBehaviour
{
    //path to the files
    string rawPath;
    string baselinePath;
    string assessmentPath;

    // Use this for initialization
    void Start()
    {
        //get the file paths
        rawPath = Application.persistentDataPath + "/raw.txt";
        baselinePath = Application.persistentDataPath + "/baseline.txt";
        assessmentPath = Application.persistentDataPath + "/assessment.txt";
    }

    // 1 - RAW FILE METHODS

    //method to clear the raw file
    public void ClearRaw()
    {
        //open the file
        FileStream file = new FileStream(rawPath, FileMode.Create);

        //create a writer
        StreamWriter writer = new StreamWriter(file, false);

        //write the data
        writer.WriteLine();

        //close the writer
        writer.Close();

        //close the file
        file.Close();
    }

    //method to write data to the raw file
    public void WriteRaw(string data)
    {
        //open the file
        FileStream file = new FileStream(rawPath, FileMode.Create);

        //create a writer
        StreamWriter writer = new StreamWriter(file);

        //write the data
        writer.WriteLine(data);

        //close the writer
        writer.Close();

        //close the file
        file.Close();
    }

    //method to get the average heart rate from the raw file
    public float GetAverageHeartRate()
    {
        return File.ReadLines(rawPath).Select(line => float.Parse(line.Split(',')[0])).Average();
    }

    //method to get the average oxygen from the raw file
    public float GetAverageOxygen()
    {
        return File.ReadLines(rawPath).Select(line => float.Parse(line.Split(',')[1])).Average();
    }

    //method to get the average temperature from the raw file
    public float GetAverageTemp()
    {
        return File.ReadLines(rawPath).Select(line => float.Parse(line.Split(',')[2])).Average();
    }

    //method to get the average velocity from the raw file
    public float GetAverageVelocity()
    {
        return File.ReadLines(rawPath).Select(line => float.Parse(line.Split(',')[3])).Average();
    }

    // 2 - BASELINE FILE METHODS

    //method to write data to the baseline file
    public void WriteBaseline()
    {
        //open the file
        FileStream file = new FileStream(baselinePath, FileMode.Create);

        //create a writer
        StreamWriter writer = new StreamWriter(file, false);

        //write the data
        writer.WriteLine("" + GetAverageHeartRate() + "," + GetAverageOxygen() + "," + GetAverageTemp() + "," + GetAverageVelocity());

        //close the writer
        writer.Close();

        //close the file
        file.Close();
    }

    //method to get the heart rate from the baseline file
    public float GetBaselineHeartRate()
    {
        return File.ReadLines(baselinePath).Select(line => float.Parse(line.Split(',')[0]));
    }

    //method to get the oxygen from the baseline file
    public float GetBaselineOxygen()
    {
        return File.ReadLines(baselinePath).Select(line => float.Parse(line.Split(',')[1]));
    }

    //method to get the temperature from the baseline file
    public float GetBaselineTemperature()
    {
        return File.ReadLines(baselinePath).Select(line => float.Parse(line.Split(',')[2]));
    }

    //method to get the heart rate from the baseline file
    public float GetBaselineVelocity()
    {
        return File.ReadLines(baselinePath).Select(line => float.Parse(line.Split(',')[3]));
    }

    // 3 - ASSESSMENT FILE METHODS

    //method to write data to the assessment file
    public void WriteAssessment()
    {
        //open the file
        FileStream file = new FileStream(assessmentPath, FileMode.Create);

        //create a writer
        StreamWriter writer = new StreamWriter(file, false);

        //write the data
        writer.WriteLine("" + GetAverageHeartRate() + "," + GetAverageOxygen() + "," + GetAverageTemp() + "," + GetAverageVelocity());

        //close the writer
        writer.Close();

        //close the file
        file.Close();
    }

    //method to get the heart rate from the assessment file
    public float GetAssessmentHeartRate()
    {
        return File.ReadLines(assessmentPath).Select(line => float.Parse(line.Split(',')[0]));
    }

    //method to get the oxygen from the assessment file
    public float GetAssessmentOxygen()
    {
        return File.ReadLines(assessmentPath).Select(line => float.Parse(line.Split(',')[1]));
    }

    //method to get the temperature from the assessment file
    public float GetAssessmentTemperature()
    {
        return File.ReadLines(assessmentPath).Select(line => float.Parse(line.Split(',')[2]));
    }

    //method to get the heart rate from the assessment file
    public float GetAssessmentVelocity()
    {
        return File.ReadLines(assessmentPath).Select(line => float.Parse(line.Split(',')[3]));
    }

    // 4 - OTHER METHODS

    // method to get the difference between the baseline and assessment heart rate
    public float GetHeartRateDifference()
    {
        return GetAssessmentHeartRate() - GetBaselineHeartRate();
    }

    // method to get the difference between the baseline and assessment oxygen
    public float GetOxygenDifference()
    {
        return GetAssessmentOxygen() - GetBaselineOxygen();
    }

    // method to get the difference between the baseline and assessment temperature
    public float GetTemperatureDifference()
    {
        return GetAssessmentTemperature() - GetBaselineTemperature();
    }

    // method to get the difference between the baseline and assessment velocity
    public float GetVelocityDifference()
    {
        return GetAssessmentVelocity() - GetBaselineVelocity();
    }
}
