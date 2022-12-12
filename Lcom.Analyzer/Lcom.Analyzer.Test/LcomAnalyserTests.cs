using Microsoft;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VerifyCS = Lcom.Analyzer.Test.CSharpAnalyzerVerifier<Lcom.Analyzer.LcomAnalyzer>;

namespace Lcom.Analyzer.Test;

[TestClass]
public class LcomAnalyserTests
{
    [TestMethod]
    public async Task TestWithKettle()
    {
        

        var expectedFields = VerifyCS.Diagnostic("NH002").WithLocation(5, 1).WithArguments("Kettle", 6);
        var expectedLcom = VerifyCS.Diagnostic("NH004").WithLocation(5, 1).WithArguments("Kettle", 0.82);

        await VerifyCS.VerifyAnalyzerAsync(KettleClass, expectedFields, expectedLcom);
    }

    [TestMethod]
    public async Task TestWithDuty()
    {
        var expectedFields = VerifyCS.Diagnostic("NH002").WithLocation(6, 1).WithArguments("Duty", 7);

        await VerifyCS.VerifyAnalyzerAsync(DutyClass, expectedFields);
    }

    const string KettleClass = @"
using System;
namespace Pra.Lecon08.Kettles;

public class Kettle
{
    private const double RunOffTemperature = 100;
    private const double SecondsInOneHour = 3600;
    private const int RoomTemperature = 20;

    private readonly double _powerInKw;             //M_power = 2
    private readonly double _capacity;              //M_capacity = 2
    private double _temperature = RoomTemperature;  //M_temperature = 2
    private bool _pluggedIn = false;                //M_pluggedIn = 3
    private bool _running = false;                  //M_running = 3
    private double _volume = 0.0;                   //M_volume = 3

    public Kettle(double powerInKw, double capacityInLiters)
    {
        _powerInKw = powerInKw;
        _capacity = capacityInLiters;
    }

    public void PlugIn() 
    {
        _pluggedIn = true;
    }

    public void PlugOut()
    {
        if(PluggedIn)
        {
            _pluggedIn = false;
            _running = false;
        }
    }

    public void Switch() 
    {
        if(PluggedIn)
        {
            _running = !_running;
        }
    }

    public void FillIn(double volume) 
    {
        if(Volume + volume > Capacity)
        {
            throw new ArgumentException(""Adding this volume would cause water overflow"", nameof(volume));
        }
        _volume += volume;
    }

    public void FillOut(double volume) 
    {
        if(Volume - volume < 0)
        {
            throw new ArgumentException(""Subtracting this volume would cause water underflow"", nameof(volume));
        }
        _volume -= volume;
    }

    public void Wait(int seconds)
    {
        double secondsLeftBeforeMaxTemp = ComputeSecondsLeftBeforeRunningOff();

        if (Running)
        {
            if (seconds > secondsLeftBeforeMaxTemp)
            {
                Switch();
            }
                
            _temperature += (seconds / secondsLeftBeforeMaxTemp) * (RunOffTemperature - _temperature);
            _temperature = Math.Min(RunOffTemperature, _temperature);
        }
        else
        {
            _temperature -= (seconds / secondsLeftBeforeMaxTemp) * (RunOffTemperature - _temperature);
            _temperature = Math.Max(RoomTemperature, _temperature);
        }
    }

    private double ComputeSecondsLeftBeforeRunningOff()
    {
        var usedPower = 4.2 * Volume * (RunOffTemperature - Temperature)/3600;
        var hoursLeftBeforeMaxTemp = usedPower / Power;
        var secondsLeftBeforeMaxTemp = hoursLeftBeforeMaxTemp * SecondsInOneHour;
        return secondsLeftBeforeMaxTemp;
    }

    public double Power => _powerInKw;
    public double Capacity => _capacity;
    public double Volume => _volume;
    public double Temperature => _temperature;
    public bool Running => _running;
    public bool PluggedIn => _pluggedIn;
}";
    const string DutyClass = @"
using System;

namespace Pra.Lecon08.Kettles;

public class Duty
{
    public string SetupName { get; set; } = string.Empty;// 0

    public string Description { get; set; } = string.Empty; //0

    public string Performer { get; set; }//1

    public DateTime PlannedStart { get; set; } // 0
    public DateTime PlannedEnd { get; set; }   // 1

    public DateTime? ActualStart { get; set; } //3
    public DateTime? ActualEnd { get; set; }   //5

    public int DaysLate 
        => ActualEndDateOrToday <= PlannedEnd ? 0 : (ActualEndDateOrToday - PlannedEnd).Days;
    private DateTime ActualEndDateOrToday 
        => ActualEnd.HasValue ? ActualEnd.Value : DateTime.Today;

    public DutyStatus Status =>
        !ActualStart.HasValue ? DutyStatus.ToDo :
        !ActualEnd.HasValue ? DutyStatus.Doing :
        DutyStatus.Done;

    public bool HasStarted => ActualStart.HasValue;

    public bool IsFinished => ActualEnd.HasValue;

    public bool HasPerformer(string performer) => Performer == performer;

    public void Update(DutyUpdateAction action)
    {
        switch(action)
        { 
            case DutyUpdateAction.Open: 
                ActualStart = DateTime.Now;
                break;
            case DutyUpdateAction.Finish:
                ActualEnd = DateTime.Now;
                break;
        }
    }
}

public enum DutyStatus
{
    ToDo = 0,
    Doing = 1,
    Done = 2
}

public enum DutyUpdateAction
{
    Open = 0,
    Finish = 1
}";
}
